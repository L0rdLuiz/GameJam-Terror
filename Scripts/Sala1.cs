using Godot;
using System;
using System.Collections.Generic;

public partial class Sala1 : Node2D
{
	GameState gameState;
	AudioStreamPlayer2D musica;
	AudioStreamPlayer2D somDialogo;

	Node2D[] comodos;

	// Falas (dia, local).
	Dictionary<(int dia, string local), string[]> falas = new()
	{
		{ (1, "Espelho"), new[]
			{
				"Good morning, Cherry",
				"All right... what do we have planned for today?",
				"Let's see... lunch, getting Sugarplum ready for school, folding the laundry...",
				"His first day of school. I wonder if he's excited.",
				"He's growing up so fast.",
				"Well, I should get started. No time to waste!"
			}
		},
		{ (1, "Cozinha"), new[]
			{
				"All right, let's get everything ready.",
				"I should prepare lunch first."
			}
		},

		// Exemplo de dia 2:
		// { (2, "Espelho"), new[] { "Another day...", "I hope today goes smoothly." } },
	};

	// Guarda o que já foi falado, para cada fala acontecer só na primeira vez no dia
	HashSet<(int, string)> falasVistas = new();

	string[] falasAtuais;
	int falaAtual = 0;

	private void TocarMusica(string caminho)
	{
		var nova = GD.Load<AudioStream>(caminho);
		if (musica.Stream == nova && musica.Playing)
			return;

		musica.Stream = nova;
		musica.Play();
	}

	public override void _Ready()
	{
		gameState = GetNode<GameState>("/root/GameState");
		gameState.DiaMudou += AoDiaMudar;
		gameState.TarefaConcluida += AoTarefaConcluir;
		musica = GetNode<AudioStreamPlayer2D>("MusicaPrincipal");
		//somDialogo = GetNode<AudioStreamPlayer2D>("somDialogo");

		var menu = GetNode<Node2D>("ComodoMenu");
		var quarto = GetNode<Node2D>("ComodoQuarto");
		var banheiro = GetNode<Node2D>("ComodoBanheiro");
		var espelho = GetNode<Node2D>("ComodoEspelho");
		var sala = GetNode<Node2D>("SalaDeEstar");
		var cozinha = GetNode<Node2D>("ComodoCozinha");
		comodos = new[] { menu, quarto, banheiro, espelho, sala, cozinha };

		MostrarComodo(menu);

		GetNode<Button>("%BotaoComecar").Pressed += () => MostrarComodo(quarto);

		GetNode<Button>("%IrBanheiro").Pressed += () => MostrarComodo(banheiro);
		GetNode<Button>("%VoltarBanheiro").Pressed += () => MostrarComodo(quarto);

		// Espelho: fala só na primeira vez do dia
		GetNode<Button>("%Espelho").Pressed += () =>
		{
			MostrarComodo(espelho);
			TentarFalar("Espelho");
		};
		GetNode<Button>("%SairEspelho").Pressed += () => MostrarComodo(banheiro);

		GetNode<Button>("%IrSala").Pressed += () => MostrarComodo(sala);
		GetNode<Button>("%VoltarDaSala").Pressed += () => MostrarComodo(quarto);

		// Cozinha: fala só na primeira vez do dia
		GetNode<Button>("%IrCozinha").Pressed += () =>
		{
			MostrarComodo(cozinha);
			TentarFalar("Cozinha");
		};
		GetNode<Button>("%VoltarDaCozinha").Pressed += () => MostrarComodo(sala);

		GetNode<Button>("Dialogo/Caixa/Avancar").Pressed += () =>
		{
			if (falasAtuais == null) return;

			falaAtual++;

			if (falaAtual < falasAtuais.Length)
				MostrarFala(falasAtuais[falaAtual]);
			else
				GetNode<CanvasLayer>("Dialogo").Hide();
		};

		Button botaoEspelho = GetNode<Button>("%Espelho");
		Label textoEspelho = GetNode<Label>("%TextoEspelho");
		textoEspelho.Hide();
		botaoEspelho.MouseEntered += () => textoEspelho.Show();
		botaoEspelho.MouseExited += () => textoEspelho.Hide();

		// TarefaEscolar
		Button tarefaEscolar = GetNode<Button>("%TarefaEscolar");
		Label textoTarefa = GetNode<Label>("%TextoTarefaEscolar");
		textoTarefa.Hide();
		tarefaEscolar.MouseEntered += () => textoTarefa.Show();
		tarefaEscolar.MouseExited += () => textoTarefa.Hide();

		// Próximo dia
		Button nextDay = GetNode<Button>("%ProximoDia");
		Label textoNextDay = GetNode<Label>("%TextoNextDay");
		textoNextDay.Hide();
		nextDay.MouseEntered += () => textoNextDay.Show();
		nextDay.MouseExited += () => textoNextDay.Hide();
		nextDay.Pressed += OnButtonPressed;

		tarefaEscolar.Pressed += () =>
		{
			if (!gameState.PossoFazerATarefa(GameState.TarefaEscolar))
			{
				GD.Print("Não pode fazer a tarefa escolar ainda!");
				return;
			}

			PackedScene tarefaEscolarScene = GD.Load<PackedScene>("res://Scenes/MinigameLicao/minigame_licao.tscn");
			Node tarefaEscolarInstance = tarefaEscolarScene.Instantiate();
			GetTree().Root.AddChild(tarefaEscolarInstance);
			Hide();

			tarefaEscolarInstance.TreeExited += () =>
			{
				Show();
				AtualizarBotoes();
			};
		};
	}

	// Mostra só um cômodo e esconde os outros
	private void MostrarComodo(Node2D comodo)
	{
		foreach (var c in comodos)
			c.Hide();

		comodo.Show();
		GetNode<CanvasLayer>("Dialogo").Hide();
		AtualizarBotoes();
	}

	private void AtualizarBotoes()
	{
		GetNode<Button>("%ProximoDia").Visible = gameState.PossoFinalizarDia();
		GetNode<Button>("%TarefaEscolar").Visible = gameState.PossoFazerATarefa(GameState.TarefaEscolar);
		GetNode<Button>("%Cavalete").Visible = gameState.PossoFazerATarefa(GameState.Pintar);
	}

	// Fala só se existir fala para este dia/local e ainda não foi vista
	private void TentarFalar(string local)
	{
		var chave = (gameState.Day, local);

		if (!falas.TryGetValue(chave, out var linhas)) return; 
		if (falasVistas.Contains(chave)) return;               

		falasVistas.Add(chave);
		falasAtuais = linhas;
		falaAtual = 0;
		MostrarFala(linhas[0]);
	}

	private void MostrarFala(string fala)
	{
		GetNode<Label>("Dialogo/Caixa/Texto").Text = fala;
		GetNode<CanvasLayer>("Dialogo").Show();
		somDialogo?.Play();
	}

	public override void _ExitTree()
	{
		if (gameState != null)
			gameState.DiaMudou -= AoDiaMudar;
		if (gameState != null)
			gameState.TarefaConcluida -= AoTarefaConcluir;
	}

	private void OnButtonPressed()
	{
		gameState.ConcluirTarefa(GameState.FinalizarDia);
		gameState.TentarVirarODia();
	}

	private void AoDiaMudar(int dia)
	{
		GD.Print("Dia mudou para: " + dia);
		GetNode<CanvasLayer>("Dialogo").Hide();
		AtualizarBotoes();
	}

	private void AoTarefaConcluir(string nomeTarefa)
	{
		AtualizarBotoes();
	}

	public override void _UnhandledInput(InputEvent @event)
	{
		if (@event is InputEventMouseButton { Pressed: true })
			GD.Print("MOUSE CLICADO!");
	}
}