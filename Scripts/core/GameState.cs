using Godot;
using System;

public partial class GameState : Node
{
	public int Day { get; private set; } = 1;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GD.Print("Day: " + Day);
	}

}
