using Godot;
using System;

public partial class Sala1 : Node2D
{
	GameState gameState;











	
		public override void _Ready()
	{
		// estado inicial: começa na sala 1 
		GetNode<Sprite2D>("Sprite2D").Show();
		GetNode<Sprite2D>("Fundo2").Hide();
		GetNode<Node2D>("SalaDeEstar").Hide();

		GetNode<Button>("BotaoVoltar").Hide();
		GetNode<Button>("TarefaEscolar").Hide();

		//	QUANDO, ENT FAÇA
		// Botao: sala 1 -> sala 2
		GetNode<Button>("Botao").Pressed += () =>
		{
			GetNode<Sprite2D>("Sprite2D").Hide();
			GetNode<Node2D>("SalaDeEstar").Hide();
			GetNode<Sprite2D>("Fundo2").Show();

			GetNode<Button>("Salaestar").Hide();   
			GetNode<Button>("BotaoVoltar").Show();
			GetNode<Button>("TarefaEscolar").Hide();
		};

		// BotaoVoltar: sala 2 -> sala 1
		GetNode<Button>("BotaoVoltar").Pressed += () =>
		{
			GetNode<Sprite2D>("Fundo2").Hide();
			GetNode<Node2D>("SalaDeEstar").Hide();
			GetNode<Sprite2D>("Sprite2D").Show();

			GetNode<Button>("TarefaEscolar").Hide();
			GetNode<Button>("BotaoVoltar").Hide();
			GetNode<Button>("Salaestar").Show();
		};

		// Salaestar: sala 1 -> sala de estar
		GetNode<Button>("Salaestar").Pressed += () =>
		{
			GetNode<Sprite2D>("Sprite2D").Hide();
			GetNode<Sprite2D>("Fundo2").Hide();
			GetNode<Node2D>("SalaDeEstar").Show();

			GetNode<Button>("BotaoVoltar").Hide();
			GetNode<Button>("TarefaEscolar").Show();
		};




		Button nextDay = GetNode<Button>("NextDay");
		Label textoNextDay = GetNode<Label>("NextDay/TextoNextDay");

		textoNextDay.Hide();

		nextDay.MouseEntered += () => textoNextDay.Show();
		nextDay.MouseExited += () => textoNextDay.Hide();

		GetNode<Button>("NextDay").Pressed += () =>
		{
			OnButtonPressed();;
		};

		GetNode<Button>("TarefaEscolar").Pressed += () =>
		{
			if (!GetNode<GameState>("/root/GameState").PossoFazerATarefa(GameState.TarefaEscolar))
			{
				GD.Print("Não pode fazer a tarefa escolar ainda!");
				return;
			}
			PackedScene tarefaEscolarScene = GD.Load<PackedScene>("res://Scenes/MinigameLicao/minigame_licao.tscn");
			Node tarefaEscolarInstance = tarefaEscolarScene.Instantiate();
			GetTree().Root.AddChild(tarefaEscolarInstance);
			Hide();
			tarefaEscolarInstance.TreeExited += () => Show();
		};

		gameState = GetNode<GameState>("/root/GameState");
		gameState.DiaMudou += AoDiaMudar;
	}

	public override void _ExitTree()
	{
		gameState.DiaMudou -= AoDiaMudar;
	}

	private void OnButtonPressed()
	{
		GetNode<GameState>("/root/GameState").NextDay();
	}

	private void AoDiaMudar(int dia)
	{
		GD.Print("Dia mudou para: " + dia);
	}
	
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _UnhandledInput(InputEvent @event)
	{
		if(@event is InputEventMouseButton {Pressed: true})
		GD.Print("MOUSE CLICADO!");
	}
}
