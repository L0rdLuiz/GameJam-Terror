using Godot;
using System;

public partial class DebugTarefas : Node2D
{

	GameState gameState;

	private void Imprimir()
    {
        GD.Print("--- Tarefas do Dia " + gameState.Day + " ---");
        var tarefas = gameState.PegarTarefasDoDia();
        for (int i = 0; i < tarefas.Count; i++)
        {
            string marca = gameState.TarefaEstaFeita(tarefas[i]) ? "[x]" : "[ ]";
            GD.Print((i + 1) + ". " + marca + " " + tarefas[i]);
        }
    }

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		gameState = GetNode<GameState>("/root/GameState");
		Imprimir();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
