using Godot;
using System;

public partial class Sala1 : Node2D
{
	GameState gameState;

	public override void _Ready()
	{
		gameState = GetNode<GameState>("/root/GameState");
		gameState.DiaMudou += AoDiaMudar;

		// estado inicial: começa na sala 1
		GetNode<Sprite2D>("Sprite2D").Show();
		GetNode<Sprite2D>("Fundo2").Hide();
		GetNode<Node2D>("SalaDeEstar").Hide();

		GetNode<Button>("Botao").Show();
		GetNode<Button>("Salaestar").Show();
		GetNode<Button>("BotaoVoltar").Hide();
		GetNode<Button>("VoltarSalaQuarto").Hide();

		// Botao: sala 1 -> sala 2
		GetNode<Button>("Botao").Pressed += () =>
		{
			GetNode<Sprite2D>("Sprite2D").Hide();
			GetNode<Node2D>("SalaDeEstar").Hide();
			GetNode<Sprite2D>("Fundo2").Show();

			GetNode<Button>("Botao").Hide();
			GetNode<Button>("Salaestar").Hide();
			GetNode<Button>("BotaoVoltar").Show();
			GetNode<Button>("VoltarSalaQuarto").Hide();
		};

		// BotaoVoltar: sala 2 -> sala 1
		GetNode<Button>("BotaoVoltar").Pressed += () =>
		{
			GetNode<Sprite2D>("Fundo2").Hide();
			GetNode<Node2D>("SalaDeEstar").Hide();
			GetNode<Sprite2D>("Sprite2D").Show();

			GetNode<Button>("BotaoVoltar").Hide();
			GetNode<Button>("VoltarSalaQuarto").Hide();
			GetNode<Button>("Botao").Show();
			GetNode<Button>("Salaestar").Show();
		};

		// Salaestar: sala 1 -> sala de estar
		GetNode<Button>("Salaestar").Pressed += () =>
		{
			GetNode<Sprite2D>("Sprite2D").Hide();
			GetNode<Sprite2D>("Fundo2").Hide();
			GetNode<Node2D>("SalaDeEstar").Show();

			GetNode<Button>("Salaestar").Hide();
			GetNode<Button>("Botao").Hide();
			GetNode<Button>("BotaoVoltar").Hide();
			GetNode<Button>("VoltarSalaQuarto").Show();
		};

		// VoltarSalaQuarto: sala de estar -> sala 1
		GetNode<Button>("VoltarSalaQuarto").Pressed += () =>
		{
			GetNode<Node2D>("SalaDeEstar").Hide();
			GetNode<Sprite2D>("Fundo2").Hide();
			GetNode<Sprite2D>("Sprite2D").Show();

			GetNode<Button>("VoltarSalaQuarto").Hide();
			GetNode<Button>("BotaoVoltar").Hide();
			GetNode<Button>("Botao").Show();
			GetNode<Button>("Salaestar").Show();
		};

		// Próximo dia
		Button nextDay = GetNode<Button>("NextDay");
		Label textoNextDay = GetNode<Label>("NextDay/TextoNextDay");

		textoNextDay.Hide();

		nextDay.MouseEntered += () => textoNextDay.Show();
		nextDay.MouseExited += () => textoNextDay.Hide();
		nextDay.Pressed += OnButtonPressed;

		// Tarefa escolar (fica dentro da SalaDeEstar)
		GetNode<Button>("SalaDeEstar/TarefaEscolar").Pressed += () =>
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
			tarefaEscolarInstance.TreeExited += () => Show();
		};
	}

	public override void _ExitTree()
	{
		if (gameState != null)
			gameState.DiaMudou -= AoDiaMudar;
	}

	private void OnButtonPressed()
	{
		gameState.NextDay();
	}

	private void AoDiaMudar(int dia)
	{
		GD.Print("Dia mudou para: " + dia);
	}

	public override void _UnhandledInput(InputEvent @event)
	{
		if (@event is InputEventMouseButton { Pressed: true })
			GD.Print("MOUSE CLICADO!");
	}
}