using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class GameState : Node
{
	public int Day { get; private set; } = 1;

	public void NextDay()
	{
		tarefasConcluidas.Clear();
		Day++;
		GD.Print("Day: " + Day);
		EmitSignal(SignalName.DiaMudou, Day);
	}
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	private Dictionary<int, List<string>> tarefasDoDia = new Dictionary<int, List<string>>
	{
		{ 1, new List<string> { "almoco", "dobrar_roupa", "janta", "tarefa_escolar", "finalizar_dia" } },
		{ 2, new List<string> { "almoco", "levar_lixo", "janta", "brincar", "finalizar_dia" } },
		{ 3, new List<string> { "almoco", "ajeitar_brinquedos", "janta", "tarefa_escolar", "finalizar_dia" } },
		{ 4, new List<string> { "almoco", "levar_lixo", "janta","buscar_remedio", "brincar", "finalizar_dia" } },
		{ 5, new List<string> { "almoco", "ajeitar_brinquedos", "janta", "tarefa_escolar", "finalizar_dia" } },
	};

	private HashSet<string> tarefasConcluidas = new HashSet<string>();

	public void ConcluirTarefa(string nomeTarefa) 
	{
		tarefasConcluidas.Add(nomeTarefa);
		EmitSignal(SignalName.TarefaConcluida, nomeTarefa);
	}

	public bool TarefaEstaFeita(string nomeTarefa)
	{
		return tarefasConcluidas.Contains(nomeTarefa);
	}

	public List<string> PegarTarefasDoDia()
	{
		if (tarefasDoDia.TryGetValue(Day, out List<string> lista))
		{
			return lista;
		}
    	return new List<string>();
	}

	public bool DiaCompleto()
	{
		var tarefasDoDiaAtual = PegarTarefasDoDia();
		return tarefasDoDiaAtual.All(tarefasConcluidas => TarefaEstaFeita(tarefasConcluidas));
	}

	public bool PossoFinalizarDia()
	{
		foreach (var tarefa in PegarTarefasDoDia())
		{
			if (!TarefaEstaFeita(tarefa) && tarefa != "finalizar_dia")
			{
				return false;
			}
		}
		return true;
	}

	public void TentarVirarODia()
	{
		if (DiaCompleto())
		{
			NextDay();
		}
		else
		{
			GD.Print("Ainda não concluiu todas as tarefas do dia.");
		}
	}

	[Signal]
	public delegate void DiaMudouEventHandler(int novoDia);
	
	[Signal]
	public delegate void TarefaConcluidaEventHandler(string nomeTarefa);

}
