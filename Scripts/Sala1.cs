using Godot;
using System;
using System.Collections.Generic;

public partial class Sala1 : Node2D
{
	GameState gameState;
	AudioStreamPlayer2D musica;
	AudioStreamPlayer2D somDialogo;

	// Falas por (dia, local). Se não existir entrada para o dia/local, não há fala.
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
		musica = GetNode<AudioStreamPlayer2D>("MusicaPrincipal");
		//somDialogo = GetNode<AudioStreamPlayer2D>("somDialogo");

		// estado inicial: começa no menu
		GetNode<Sprite2D>("Menu").Show();
		GetNode<Button>("SairEspelho").Hide();
		GetNode<Sprite2D>("Sprite2D").Hide();
		GetNode<Sprite2D>("Fundo2").Hide();
		GetNode<Node2D>("SalaDeEstar").Hide();
		GetNode<Sprite2D>("FundoEspelho").Hide();
		GetNode<CanvasItem>("Cozinha").Hide();
		GetNode<Button>("Comecar").Show();

		GetNode<Button>("Botao").Hide();
		GetNode<Button>("Salaestar").Hide();
		GetNode<Button>("BotaoVoltar").Hide();
		GetNode<Button>("TarefaEscolar").Hide();
		GetNode<Button>("VoltarSalaQuarto").Hide();
		GetNode<Button>("Espelho").Hide();
		GetNode<Button>("IrCozinha").Hide();
		GetNode<Button>("VoltarPSala").Hide();
		GetNode<Button>("NextDay").Hide();
		GetNode<CanvasLayer>("Dialogo").Hide();

		// Avançar diálogo / fechar a caixa no fim
		GetNode<Button>("Dialogo/Caixa/Avancar").Pressed += () =>
		{
			if (falasAtuais == null) return;

			falaAtual++;

			if (falaAtual < falasAtuais.Length)
				MostrarFala(falasAtuais[falaAtual]);
			else
				GetNode<CanvasLayer>("Dialogo").Hide();
		};

		// Comecar: menu -> quarto
		GetNode<Button>("Comecar").Pressed += () =>
		{
			GetNode<Sprite2D>("Menu").Hide();
			GetNode<Button>("Comecar").Hide();
			GetNode<Sprite2D>("Sprite2D").Show();
			GetNode<Sprite2D>("Fundo2").Hide();
			GetNode<Node2D>("SalaDeEstar").Hide();
			GetNode<Sprite2D>("FundoEspelho").Hide();
			GetNode<CanvasItem>("Cozinha").Hide();

			GetNode<Button>("Botao").Show();
			GetNode<Button>("Salaestar").Show();
			GetNode<Button>("BotaoVoltar").Hide();
			GetNode<Button>("TarefaEscolar").Hide();
			GetNode<Button>("VoltarSalaQuarto").Hide();
			GetNode<Button>("Espelho").Hide();
			GetNode<Button>("IrCozinha").Hide();
			GetNode<Button>("VoltarPSala").Hide();
			GetNode<Button>("NextDay").Visible = gameState.PossoFinalizarDia();
		};

		// Botao: quarto -> banheiro
		GetNode<Button>("Botao").Pressed += () =>
		{
			GetNode<Button>("Comecar").Hide();
			GetNode<Sprite2D>("FundoEspelho").Hide();
			GetNode<Sprite2D>("Sprite2D").Hide();
			GetNode<Node2D>("SalaDeEstar").Hide();
			GetNode<Sprite2D>("Fundo2").Show();
			GetNode<CanvasItem>("Cozinha").Hide();

			GetNode<Button>("Botao").Hide();
			GetNode<Button>("Salaestar").Hide();
			GetNode<Button>("BotaoVoltar").Show();
			GetNode<Button>("TarefaEscolar").Hide();
			GetNode<Button>("VoltarSalaQuarto").Hide();
			GetNode<Button>("Espelho").Show();
			GetNode<Button>("NextDay").Hide();
			GetNode<Button>("SairEspelho").Hide();
			GetNode<Button>("IrCozinha").Hide();
			GetNode<Button>("VoltarPSala").Hide();
		};

		// Espelho: banheiro -> espelho (fala do espelho, só na primeira vez do dia)
		GetNode<Button>("Espelho").Pressed += () =>
		{
			GetNode<Button>("Comecar").Hide();
			GetNode<Node2D>("SalaDeEstar").Hide();
			GetNode<Sprite2D>("Fundo2").Hide();
			GetNode<Sprite2D>("FundoEspelho").Show();
			GetNode<Sprite2D>("Sprite2D").Hide();
			GetNode<CanvasItem>("Cozinha").Hide();
			GetNode<Button>("VoltarSalaQuarto").Hide();
			GetNode<Button>("TarefaEscolar").Hide();
			GetNode<Button>("BotaoVoltar").Hide();
			GetNode<Button>("SairEspelho").Show();
			GetNode<Button>("IrCozinha").Hide();
			GetNode<Button>("VoltarPSala").Hide();
			TentarFalar("Espelho");
		};

		// SairEspelho: espelho -> banheiro
		GetNode<Button>("SairEspelho").Pressed += () =>
		{
			GetNode<Button>("Comecar").Hide();
			GetNode<Sprite2D>("FundoEspelho").Hide();
			GetNode<Sprite2D>("Sprite2D").Hide();
			GetNode<Node2D>("SalaDeEstar").Hide();
			GetNode<Sprite2D>("Fundo2").Show();
			GetNode<CanvasItem>("Cozinha").Hide();
			GetNode<Button>("SairEspelho").Hide();
			GetNode<CanvasLayer>("Dialogo").Hide();

			GetNode<Button>("Botao").Hide();
			GetNode<Button>("Salaestar").Hide();
			GetNode<Button>("BotaoVoltar").Show();
			GetNode<Button>("TarefaEscolar").Hide();
			GetNode<Button>("VoltarSalaQuarto").Hide();
			GetNode<Button>("Espelho").Show();
			GetNode<Button>("NextDay").Hide();
			GetNode<Button>("IrCozinha").Hide();
			GetNode<Button>("VoltarPSala").Hide();
		};

		// BotaoVoltar: banheiro -> quarto
		GetNode<Button>("BotaoVoltar").Pressed += () =>
		{
			GetNode<Button>("Comecar").Hide();
			GetNode<Sprite2D>("FundoEspelho").Hide();
			GetNode<Sprite2D>("Fundo2").Hide();
			GetNode<Node2D>("SalaDeEstar").Hide();
			GetNode<Sprite2D>("Sprite2D").Show();
			GetNode<CanvasItem>("Cozinha").Hide();

			GetNode<Button>("TarefaEscolar").Hide();
			GetNode<Button>("BotaoVoltar").Hide();
			GetNode<Button>("VoltarSalaQuarto").Hide();
			GetNode<Button>("Botao").Show();
			GetNode<Button>("Salaestar").Show();
			GetNode<Button>("Espelho").Hide();
			GetNode<Button>("NextDay").Visible = gameState.PossoFinalizarDia();
			GetNode<Button>("IrCozinha").Hide();
			GetNode<Button>("VoltarPSala").Hide();
		};

		// Salaestar: quarto -> sala de estar
		GetNode<Button>("Salaestar").Pressed += () =>
		{
			GetNode<Button>("Comecar").Hide();
			GetNode<Sprite2D>("FundoEspelho").Hide();
			GetNode<Sprite2D>("Sprite2D").Hide();
			GetNode<Sprite2D>("Fundo2").Hide();
			GetNode<Node2D>("SalaDeEstar").Show();
			GetNode<CanvasItem>("Cozinha").Hide();

			GetNode<Button>("Salaestar").Hide();
			GetNode<Button>("Botao").Hide();
			GetNode<Button>("BotaoVoltar").Hide();
			GetNode<Button>("TarefaEscolar").Visible = gameState.PossoFazerATarefa(GameState.TarefaEscolar);
			// Pintura: só aparece nos dias em que Pintar está na lista (no dia 1 não está)
			GetNode<Button>("SalaDeEstar/Cavalete").Visible = gameState.PossoFazerATarefa(GameState.Pintar);
			GetNode<Button>("VoltarSalaQuarto").Show();
			GetNode<Button>("Espelho").Hide();
			GetNode<Button>("NextDay").Hide();
			GetNode<Button>("IrCozinha").Show();
			GetNode<Button>("VoltarPSala").Hide();
		};

		// VoltarSalaQuarto: sala de estar -> quarto
		GetNode<Button>("VoltarSalaQuarto").Pressed += () =>
		{
			GetNode<Button>("Comecar").Hide();
			GetNode<Sprite2D>("FundoEspelho").Hide();
			GetNode<Node2D>("SalaDeEstar").Hide();
			GetNode<Sprite2D>("Fundo2").Hide();
			GetNode<Sprite2D>("Sprite2D").Show();
			GetNode<CanvasItem>("Cozinha").Hide();

			GetNode<Button>("VoltarSalaQuarto").Hide();
			GetNode<Button>("TarefaEscolar").Hide();
			GetNode<Button>("BotaoVoltar").Hide();
			GetNode<Button>("Botao").Show();
			GetNode<Button>("Salaestar").Show();
			GetNode<Button>("Espelho").Hide();
			GetNode<Button>("NextDay").Visible = gameState.PossoFinalizarDia();
			GetNode<Button>("IrCozinha").Hide();
			GetNode<Button>("VoltarPSala").Hide();
		};

		// IrCozinha: sala de estar -> cozinha (fala da cozinha, só na primeira vez do dia)
		GetNode<Button>("IrCozinha").Pressed += () =>
		{
			GetNode<Button>("Comecar").Hide();
			GetNode<Sprite2D>("FundoEspelho").Hide();
			GetNode<Sprite2D>("Sprite2D").Hide();
			GetNode<Sprite2D>("Fundo2").Hide();
			GetNode<Node2D>("SalaDeEstar").Hide();
			GetNode<CanvasItem>("Cozinha").Show();

			GetNode<Button>("Salaestar").Hide();
			GetNode<Button>("Botao").Hide();
			GetNode<Button>("BotaoVoltar").Hide();
			GetNode<Button>("TarefaEscolar").Hide();
			GetNode<Button>("VoltarSalaQuarto").Hide();
			GetNode<Button>("Espelho").Hide();
			GetNode<Button>("NextDay").Hide();
			GetNode<Button>("IrCozinha").Hide();
			GetNode<Button>("VoltarPSala").Show();
			TentarFalar("Cozinha");
		};

		// VoltarPSala: cozinha -> sala de estar
		GetNode<Button>("VoltarPSala").Pressed += () =>
		{
			GetNode<Button>("Comecar").Hide();
			GetNode<Sprite2D>("FundoEspelho").Hide();
			GetNode<Sprite2D>("Sprite2D").Hide();
			GetNode<Sprite2D>("Fundo2").Hide();
			GetNode<CanvasItem>("Cozinha").Hide();
			GetNode<Node2D>("SalaDeEstar").Show();
			GetNode<CanvasLayer>("Dialogo").Hide();

			GetNode<Button>("Salaestar").Hide();
			GetNode<Button>("Botao").Hide();
			GetNode<Button>("BotaoVoltar").Hide();
			GetNode<Button>("Espelho").Hide();
			GetNode<Button>("NextDay").Hide();
			GetNode<Button>("VoltarPSala").Hide();
			GetNode<Button>("TarefaEscolar").Visible = gameState.PossoFazerATarefa(GameState.TarefaEscolar);
			GetNode<Button>("SalaDeEstar/Cavalete").Visible = gameState.PossoFazerATarefa(GameState.Pintar);
			GetNode<Button>("VoltarSalaQuarto").Show();
			GetNode<Button>("IrCozinha").Show();
		};

		// Texto ao passar o mouse: Espelho
		Button espelho = GetNode<Button>("Espelho");
		Label textoEspelho = GetNode<Label>("Espelho/TextoEspelho");
		textoEspelho.Hide();
		espelho.MouseEntered += () => textoEspelho.Show();
		espelho.MouseExited += () => textoEspelho.Hide();

		// Texto ao passar o mouse: TarefaEscolar
		Button tarefaEscolar = GetNode<Button>("TarefaEscolar");
		Label textoTarefa = GetNode<Label>("TarefaEscolar/TextoTarefaEscolar");
		textoTarefa.Hide();
		tarefaEscolar.MouseEntered += () => textoTarefa.Show();
		tarefaEscolar.MouseExited += () => textoTarefa.Hide();

		// Próximo dia
		Button nextDay = GetNode<Button>("NextDay");
		Label textoNextDay = GetNode<Label>("NextDay/TextoNextDay");
		textoNextDay.Hide();
		nextDay.MouseEntered += () => textoNextDay.Show();
		nextDay.MouseExited += () => textoNextDay.Hide();
		nextDay.Pressed += OnButtonPressed;

		// Tarefa escolar
		GetNode<Button>("TarefaEscolar").Pressed += () =>
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
				GetNode<Button>("TarefaEscolar").Visible = gameState.PossoFazerATarefa(GameState.TarefaEscolar);
			};
		};
	}

	// Fala só se existir fala para este dia/local e ainda não foi vista
	private void TentarFalar(string local)
	{
		var chave = (gameState.Day, local);

		if (!falas.TryGetValue(chave, out var linhas)) return; // sem fala nesse dia/local
		if (falasVistas.Contains(chave)) return;               // já falou aqui hoje

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
		GetNode<Button>("NextDay").Visible = gameState.PossoFinalizarDia();
	}

	public override void _UnhandledInput(InputEvent @event)
	{
		if (@event is InputEventMouseButton { Pressed: true })
			GD.Print("MOUSE CLICADO!");
	}
}