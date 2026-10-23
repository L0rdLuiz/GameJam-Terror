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
	bool livroLido = false;
	bool licaoLiberada = false;
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
		{ (1,"Livro"), new[]
		{
			"Cherry|Okay, where was I?",
			"Cherry finds the page and continues reading.",
			"Cherry reaches a passage that catches her attention. She pauses briefly, keeping her finger between the pages.",
			"Cherry|Mom was right. This is actually pretty good.",
			"She smiles slightly and continues reading for a few more moments.",
			"Cherry glances at the clock and realizes she's spent more time reading than she expected.",
			"Cherry|I should tell her I'm enjoying it.",
			"She picks up the phone from the table, looks at the screen for a moment, and puts it back down.",
			"Cherry|It's been a while since she came over.",
			"Cherry glances toward Sugarplum's bedroom, where the toys are still scattered around.",
			"Cherry|Sugarplum would love to see her.",
			"She considers sending a message for a few seconds, but eventually turns her attention back to the book.",
			"Cherry|Maybe I'll call her later.",
			"Cherry goes back to reading.",
			"Cherry finally closes the book and stretches her arms, letting out a small sigh.",
			"Cherry|All right, that's enough reading for now.",
			"She leaves the book on the table and gets up.",
			"Cherry notices the clothes that need folding and walks over to them.",
			"Cherry|Oh, right. The laundry.",
			"Cherry gathers the clothes and finds some space in the living room to start folding them."
		}
		},

		{ (1, "PosDobrarRoupa"), new[]
			{
				"Cherry|There. Much better.",
				"Cherry|I really need to get him to put his clothes away.",
				"Cherry|And these art supplies... I'll leave those for later.",
				"She looks around, satisfied with the tidiness.",
				"Cherry|All right. Dinner next."
			}
		},

		{ (1, "Licao"), new[]
			{
				"Cherry sits beside Sugarplum at the table. He opens his notebook and looks for the homework page while Cherry watches over his shoulder.",
				"Sugarplum|Here it is!",
				"Cherry|Let's see... Oh, that's not too bad.",
				"Sugarplum|Can you help me?",
				"Cherry|Of course. Let's do it together."
			}
		},

		{ (1, "PosLicao"), new[]
			{
				"Cherry closes the notebook and sets it aside. Sugarplum starts putting his pencils away.",
				"Sugarplum|Can I go play now?",
				"Cherry|Sure, just put your things away first.",
				"Sugarplum|Okay!",
				"Cherry|There. One more thing done."
			}
		},

		{ (1, "PreDormir"), new[]
			{
				"Night has fallen. Sugarplum is already in his room, getting ready for bed.",
				"Cherry|All right, time for bed.",
				"Sugarplum|Already?",
				"Cherry|Already. Teeth brushed?",
				"Sugarplum|Yes!",
				"Cherry|Good. Get under the covers. I'll be right back, okay?",
				"Sugarplum|Okay."
			}
		},

		{ (1, "Remedio"), new[]
			{
				"Cherry finishes tidying up a few things in the kitchen and looks at the clock.",
				"Cherry|What a day.",
				"Cherry takes one of her sleeping pills and swallows it with a glass of water.",
				"Cherry|All right. That's taken care of."
			}
		},

		{ (1, "HistoriaUrsos"), new[]
			{
				"The room is lit by a soft light. Toys and drawings are still scattered around. Sugarplum is already lying down, waiting.",
				"Sugarplum|Mommy! You're back!",
				"Cherry|I told you I'd be right back.",
				"Sugarplum|Can you tell me a story?",
				"Cherry|The bear story?",
				"Sugarplum|The bear story!",
				"Cherry|All right, get comfy.",
				"Cherry|Once upon a time, there was a mother bear named Berry. She loved wandering through the forest, picking little berries. Blueberries, blackberries, raspberries... she loved all kinds of berries.",
				"Cherry|Berry always took her little cub with her to pick berries. But unlike Berry, her cub preferred other fruits, like apples, apricots, and his favorite of all, plums.",
				"Cherry|He loved plums so much that his mother gave him the nickname Little Plum. And whenever they went out to pick fruit, she always made sure to find at least one plum to bring back to her little cub.",
				"Cherry smiles and closes the book for a moment.",
				"Cherry|We'll read the rest tomorrow, okay?",
				"Sugarplum|Okay. Goodnight, Mommy.",
				"Cherry|Goodnight, my little Sugarplum. I love you.",
				"Sugarplum|Love you too.",
				"End of day 1."
			}
		},

		{ (1, "PrepararJantar"), new[]
			{
				"Cherry|All right, dinner time.",
				"Cherry|Let's see what we've got...",
				"Cherry|This should do."
			}
		},

		{ (1, "PensamentosJantar"), new[]
			{
				"Cherry|I hope he's hungry.",
				"Cherry|There we go."
			}
		},

		{ (1, "JantarPronto"), new[]
			{
				"Cherry|Dinner's ready!"
			}
		},

		{ (1, "MesaJantar"), new[]
			{
				"Cherry|There you go. Careful, it's hot.",
				"Sugarplum|I know, I know!",
				"Sugarplum|Mmm! This is good!",
				"Cherry|Glad you like it.",
				"Sugarplum|Can I have more?",
				"Cherry|You haven't even finished that yet!",
				"Sugarplum|But I'm hungry!",
				"Cherry|All right, all right. Finish that first, and I'll get you some more."
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

		GetNode<Button>("%IrCozinha").Pressed += () =>
		{
			MostrarComodo(cozinha);
			TentarFalar("Cozinha");
		};

		Button dobrarRoupa = GetNode<Button>("%DobrarRoupa");
		dobrarRoupa.Pressed += IniciarMinigameRoupa;

		Button levarLixo = GetNode<Button>("%LevarLixo");
		levarLixo.Pressed += IniciarMinigameLixo;

		GetNode<Button>("%BotaoComecar").Pressed += () =>
		{
			MostrarComodo(espelho);
			TentarFalar("Espelho");
			GetNode<Button>("%SairEspelho").Show();
		};
		GetNode<Button>("%IrBanheiro").Pressed += () => MostrarComodo(banheiro);
		GetNode<Button>("%VoltarBanheiro").Pressed += () => MostrarComodo(quarto);

		// Espelho: fala só na primeira vez do dia
		GetNode<Button>("%Espelho").Pressed += () =>
		{
			MostrarComodo(espelho);
			GetNode<Button>("%SairEspelho").Show();
		};
		GetNode<Button>("%SairEspelho").Pressed += () => MostrarComodo(banheiro);

		// Lição: só depois do jantar
		GetNode<Button>("%IrSala").Pressed += () =>
		{
			MostrarComodo(sala);

			if (gameState.PossoFazerATarefa(GameState.TarefaEscolar)
				&& gameState.TarefaEstaFeita(GameState.Janta)
				&& !licaoLiberada)
			{
				TentarFalar("Licao", () =>
				{
					licaoLiberada = true;
					AtualizarBotoes();
				});
			}
		};
		GetNode<Button>("%VoltarDaSala").Pressed += () => MostrarComodo(quarto);

		// Livro: sala -> livro -> sala
		GetNode<Button>("%Livro").Pressed += () =>
		{
			MostrarComodo(livro);
			TentarFalar("Livro", () =>
			{
				livroLido = true;
				AtualizarBotoes();
			});
		};
		GetNode<Button>("%SairLivro").Pressed += () => MostrarComodo(sala);

		GetNode<Button>("%FogaoCozinhar").Pressed += IniciarMinigameCozinha;

		GetNode<Button>("%VoltarDaCozinha").Pressed += () => MostrarComodo(sala);

		GetNode<Button>("%IrSalaDaCozinha").Pressed += () =>
		{
			if (gameState.TarefaEstaFeita(GameState.Janta))
			{
				MostrarComodo(sala);
			}
			else
			{
				MostrarComodo(salafilho);
				TentarFalar("SalaComFilho", () => TentarFalar("fundosala"));
			}
		};

		GetNode<Button>("%sair").Pressed += () =>
		{
			MostrarComodo(mesa);
			TentarFalar("Mesa");
		};

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
		textoTarefa.MouseFilter = Control.MouseFilterEnum.Ignore;
		textoTarefa.Hide();
		tarefaEscolar.MouseEntered += () => textoTarefa.Show();
		tarefaEscolar.MouseExited += () => textoTarefa.Hide();
		tarefaEscolar.VisibilityChanged += () => textoTarefa.Hide();

		// Próximo dia
		Button nextDay = GetNode<Button>("%ProximoDia");
		Label textoNextDay = GetNode<Label>("%TextoNextDay");

		nextDay.Visible = gameState.DiaCompleto();

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
			if (tarefaEscolarScene == null)
			{
				GD.PrintErr("Não foi possível carregar o minigame da lição.");
				return;
			}
			Node tarefaEscolarInstance = tarefaEscolarScene.Instantiate();
			GetTree().Root.AddChild(tarefaEscolarInstance);
			Hide();

			tarefaEscolarInstance.TreeExited += () =>
			{
				if (!IsInsideTree()) return;

				if (gameState.PossoFazerATarefa(GameState.TarefaEscolar))
					gameState.ConcluirTarefa(GameState.TarefaEscolar);

				Show();
				MostrarComodo(GetNode<Node2D>("SalaDeEstar"));
				TentarFalar("PosLicao", SequenciaDormir);
			};
		};
	}

	// Depois do jantar: vai pra sala e libera o botão da lição
	private void LiberarLicao()
	{
		MostrarComodo(GetNode<Node2D>("SalaDeEstar"));
		TentarFalar("Licao", () =>
		{
			licaoLiberada = true;
			AtualizarBotoes();
		});
	}

	// Pós-lição: quarto -> cozinha (remédio) -> quarto (história dos ursos)
	private void SequenciaDormir()
	{
		var quarto = GetNode<Node2D>("ComodoQuarto");
		var cozinha = GetNode<Node2D>("ComodoCozinha");

		MostrarComodo(quarto);
		TentarFalar("PreDormir", () =>
		{
			MostrarComodo(cozinha);
			TentarFalar("Remedio", () =>
			{
				MostrarComodo(quarto);
				TentarFalar("HistoriaUrsos", AtualizarBotoes);
			});
		});
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
		GetNode<Button>("%ProximoDia").Visible =
			gameState.DiaCompleto();

		GetNode<Button>("%TarefaEscolar").Visible =
			licaoLiberada &&
			gameState.PossoFazerATarefa(GameState.TarefaEscolar);

		GetNode<Button>("%Cavalete").Visible =
			gameState.PossoFazerATarefa(GameState.Pintar);

		GetNode<Button>("%FogaoCozinhar").Visible =
			gameState.PossoFazerATarefa(GameState.Almoco)
			|| gameState.PossoFazerATarefa(GameState.Janta);

		GetNode<Button>("%DobrarRoupa").Visible =
			livroLido && gameState.PossoFazerATarefa(GameState.DobrarRoupa);
		GetNode<Button>("%LevarLixo").Visible =
			gameState.PossoFazerATarefa(GameState.LevarLixo);
	}

	private void IniciarMinigameCozinha()
	{
		string tarefaAtual;

		if (gameState.PossoFazerATarefa(GameState.Almoco))
			tarefaAtual = GameState.Almoco;
		else if (gameState.PossoFazerATarefa(GameState.Janta))
			tarefaAtual = GameState.Janta;
		else
			return;

		PackedScene cena = GD.Load<PackedScene>(
			"res://Scenes/minigame_precisao.tscn"
		);

		if (cena == null)
		{
			GD.PrintErr("Não foi possível carregar o minigame.");
			return;
		}

		Node minigame = cena.Instantiate();

		minigame.TreeExited += () =>
		{
			if (!IsInsideTree()) return;

			gameState.ConcluirTarefa(tarefaAtual);
			Show();

			if (tarefaAtual == GameState.Janta)
			{
				MostrarComodo(GetNode<Node2D>("ComodoCozinha"));

				TentarFalar("JantarPronto", () =>
				{
					MostrarComodo(GetNode<Node2D>("ComodoCozinhaMesa"));
					TentarFalar("MesaJantar", LiberarLicao);
				});
			}
			else
			{
				MostrarComodo(GetNode<Node2D>("ComodoCozinhaMesa"));
				TentarFalar("Mesa");
			}
		};

		GetTree().Root.AddChild(minigame);
		Hide();
	}

	private void IniciarMinigameRoupa()
	{
		if (!gameState.PossoFazerATarefa(GameState.DobrarRoupa))
			return;

		PackedScene cena = GD.Load<PackedScene>(
			"res://Scenes/MinigameRoupa/minigame_roupa.tscn"
		);

		if (cena == null)
		{
			GD.PrintErr("Não foi possível carregar o minigame de roupa.");
			return;
		}

		Node minigame = cena.Instantiate();

		minigame.TreeExited += () =>
		{
			if (!IsInsideTree()) return;

			gameState.ConcluirTarefa(GameState.DobrarRoupa);

			Show();

			MostrarComodo(GetNode<Node2D>("SalaDeEstar"));

			AtualizarBotoes();

			TentarFalar("PosDobrarRoupa");
		};

		GetTree().Root.AddChild(minigame);
		Hide();
	}

	private void IniciarMinigameLixo()
	{
		if (!gameState.PossoFazerATarefa(GameState.LevarLixo))
			return;

		PackedScene cena = GD.Load<PackedScene>(
			"res://Scenes/MinigameLixo/minigame_lixo.tscn"
		);

		if (cena == null)
		{
			GD.PrintErr("Não foi possível carregar o minigame do lixo.");
			return;
		}

		Node minigame = cena.Instantiate();

		minigame.TreeExited += () =>
		{
			gameState.ConcluirTarefa(GameState.LevarLixo);
			Show();
			AtualizarBotoes();
		};

		GetTree().Root.AddChild(minigame);
		Hide();
	}

	// Fala só se existir fala para este dia/local e ainda não foi vista
	private void TentarFalar(string local, Action aoTerminar = null)
	{
		var chave = (gameState.Day, local);

		if (!falas.TryGetValue(chave, out var linhas) || falasVistas.Contains(chave))
		{
			aoTerminar?.Invoke();
			return;
		}

		falasVistas.Add(chave);
		falasAtuais = linhas;
		falaAtual = 0;
		aoTerminarFala = aoTerminar;
		MostrarProximaLinha();
	}

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
		gameState.TentarVirarODia();
	}

	private void AoDiaMudar(int dia)
	{
		GD.Print("Dia mudou para: " + dia);
		livroLido = false;
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