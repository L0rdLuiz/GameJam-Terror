using Godot;
using System;

public partial class Sala1 : Node2D
{
	GameState gameState;

	public override void _Ready()
	{									//	QUANDO, ENT FAÇA
		GetNode<Button>("Botao").Pressed += () =>
		{
			GetNode<Sprite2D>("Sprite2D").Hide();
			GetNode<Sprite2D>("Fundo2").Show();
		};
			GetNode<Button>("BotaoVoltar").Pressed += () =>
		{
			GetNode<Sprite2D>("Fundo2").Hide();
			GetNode<Sprite2D>("Sprite2D").Show();
		};

		GetNode<Button>("NextDay").Pressed += () =>
		{
			OnButtonPressed();;
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
