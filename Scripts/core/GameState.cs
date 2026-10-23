using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class GameState : Node
{
	public int Day { get; private set; } = 1;

	//Tarefas
	public const string Almoco = "almoco";
	public const string DobrarRoupa = "dobrar_roupa";
	public const string Janta = "janta";
	public const string TarefaEscolar = "tarefa_escolar";
	public const string LevarLixo = "levar_lixo";
	public const string Brincar = "brincar";
	public const string AjeitarBrinquedos = "ajeitar_brinquedos";
	public const string BuscarRemedio = "buscar_remedio";
	public const string TomarRemedio = "tomar_remedio";
	public const string HistoriaFilho = "historia_filho";

	public const string Pintar = "pintar";


	public void NextDay()
	{
		tarefasConcluidas.Clear();
		Day++;
		GD.Print("Day: " + Day);
		PedirAviso("Day: " + Day);
		EmitSignal(SignalName.DiaMudou, Day);
	}
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	private Dictionary<int, List<string>> tarefasDoDia = new Dictionary<int, List<string>>
	{
		{ 1, new List<string> { Almoco, DobrarRoupa, Janta, TarefaEscolar, TomarRemedio, HistoriaFilho } },
		{ 2, new List<string> { Almoco, LevarLixo, Janta, Brincar, TomarRemedio, HistoriaFilho } },
		{ 3, new List<string> { Almoco, AjeitarBrinquedos, Janta, TarefaEscolar, TomarRemedio, HistoriaFilho } },
		{ 4, new List<string> { Almoco, LevarLixo, Janta, BuscarRemedio, Brincar, TomarRemedio, HistoriaFilho } },
		{ 5, new List<string> { Almoco, AjeitarBrinquedos, Janta, TarefaEscolar, TomarRemedio, HistoriaFilho } },
	};

	private HashSet<string> tarefasConcluidas = new HashSet<string>();

	public void ConcluirTarefa(string nomeTarefa) 
	{
		if (tarefasConcluidas.Add(nomeTarefa)) 
		{
			EmitSignal(SignalName.TarefaConcluida, nomeTarefa);
		}
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

	public bool PossoFazerATarefa(string nomeTarefa)
	{
		List<string> tarefas = PegarTarefasDoDia();
		int posicao = tarefas.IndexOf(nomeTarefa);

		if (posicao == -1)
		{
			return false;
		}
		if (TarefaEstaFeita(tarefas[posicao]))
		{
			return false;
		}

		for (int i = 0; i < posicao; i++)
		{
			if (!TarefaEstaFeita(tarefas[i]))
			{
				return false;
			}
		}
		return true;
	}

	public void PedirAviso(string texto)
	{
		EmitSignal(SignalName.AvisoPedido, texto);
	}

	[Signal]
	public delegate void DiaMudouEventHandler(int novoDia);
	
	[Signal]
	public delegate void TarefaConcluidaEventHandler(string nomeTarefa);

	[Signal]
	public delegate void AvisoPedidoEventHandler(string texto);

}
