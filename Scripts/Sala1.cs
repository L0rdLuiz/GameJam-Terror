using Godot;
using System;

public partial class Sala1 : Node2D
{
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
}
	
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _UnhandledInput(InputEvent @event)
	{
		if(@event is InputEventMouseButton {Pressed: true})
		GD.Print("MOUSE CLICADO!");
	}
}
