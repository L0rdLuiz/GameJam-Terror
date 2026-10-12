using Godot;
using System;

public partial class GameState : Node
{
	public int Day { get; private set; } = 1;

	public void NextDay()
	{
		Day++;
		GD.Print("Day: " + Day);
	}
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GD.Print("Day: " + Day);
	}

}
