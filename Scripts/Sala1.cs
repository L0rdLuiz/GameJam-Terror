using Godot;
using System;
using System.Collections.Generic;

public partial class Sala1 : Node2D
{
    GameState gameState;
    AudioStreamPlayer2D musica;
    AudioStreamPlayer2D somDialogo;
    Action aoTerminarFala;
    Tween tweenTexto;
    bool livroLido = false;
    bool licaoLiberada = false;
    bool remedioTomado = false;
    bool historiaContada = false;
    Button botaoJogarLixo;
    Node2D[] comodos;
    private CanvasLayer painelTarefa;

    // Falas organizadas por dia e local. Linhas iniciadas com # são comandos internos.
    Dictionary<(int dia, string local), string[]> falas = new()
    {
        { (1, "Espelho"), new[] {
            "Cherry|Good morning, Cherry", "Cherry|All right... what do we have planned for today?",
            "Cherry|Let's see... lunch, getting Sugarplum ready for school, folding the laundry...",
            "Cherry|His first day of school. I wonder if he's excited.", "Cherry|He's growing up so fast.",
            "Cherry|Well, I should get started. No time to waste!"
        }},
        { (1, "Cozinha"), new[] { "Cherry|All right, let's get everything ready.", "Cherry|I should prepare lunch first." }},
        { (1, "Mesa"), new[] {
            "Cherry|So... first day of school. Nervous?", "Sugarplum|A little.", "Cherry|Yeah? What about?",
            "Sugarplum|I don't know. What if nobody wants to be my friend?", "Cherry|I'm sure you'll find someone. You just have to talk to them.",
            "Sugarplum|But what if I don't know what to say?", "Cherry|Then... ask them what they like. Or if they wanna play something. That's what I used to do.",
            "Sugarplum|You did?", "Cherry|Of course! You think I was born knowing how to talk to people?", "Sugarplum giggles.",
            "Sugarplum|Kinda.", "Cherry|Oh, you little rascal.", "Cherry smiles and shakes her head, pretending to be offended.",
            "The two keep eating for a few moments.", "Sugarplum|Mommy?", "Cherry|Hmm?", "Sugarplum|You'll be here when I get back, right?",
            "Cherry|Of course, sweetheart. Where else would I be?", "Sugarplum|Okay.", "Sugarplum smiles, satisfied, and goes back to eating.",
            "After lunch, Cherry clears the plates while Sugarplum climbs down from the chair and grabs his backpack near the entrance.",
            "Sugarplum|I'm ready!", "Cherry|Already? Let me see your backpack.", "Cherry quickly checks his supplies and adjusts the straps on his shoulders.",
            "Cherry|Yep, looks like you've got everything.", "Sugarplum|Can we go now?", "Cherry|Hold on, let me grab my things. No need to rush.",
            "Cherry picks up her things and walks over to him.", "Cherry|All right, let's go.", "Sugarplum|Yay!",
            "Cherry takes Sugarplum's hand and the two walk to the door."
        }},
        { (1, "SalaComFilho"), new[] {
            "Cherry bends down to adjust the backpack straps on her son's shoulders.", "Cherry|There. Is it too heavy?", "Sugarplum|Nope!",
            "Cherry|Good. Did you get everything?", "Sugarplum|I think so.", "Cherry|You think so?",
            "Cherry smiles and unzips one of the pockets to check quickly.", "Cherry|Looks fine to me.", "She closes the backpack and stands up.",
            "Sugarplum|Bye, Mommy!", "Cherry|Bye, sweetheart. Have a good day!", "Sugarplum|I will!",
            "Sugarplum starts to walk away. Cherry stays at the door, watching him for a few seconds.", "Cherry|And don't forget to eat your lunch!",
            "Sugarplum|I won't!", "#sala", "Cherry closes the door and stands still for a moment, her hand still on the doorknob.",
            "The sound of the street goes muffled as soon as the door closes. The house is quiet again, except for the natural ambient sounds."
        }},
        { (1, "fundosala"), new[] {
            "Cherry|Well, that's that.", "Cherry walks into the kitchen, which is connected to the living room, and picks up a glass left on the table.",
            "Cherry briefly glances toward the bedroom, where a few toys are still scattered around.", "Cherry|I swear, he was tiny just yesterday.",
            "Cherry|First day of school already...", "Cherry places the glass in the sink and looks at the clock.",
            "Cherry|I've got some time before dinner.", "Cherry glances at the book she had set aside and decides to pick it up."
        }},
        { (1, "Livro"), new[] {
            "Cherry|Okay, where was I?", "Cherry finds the page and continues reading.",
            "Cherry reaches a passage that catches her attention. She pauses briefly, keeping her finger between the pages.",
            "Cherry|Mom was right. This is actually pretty good.", "She smiles slightly and continues reading for a few more moments.",
            "Cherry glances at the clock and realizes she's spent more time reading than she expected.", "Cherry|I should tell her I'm enjoying it.",
            "She picks up the phone from the table, looks at the screen for a moment, and puts it back down.", "Cherry|It's been a while since she came over.",
            "Cherry glances toward Sugarplum's bedroom, where the toys are still scattered around.", "Cherry|Sugarplum would love to see her.",
            "She considers sending a message for a few seconds, but eventually turns her attention back to the book.", "Cherry|Maybe I'll call her later.",
            "Cherry goes back to reading.", "Cherry finally closes the book and stretches her arms, letting out a small sigh.",
            "Cherry|All right, that's enough reading for now.", "She leaves the book on the table and gets up.",
            "Cherry notices the clothes that need folding and walks over to them.", "Cherry|Oh, right. The laundry.",
            "Cherry gathers the clothes and finds some space in the living room to start folding them."
        }},
        { (1, "PosDobrarRoupa"), new[] { "Cherry|There. Much better.", "Cherry|I really need to get him to put his clothes away.", "Cherry|And these art supplies... I'll leave those for later.", "She looks around, satisfied with the tidiness.", "Cherry|All right. Dinner next." }},
        { (1, "Licao"), new[] { "Cherry sits beside Sugarplum at the table. He opens his notebook and looks for the homework page while Cherry watches over his shoulder.", "Sugarplum|Here it is!", "Cherry|Let's see... Oh, that's not too bad.", "Sugarplum|Can you help me?", "Cherry|Of course. Let's do it together." }},
        { (1, "PosLicao"), new[] { "Cherry closes the notebook and sets it aside. Sugarplum starts putting his pencils away.", "Sugarplum|Can I go play now?", "Cherry|Sure, just put your things away first.", "Sugarplum|Okay!", "Cherry|There. One more thing done." }},
        { (1, "PreDormir"), new[] { "Night has fallen. Sugarplum is already in his room, getting ready for bed.", "Cherry|All right, time for bed.", "Sugarplum|Already?", "Cherry|Already. Teeth brushed?", "Sugarplum|Yes!", "Cherry|Good. Get under the covers. I'll be right back, okay?", "Sugarplum|Okay." }},
        { (1, "Remedio"), new[] { "Cherry finishes tidying up a few things in the kitchen and looks at the clock.", "Cherry|What a day.", "Cherry takes one of her sleeping pills and swallows it with a glass of water.", "Cherry|All right. That's taken care of." }},
        { (1, "HistoriaUrsos"), new[] {
            "The room is lit by a soft light. Toys and drawings are still scattered around. Sugarplum is already lying down, waiting.",
            "Sugarplum|Mommy! You're back!", "Cherry|I told you I'd be right back.", "Sugarplum|Can you tell me a story?", "Cherry|The bear story?", "Sugarplum|The bear story!", "Cherry|All right, get comfy.",
            "Cherry|Once upon a time, there was a mother bear named Berry. She loved wandering through the forest, picking little berries. Blueberries, blackberries, raspberries... she loved all kinds of berries.",
            "Cherry|Berry always took her little cub with her to pick berries. But unlike Berry, her cub preferred other fruits, like apples, apricots, and his favorite of all, plums.",
            "Cherry|He loved plums so much that his mother gave him the nickname Little Plum. And whenever they went out to pick fruit, she always made sure to find at least one plum to bring back to her little cub.",
            "Cherry smiles and closes the book for a moment.", "Cherry|We'll read the rest tomorrow, okay?", "Sugarplum|Okay. Goodnight, Mommy.", "Cherry|Goodnight, my little Sugarplum. I love you.", "Sugarplum|Love you too.",
            "Cherry turns off the lamp, leaves the room and finally goes to sleep.", "End of day 1."
        }},
        { (1, "PrepararJantar"), new[] { "Cherry|All right, dinner time.", "Cherry|Let's see what we've got...", "Cherry|This should do." }},
        { (1, "PensamentosJantar"), new[] { "Cherry|I hope he's hungry.", "Cherry|There we go." }},
        { (1, "JantarPronto"), new[] { "Cherry|Dinner's ready!" }},
        { (1, "MesaJantar"), new[] { "Cherry|There you go. Careful, it's hot.", "Sugarplum|I know, I know!", "Sugarplum|Mmm! This is good!", "Cherry|Glad you like it.", "Sugarplum|Can I have more?", "Cherry|You haven't even finished that yet!", "Sugarplum|But I'm hungry!", "Cherry|All right, all right. Finish that first, and I'll get you some more." }},

        // DIA 2
        { (2, "Acordar"), new[] { "Cherry|Morning, Cherry.", "Cherry adjusts her clothes and studies her own reflection for a few more moments.", "Cherry|Let's see... lunch, taking out the trash, dinner...", "Cherry|Another day of school for Sugarplum.", "#quarto", "She looks toward the bed, where Sugarplum is still sleeping.", "Cherry|I hope he had a good time yesterday.", "Cherry gently approaches the bed and touches her son's shoulder.", "Cherry|Sugarplum, time to wake up.", "Sugarplum|Already?", "Cherry|Yep. Come on, sleepyhead.", "Sugarplum stretches and sits up in bed.", "Cherry heads to the kitchen to prepare lunch.", "The camera follows her out of the room." }},
        { (2, "Cozinha"), new[] { "Cherry gathers the ingredients on the counter and starts preparing lunch.", "Cherry|Let's see what I can make today.", "She picks up a knife and starts cutting the ingredients.", "Cherry|Sugarplum could use a little more variety." }},
        { (2, "AlmocoPronto"), new[] { "Cherry puts the plates on the table.", "Cherry|Lunch is ready!", "Cherry calls Sugarplum to eat.", "The camera cuts to the two of them sitting at the table." }},
        { (2, "MesaAlmoco"), new[] {
            "Cherry puts the plate in front of Sugarplum and sits across from him.", "Cherry|Careful, it's hot.", "Sugarplum|Okay!", "They start eating.", "Cherry|So, how was school today?", "Sugarplum keeps eating for a few moments before answering.", "Sugarplum|Fine.", "Cherry|Anything interesting happen?", "Sugarplum|Not really.", "Cherry notices he doesn't seem as excited as she expected.", "Cherry|Did you talk to that boy who sits next to you?", "Sugarplum|A little.", "Cherry|Well, that's a start.", "Cherry thinks for a moment and smiles, as if she just had an idea.", "Cherry|Hey, why don't we invite him over this weekend?", "Sugarplum|Really? Can we?", "Cherry|Of course! You could play together.", "Sugarplum|Do you think he'll come?", "Cherry|There's only one way to find out.", "Sugarplum smiles and goes back to eating, a little more cheerful.", "After lunch, Cherry clears the plates while Sugarplum gets ready for school.", "Sugarplum|Bye, Mommy!", "Cherry|Have a good day, sweetheart!", "Sugarplum leaves. Cherry closes the door and goes back to the living room."
        }},
        { (2, "Mensagem"), new[] { "Cherry picks up her phone and sits on the couch.", "Cherry|Let's see... I should probably message his mom.", "She opens the contact list and looks for the number of her son's classmate's mom.", "When she finds the contact, Cherry opens the conversation.", "The screen shows that the number is blocked. Cherry doesn't seem to notice.", "Cherry|Now, what should I say?", "Message on screen: Hi! This is Sugarplum's mom. He was talking about your son, and I was wondering if he'd like to come over this weekend to play?", "Cherry taps send.", "The message appears in the conversation as normal, but the contact stays marked as blocked. No reply comes.", "Cherry locks the phone and leaves it on the table, noticing nothing strange.", "Cherry|There. Hopefully she'll get back to me soon.", "Cherry looks at the trash bag near the kitchen.", "Cherry|Oh, right. I still need to take that out.", "She gets up and walks over to the bag." }},
        { (2, "LixoPensamento"), new[] { "Cherry|Huh. Must've been something in the bag.", "She continues the task without investigating." }},
        { (2, "PosLixo"), new[] { "Cherry finishes sorting the trash and goes back up to the loft.", "She looks at the clock and decides to use her free time to keep reading Hamnet.", "Cherry|Still got some time before dinner.", "Cherry picks up the book and sits on the couch." }},
        { (2, "Livro"), new[] { "Cherry opens Hamnet to the marked page and starts reading.", "The camera stays back, showing the quiet routine of the house.", "After a while, Cherry closes the book and looks at the clock.", "Cherry|I should start thinking about dinner.", "She leaves the book on the table and gets up.", "The camera follows Cherry to the kitchen." }},
        { (2, "PrepararJantar"), new[] { "Cherry gathers the ingredients and starts preparing dinner.", "Cherry|Let's get dinner ready." }},
        { (2, "JantarPronto"), new[] { "Cherry arranges the plates and calls Sugarplum.", "Cherry|Dinner's ready!" }},
        { (2, "MesaJantar"), new[] {
            "The camera cuts to the table, where Cherry and Sugarplum start eating.", "Cherry and Sugarplum eat in silence for a few moments.", "Cherry|So, how was your day?", "Sugarplum|Fine.", "Cherry|Did you get to play with anyone?", "Sugarplum lowers his eyes to his plate.", "Sugarplum|Not really.", "Cherry|Oh. How come?", "Sugarplum|They already have their own friends.", "Cherry stops eating for a moment.", "Cherry|Well, it can take a little while to get to know people.", "Sugarplum|I tried talking to them, but they just went somewhere else.", "Cherry tries not to show her concern.", "Cherry|Maybe they were busy. Give it some time, okay?", "Sugarplum doesn't answer right away.", "Cherry|Oh! I almost forgot. I sent a message to his mom about coming over this weekend.", "Sugarplum|Really?", "Cherry|Yep! We'll see what she says.", "Sugarplum seems a little more cheerful.", "After dinner, Cherry clears the plates and tidies up the kitchen."
        }},
        { (2, "PreRemedio"), new[] { "Cherry goes to the bathroom and stops in front of the mirror.", "She looks tired, but doesn't show any concern.", "Cherry|Time to get ready for bed." }},
        { (2, "Remedio"), new[] { "She takes her medication, follows her routine and puts the packaging away.", "Cherry|There. All done.", "Cherry turns off the light and goes back to the shared bedroom." }},
        { (2, "HistoriaUrsos"), new[] {
            "Sugarplum is lying in bed, waiting for her.", "Sugarplum|Can we read the bear story?", "Cherry|Of course. Where did we stop?", "Sugarplum|Berry was picking berries!", "Cherry|That's right. Let's see what happens next.", "Cherry opens the book and starts reading.",
            "Cherry|Berry didn't spend all her days wandering through the forest picking berries. She also helped take care of the sick animals. Everyone knew her as the forest healer.",
            "Cherry|She knew how to use all kinds of plants to help others. If someone had a cold, she knew how to prepare a nourishing meal. If someone had trouble sleeping, she knew how to make a tea that would help them rest. And if someone felt anxious, she knew how to make incense to calm their mind.",
            "Sugarplum listens carefully.", "Cherry|Her little cub admired her so much. He often told her that, when he grew up, he wanted to become a healer too.",
            "Cherry|And Berry always told him that if he kept eating his fruit, he'd grow even bigger than her and become a wonderful healer.", "Sugarplum|I'd eat all the plums!", "Cherry chuckles softly.", "Cherry|I bet you would.", "Cherry closes the book and puts it beside the bed.", "Cherry|Goodnight, my little Sugarplum.", "Sugarplum|Goodnight, Mommy.", "The camera slowly pulls back, showing the two of them in the same bed.", "The screen fades to black.", "Day 2 — Complete"
        }}
    };

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
        if (musica.Stream == nova && musica.Playing) return;
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
        var areaExterna = GetNode<Node2D>("AreaExterna");

        comodos = new[] { menu, quarto, banheiro, espelho, sala, cozinha, mesa, cozinhar, salafilho, livro, areaExterna };
        painelTarefa = GetNode<CanvasLayer>("PainelTarefa");
        MostrarComodo(menu);

        GetNode<Button>("%IrCozinha").Pressed += () => { MostrarComodo(cozinha); TentarFalar("Cozinha"); };
        GetNode<Button>("%DobrarRoupa").Pressed += IniciarMinigameRoupa;
        GetNode<Button>("%LevarLixo").Pressed += IniciarMinigameLixo;
        GetNode<Button>("%TarefaEscolar").Pressed += IniciarMinigameLicao;

        GetNode<Button>("%BotaoComecar").Pressed += () => { MostrarComodo(espelho); TentarFalar("Espelho"); GetNode<Button>("%SairEspelho").Show(); };
        GetNode<Button>("%IrBanheiro").Pressed += () => MostrarComodo(banheiro);
        GetNode<Button>("%VoltarBanheiro").Pressed += IrParaOQuarto;
        GetNode<Button>("%IrAreaExterna").Pressed += () => MostrarComodo(areaExterna);

        botaoJogarLixo = new Button { Text = "Jogar lixo", Position = new Vector2(500, 300), Size = new Vector2(200, 60) };
        botaoJogarLixo.Pressed += JogarLixo;
        areaExterna.AddChild(botaoJogarLixo);
        var voltarExterna = new Button { Text = "Voltar", Position = new Vector2(50, 50) };
        voltarExterna.Pressed += () => MostrarComodo(banheiro);
        areaExterna.AddChild(voltarExterna);

        GetNode<Button>("%Espelho").Pressed += () =>
        {
            MostrarComodo(espelho);
            GetNode<Button>("%SairEspelho").Show();
            if (gameState.Day == 1 && gameState.PossoFazerATarefa(GameState.TomarRemedio) && !remedioTomado)
            {
                TentarFalar("Remedio", () => { remedioTomado = true; gameState.ConcluirTarefa(GameState.TomarRemedio); });
            }
            else if (gameState.Day == 2 && gameState.PossoFazerATarefa(GameState.TomarRemedio) && !remedioTomado)
            {
                SeguirParaORemedio();
            }
        };
        GetNode<Button>("%SairEspelho").Pressed += () => MostrarComodo(banheiro);

        GetNode<Button>("%IrSala").Pressed += () =>
        {
            MostrarComodo(sala);
            if (gameState.PossoFazerATarefa(GameState.TarefaEscolar) && gameState.TarefaEstaFeita(GameState.Janta) && !licaoLiberada)
                TentarFalar("Licao", () => { licaoLiberada = true; AtualizarBotoes(); });
        };
        GetNode<Button>("%VoltarDaSala").Pressed += IrParaOQuarto;

        GetNode<Button>("%Livro").Pressed += () =>
        {
            MostrarComodo(livro);
            TentarFalar("Livro", () =>
            {
                livroLido = true;
                gameState.ConcluirTarefa(GameState.LerLivro);
                AtualizarBotoes();
                if (gameState.Day == 2) { MostrarComodo(cozinha); TentarFalar("PrepararJantar"); }
            });
        };
        GetNode<Button>("%SairLivro").Pressed += () => MostrarComodo(sala);
        GetNode<Button>("%FogaoCozinhar").Pressed += IniciarMinigameCozinha;
        GetNode<Button>("%VoltarDaCozinha").Pressed += () => MostrarComodo(sala);

        GetNode<Button>("%IrSalaDaCozinha").Pressed += () =>
        {
            if (gameState.Day != 1 || gameState.TarefaEstaFeita(GameState.Janta)) MostrarComodo(sala);
            else { MostrarComodo(salafilho); TentarFalar("SalaComFilho", () => TentarFalar("fundosala")); }
        };
        GetNode<Button>("%sair").Pressed += () => { MostrarComodo(mesa); TentarFalar("Mesa"); };

        GetNode<Button>("Dialogo/Caixa/Avancar").Pressed += () =>
        {
            if (falasAtuais == null) return;
            if (tweenTexto != null && tweenTexto.IsRunning())
            {
                tweenTexto.Kill(); somDialogo?.Stop();
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

        Button nextDay = GetNode<Button>("%ProximoDia");
        Label textoNextDay = GetNode<Label>("%TextoNextDay");
        nextDay.Visible = gameState.DiaCompleto();
        textoNextDay.Hide();
        nextDay.MouseEntered += () => textoNextDay.Show();
        nextDay.MouseExited += () => textoNextDay.Hide();
        nextDay.Pressed += OnButtonPressed;
    }

    private void JogarLixo()
    {
        if (!gameState.PossoFazerATarefa(GameState.LevarLixo)) return;
        gameState.ConcluirTarefa(GameState.LevarLixo);
        TentarFalar("LixoPensamento", () => { MostrarComodo(GetNode<Node2D>("SalaDeEstar")); TentarFalar("PosLixo"); });
    }

    private void AposJantar()
    {
        if (gameState.Day == 2) { SeguirParaORemedio(); return; }
        LiberarLicao();
    }

    // Dia 2: pintura ignorada; o remédio acontece automaticamente na narrativa.
    private void SeguirParaORemedio()
    {
        MostrarComodo(GetNode<Node2D>("ComodoEspelho"));
        TentarFalar("PreRemedio", () => TentarFalar("Remedio", AoTomarRemedio));
    }

    private void LiberarLicao()
    {
        MostrarComodo(GetNode<Node2D>("SalaDeEstar"));
        TentarFalar("Licao", () => { licaoLiberada = true; AtualizarBotoes(); });
    }

    private void AoTomarRemedio()
    {
        if (remedioTomado) return;
        remedioTomado = true;
        gameState.ConcluirTarefa(GameState.TomarRemedio);
        if (gameState.Day == 2) ContarHistoria();
    }

    private void ContarHistoria()
    {
        if (historiaContada) return;
        GD.Print("Objetivo: contar a história.");
        MostrarComodo(GetNode<Node2D>("ComodoQuarto"));
        TentarFalar("HistoriaUrsos", AoFimDaHistoria);
    }

    private void IrParaOQuarto()
    {
        MostrarComodo(GetNode<Node2D>("ComodoQuarto"));
        if (gameState.Day == 2 && remedioTomado && !historiaContada && gameState.PossoFazerATarefa(GameState.HistoriaFilho))
            ContarHistoria();
    }

    private void AoFimDaHistoria()
    {
        if (historiaContada) return;
        historiaContada = true;
        gameState.ConcluirTarefa(GameState.HistoriaFilho);
        GD.Print("História contada.");
        GD.Print("Cherry e Sugarplum vão dormir.");
        FecharDialogo();
        if (gameState.Day == 2)
        {
            GD.Print("Dia 2 concluído. Avançando para o Dia 3.");
            gameState.TentarVirarODia();
        }
    }

    private void MostrarComodo(Node2D comodo, bool fecharDialogo = true)
    {
        foreach (var c in comodos) c.Hide();
        comodo.Show();
        painelTarefa.Visible = comodo != comodos[0];
        if (fecharDialogo) GetNode<CanvasLayer>("Dialogo").Hide();
        AtualizarBotoes();
    }

    private void AtualizarBotoes()
    {
        GetNode<Button>("%ProximoDia").Visible = gameState.DiaCompleto();
        GetNode<Button>("%TarefaEscolar").Visible = licaoLiberada && gameState.PossoFazerATarefa(GameState.TarefaEscolar);
        GetNode<Button>("%FogaoCozinhar").Visible = gameState.PossoFazerATarefa(GameState.Almoco) || gameState.PossoFazerATarefa(GameState.Janta);
        GetNode<Button>("%DobrarRoupa").Visible = livroLido && gameState.PossoFazerATarefa(GameState.DobrarRoupa);
        GetNode<Button>("%LevarLixo").Visible = false;
        GetNode<Button>("%IrAreaExterna").Visible = gameState.PossoFazerATarefa(GameState.LevarLixo);
        GetNode<Button>("%Livro").Visible = gameState.PossoFazerATarefa(GameState.LerLivro);
        if (botaoJogarLixo != null) botaoJogarLixo.Visible = gameState.PossoFazerATarefa(GameState.LevarLixo);
    }

    private void IniciarMinigameCozinha()
    {
        string tarefaAtual;
        if (gameState.PossoFazerATarefa(GameState.Almoco)) tarefaAtual = GameState.Almoco;
        else if (gameState.PossoFazerATarefa(GameState.Janta)) tarefaAtual = GameState.Janta;
        else return;

        PackedScene cena = GD.Load<PackedScene>("res://Scenes/minigame_precisao.tscn");
        if (cena == null) { GD.PrintErr("Não foi possível carregar o minigame."); return; }
        Node minigame = cena.Instantiate();
        minigame.TreeExited += () =>
        {
            if (!IsInsideTree()) return;
            gameState.ConcluirTarefa(tarefaAtual);
            Show();
            if (tarefaAtual == GameState.Janta)
            {
                MostrarComodo(GetNode<Node2D>("ComodoCozinha"));
                TentarFalar("JantarPronto", () => { MostrarComodo(GetNode<Node2D>("ComodoCozinhaMesa")); TentarFalar("MesaJantar", AposJantar); });
            }
            else if (gameState.Day == 2)
            {
                MostrarComodo(GetNode<Node2D>("ComodoCozinha"));
                TentarFalar("AlmocoPronto", () => { MostrarComodo(GetNode<Node2D>("ComodoCozinhaMesa")); TentarFalar("MesaAlmoco", () => { MostrarComodo(GetNode<Node2D>("SalaDeEstar")); TentarFalar("Mensagem"); }); });
            }
            else { MostrarComodo(GetNode<Node2D>("ComodoCozinhaMesa")); TentarFalar("Mesa"); }
        };
        GetTree().Root.AddChild(minigame);
        Hide();
    }

    private void IniciarMinigameRoupa()
    {
        if (!gameState.PossoFazerATarefa(GameState.DobrarRoupa)) return;
        PackedScene cena = GD.Load<PackedScene>("res://Scenes/MinigameRoupa/minigame_roupa.tscn");
        if (cena == null) { GD.PrintErr("Não foi possível carregar o minigame de roupa."); return; }
        Node minigame = cena.Instantiate();
        minigame.TreeExited += () =>
        {
            if (!IsInsideTree()) return;
            gameState.ConcluirTarefa(GameState.DobrarRoupa);
            Show(); MostrarComodo(GetNode<Node2D>("SalaDeEstar")); AtualizarBotoes(); TentarFalar("PosDobrarRoupa");
        };
        GetTree().Root.AddChild(minigame); Hide();
    }

    private void IniciarMinigameLixo()
    {
        if (!gameState.PossoFazerATarefa(GameState.LevarLixo)) return;
        PackedScene cena = GD.Load<PackedScene>("res://Scenes/MinigameLixo/minigame_lixo.tscn");
        if (cena == null) { GD.PrintErr("Não foi possível carregar o minigame do lixo."); return; }
        Node minigame = cena.Instantiate();
        minigame.TreeExited += () => { if (!IsInsideTree()) return; gameState.ConcluirTarefa(GameState.LevarLixo); Show(); AtualizarBotoes(); };
        GetTree().Root.AddChild(minigame); Hide();
    }

    private void IniciarMinigameLicao()
    {
        if (!gameState.PossoFazerATarefa(GameState.TarefaEscolar)) return;
        PackedScene cena = GD.Load<PackedScene>("res://Scenes/MinigameLicao/minigame_licao.tscn");
        if (cena == null) { GD.PrintErr("Não foi possível carregar o minigame de lição."); return; }
        Node minigame = cena.Instantiate();
        minigame.TreeExited += () =>
        {
            if (!IsInsideTree()) return;
            gameState.ConcluirTarefa(GameState.TarefaEscolar); Show(); AtualizarBotoes(); TentarFalar("PosLicao");
        };
        GetTree().Root.AddChild(minigame); Hide();
    }

    private void TentarFalar(string local, Action aoTerminar = null)
    {
        var chave = (gameState.Day, local);
        if (!falas.TryGetValue(chave, out var linhas) || falasVistas.Contains(chave)) { aoTerminar?.Invoke(); return; }
        falasVistas.Add(chave); falasAtuais = linhas; falaAtual = 0; aoTerminarFala = aoTerminar; MostrarProximaLinha();
    }

    private void MostrarProximaLinha()
    {
        while (falasAtuais != null && falaAtual < falasAtuais.Length && falasAtuais[falaAtual].StartsWith("#"))
        { ExecutarComando(falasAtuais[falaAtual]); falaAtual++; }
        if (falasAtuais != null && falaAtual < falasAtuais.Length) MostrarFala(falasAtuais[falaAtual]);
        else FimDoDialogo();
    }

    private void ExecutarComando(string comando)
    {
        switch (comando)
        {
            case "#sala": MostrarComodo(GetNode<Node2D>("SalaDeEstar"), false); break;
            case "#quarto": MostrarComodo(GetNode<Node2D>("ComodoQuarto"), false); break;
        }
    }

    private void FimDoDialogo()
    {
        GetNode<CanvasLayer>("Dialogo").Hide();
        var acao = aoTerminarFala; aoTerminarFala = null; acao?.Invoke();
    }

    private void FecharDialogo()
    {
        tweenTexto?.Kill(); somDialogo?.Stop(); GetNode<CanvasLayer>("Dialogo").Hide();
    }

    private void MostrarFala(string fala)
    {
        Label texto = GetNode<Label>("Dialogo/Caixa/Texto");
        string personagem = "";
        if (fala.Contains('|')) { var partes = fala.Split('|', 2); personagem = partes[0]; fala = partes[1]; }
        Color cor = coresPersonagem.TryGetValue(personagem, out var c) ? c : corPadrao;
        texto.AddThemeColorOverride("font_color", cor);
        texto.Text = fala; texto.VisibleRatio = 0; GetNode<CanvasLayer>("Dialogo").Show();
        tweenTexto?.Kill(); tweenTexto = CreateTween(); tweenTexto.TweenProperty(texto, "visible_ratio", 1.0, fala.Length * 0.05);
        tweenTexto.Finished += () => somDialogo?.Stop(); somDialogo?.Play();
    }

    public override void _ExitTree()
    {
        if (gameState != null) gameState.DiaMudou -= AoDiaMudar;
        if (gameState != null) gameState.TarefaConcluida -= AoTarefaConcluir;
    }

    private void OnButtonPressed() { gameState.TentarVirarODia(); }

    private void AoDiaMudar(int dia)
    {
        GD.Print("Dia mudou para: " + dia);
        livroLido = false; licaoLiberada = false; remedioTomado = false; historiaContada = false;
        MostrarComodo(GetNode<Node2D>("ComodoQuarto"));
        if (falas.ContainsKey((dia, "Acordar")))
        {
            GetTree().CreateTimer(2.5).Timeout += () =>
            {
                if (!IsInsideTree()) return;
                MostrarComodo(GetNode<Node2D>("ComodoEspelho")); GetNode<Button>("%SairEspelho").Show(); TentarFalar("Acordar");
            };
        }
    }

    private void AoTarefaConcluir(string nomeTarefa) { AtualizarBotoes(); }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event is InputEventMouseButton { Pressed: true }) GD.Print("MOUSE CLICADO!");
        if (OS.IsDebugBuild() && @event is InputEventKey { Pressed: true, Echo: false } tecla)
        {
            if (tecla.Keycode == Key.F1) PularDiaDebug();
            else if (tecla.Keycode == Key.F2) gameState.DebugConcluirTarefasDoDia();
        }
    }

    private void PularDiaDebug()
    {
        tweenTexto?.Kill(); somDialogo?.Stop(); falasAtuais = null; aoTerminarFala = null;
        gameState.DebugPularDia();
    }
}
