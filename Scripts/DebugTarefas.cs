using Godot;
using System;

public partial class DebugTarefas : Node2D
{

	GameState gameState;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		gameState = GetNode<GameState>("/root/GameState");
		gameState.DiaMudou += AoDiaMudar;
		gameState.TarefaConcluida += AoTarefaConcluir;
		Imprimir();
	}

	public override void _ExitTree()
	{
		gameState.DiaMudou -= AoDiaMudar;
		gameState.TarefaConcluida -= AoTarefaConcluir;
	}

	private void AoDiaMudar(int dia)
	{
		Imprimir();
	}

	private void AoTarefaConcluir(string nomeTarefa)
	{
		Imprimir();
	}

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

	public override void _UnhandledInput(InputEvent @event)
	{
		if (@event is InputEventKey tecla && tecla.Pressed && !tecla.Echo)
		{
			int indice = (int)tecla.Keycode - (int)Key.Key1;
			var tarefas = gameState.PegarTarefasDoDia();

			if (indice >= 0 && indice < tarefas.Count)
			{
				string tarefa = tarefas[indice];

				if (gameState.PossoFazerATarefa(tarefa))
				{
					gameState.ConcluirTarefa(tarefa);
				}
				else
				{
					GD.Print("Você precisa concluir as tarefas anteriores primeiro.");
				}
			}
		}
	}

}
