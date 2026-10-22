using Godot;
using System;
using System.Collections.Generic;

public partial class Sala1 : Node2D
{
	GameState gameState;
	AudioStreamPlayer2D musica;
	AudioStreamPlayer2D somDialogo;
	Action aoTerminarFala;   // o que acontece quando o diálogo acaba
	Tween tweenTexto; //para a animação do dialogo
	Node2D[] comodos;
	private CanvasLayer painelTarefa;

	// Falas (dia, local).
	// Linhas que começam com "#" são comandos (não aparecem na caixa de diálogo)
	Dictionary<(int dia, string local), string[]> falas = new()
	{
		{ (1, "Espelho"), new[]
			{
				"Cherry|Good morning, Cherry",
				"Cherry|All right... what do we have planned for today?",
				"Cherry|Let's see... lunch, getting Sugarplum ready for school, folding the laundry...",
				"Cherry|His first day of school. I wonder if he's excited.",
				"Cherry|He's growing up so fast.",
				"Cherry|Well, I should get started. No time to waste!"
			}
		},
		{ (1, "Cozinha"), new[]
			{
				"Cherry|All right, let's get everything ready.",
				"Cherry|I should prepare lunch first."
			}
		},
		{ (1, "Mesa"), new[]
			{
				"Cherry|So... first day of school. Nervous?",
				"Sugarplum|A little.",
				"Cherry|Yeah? What about?",
				"Sugarplum|I don't know. What if nobody wants to be my friend?",
				"Cherry|I'm sure you'll find someone. You just have to talk to them.",
				"Sugarplum|But what if I don't know what to say?",
				"Cherry|Then... ask them what they like. Or if they wanna play something. That's what I used to do.",
				"Sugarplum|You did?",
				"Cherry|Of course! You think I was born knowing how to talk to people?",
				"Sugarplum giggles.",
				"Sugarplum|Kinda.",
				"Cherry|Oh, you little rascal.",
				"Cherry smiles and shakes her head, pretending to be offended.",
				"The two keep eating for a few moments.",
				"Sugarplum|Mommy?",
				"Cherry|Hmm?",
				"Sugarplum|You'll be here when I get back, right?",
				"Cherry|Of course, sweetheart. Where else would I be?",
				"Sugarplum|Okay.",
				"Sugarplum smiles, satisfied, and goes back to eating.",
				"After lunch, Cherry clears the plates while Sugarplum climbs down from the chair and grabs his backpack near the entrance.",
				"Sugarplum|I'm ready!",
				"Cherry|Already? Let me see your backpack.",
				"Cherry quickly checks his supplies and adjusts the straps on his shoulders.",
				"Cherry|Yep, looks like you've got everything.",
				"Sugarplum|Can we go now?",
				"Cherry|Hold on, let me grab my things. No need to rush.",
				"Cherry picks up her things and walks over to him.",
				"Cherry|All right, let's go.",
				"Sugarplum|Yay!",
				"Cherry takes Sugarplum's hand and the two walk to the door."
			}
		},
		{ (1, "SalaComFilho"), new[]
			{
				"Cherry bends down to adjust the backpack straps on her son's shoulders.",
				"Cherry|There. Is it too heavy?",
				"Sugarplum|Nope!",
				"Cherry|Good. Did you get everything?",
				"Sugarplum|I think so.",
				"Cherry|You think so?",
				"Cherry smiles and unzips one of the pockets to check quickly.",
				"Cherry|Looks fine to me.",
				"She closes the backpack and stands up.",
				"Sugarplum|Bye, Mommy!",
				"Cherry|Bye, sweetheart. Have a good day!",
				"Sugarplum|I will!",
				"Sugarplum starts to walk away. Cherry stays at the door, watching him for a few seconds.",
				"Cherry|And don't forget to eat your lunch!",
				"Sugarplum|I won't!",
				"#sala",
				"Cherry closes the door and stands still for a moment, her hand still on the doorknob.",
				"The sound of the street goes muffled as soon as the door closes. The house is quiet again, except for the natural ambient sounds."
			}
		},
		{ (1, "fundosala"), new[]
			{
				"Cherry|Well, that's that.",
				"Cherry walks into the kitchen, which is connected to the living room, and picks up a glass left on the table.",
				"Cherry briefly glances toward the bedroom, where a few toys are still scattered around.",
				"Cherry|I swear, he was tiny just yesterday.",
				"Cherry|First day of school already...",
				"Cherry places the glass in the sink and looks at the clock.",
				"Cherry|I've got some time before dinner.",
				"Cherry glances at the book she had set aside and decides to pick it up."
			}
		},

		// Exemplo de dia 2:
		// { (2, "Espelho"), new[] { "Another day...", "I hope today goes smoothly." } },
	};

	// Guarda o que já foi falado, para cada fala acontecer só na primeira vez no dia
	HashSet<(int, string)> falasVistas = new();

	string[] falasAtuais;
	int falaAtual = 0;

	Dictionary<string, Color> coresPersonagem = new()
	{
		{ "Cherry", new Color("ffb6c1") },
		{ "Sugarplum", new Color("87ceeb") }
	};
	Color corPadrao = Colors.White;

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
		somDialogo = GetNode<AudioStreamPlayer2D>("somDialogo");

		var menu = GetNode<Node2D>("ComodoMenu");
		var quarto = GetNode<Node2D>("ComodoQuarto");
		var banheiro = GetNode<Node2D>("ComodoBanheiro");
		var espelho = GetNode<Node2D>("ComodoEspelho");
		var sala = GetNode<Node2D>("SalaDeEstar");
		var cozinha = GetNode<Node2D>("ComodoCozinha");
		var mesa = GetNode<Node2D>("ComodoCozinhaMesa");
		var cozinhar = GetNode<Node2D>("Cozinhar");
		var salafilho = GetNode<Node2D>("SalaFilho");
		var livro = GetNode<Node2D>("ComodoLivro");

		comodos = new[] { menu, quarto, banheiro, espelho, sala, cozinha, mesa, cozinhar, salafilho, livro };
		painelTarefa = GetNode<CanvasLayer>("PainelTarefa");

		MostrarComodo(menu);

		GetNode<Button>("%BotaoComecar").Pressed += () => MostrarComodo(quarto);

		GetNode<Button>("%IrBanheiro").Pressed += () => MostrarComodo(banheiro);
		GetNode<Button>("%VoltarBanheiro").Pressed += () => MostrarComodo(quarto);

		// Espelho: fala só na primeira vez do dia
		GetNode<Button>("%Espelho").Pressed += () =>
		{
			MostrarComodo(espelho);
			TentarFalar("Espelho");
			GetNode<Button>("%SairEspelho").Show();
		};
		GetNode<Button>("%SairEspelho").Pressed += () => MostrarComodo(banheiro);

		GetNode<Button>("%IrSala").Pressed += () => MostrarComodo(sala);
		GetNode<Button>("%VoltarDaSala").Pressed += () => MostrarComodo(quarto);

		// Livro: sala -> livro -> sala
		GetNode<Button>("%Livro").Pressed += () => MostrarComodo(livro);
		GetNode<Button>("%VoltarDoLivro").Pressed += () => MostrarComodo(sala);

		// Cozinha: fala só na primeira vez do dia
		GetNode<Button>("%IrCozinha").Pressed += () =>
		{
			MostrarComodo(cozinha);
			TentarFalar("Cozinha");
		};

		GetNode<Button>("%FogaoCozinhar").Pressed += () => MostrarComodo(cozinhar);

		GetNode<Button>("%VoltarDaCozinha").Pressed += () => MostrarComodo(sala);

		
		GetNode<Button>("%IrSalaDaCozinha").Pressed += () =>
		{
			MostrarComodo(salafilho);
			TentarFalar("SalaComFilho", () => TentarFalar("fundosala"));
		};

		GetNode<Button>("%sair").Pressed += () =>
		{
			MostrarComodo(mesa);
			TentarFalar("Mesa");
		};
		GetNode<Button>("%SairLivro").Pressed += () => MostrarComodo(sala);

		GetNode<Button>("Dialogo/Caixa/Avancar").Pressed += () =>
		{
			if (falasAtuais == null) return;

			// Se ainda está digitando, mostra tudo de uma vez
			if (tweenTexto != null && tweenTexto.IsRunning())
			{
				tweenTexto.Kill();
				somDialogo?.Stop();
				GetNode<Label>("Dialogo/Caixa/Texto").VisibleRatio = 1;
				return;
			}

			falaAtual++;
			MostrarProximaLinha();
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

	// Mostra só um cômodo. Se fecharDialogo for false, a caixa de diálogo continua aberta
	private void MostrarComodo(Node2D comodo, bool fecharDialogo = true)
	{
		foreach (var c in comodos)
			c.Hide();

		comodo.Show();

		// O primeiro cômodo do array é o menu
		bool estaNoMenu = comodo == comodos[0];
		painelTarefa.Visible = !estaNoMenu;

		if (fecharDialogo)
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
	private void TentarFalar(string local, Action aoTerminar = null)
	{
		var chave = (gameState.Day, local);

		if (!falas.TryGetValue(chave, out var linhas)) return;
		if (falasVistas.Contains(chave)) return;

		falasVistas.Add(chave);
		falasAtuais = linhas;
		falaAtual = 0;
		aoTerminarFala = aoTerminar;
		MostrarProximaLinha();
	}

	// Mostra a linha atual. Linhas que começam com "#" são comandos
	private void MostrarProximaLinha()
	{
		while (falaAtual < falasAtuais.Length && falasAtuais[falaAtual].StartsWith("#"))
		{
			ExecutarComando(falasAtuais[falaAtual]);
			falaAtual++;
		}

		if (falaAtual < falasAtuais.Length)
			MostrarFala(falasAtuais[falaAtual]);
		else
			FimDoDialogo();
	}

	private void ExecutarComando(string comando)
	{
		switch (comando)
		{
			case "#sala":
				MostrarComodo(GetNode<Node2D>("SalaDeEstar"), false);
				break;
		}
	}

	private void FimDoDialogo()
	{
		GetNode<CanvasLayer>("Dialogo").Hide();

		var acao = aoTerminarFala;
		aoTerminarFala = null;   // limpa antes, para não repetir
		acao?.Invoke();
	}

	private void FecharDialogo()
	{
		tweenTexto?.Kill();
		somDialogo?.Stop();
		GetNode<CanvasLayer>("Dialogo").Hide();
	}

	private void MostrarFala(string fala)
	{
		Label texto = GetNode<Label>("Dialogo/Caixa/Texto");

		// Separa "Nome|fala". Se não tiver "|", não há personagem
		string personagem = "";
		if (fala.Contains('|'))
		{
			var partes = fala.Split('|', 2);
			personagem = partes[0];
			fala = partes[1];
		}

		// Escolhe a cor do personagem (ou a padrão)
		Color cor = coresPersonagem.TryGetValue(personagem, out var c) ? c : corPadrao;
		texto.AddThemeColorOverride("font_color", cor);

		texto.Text = fala;
		texto.VisibleRatio = 0;
		GetNode<CanvasLayer>("Dialogo").Show();

		tweenTexto?.Kill();
		tweenTexto = CreateTween();
		tweenTexto.TweenProperty(texto, "visible_ratio", 1.0, fala.Length * 0.05);
		tweenTexto.Finished += () => somDialogo?.Stop();
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