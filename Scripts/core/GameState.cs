using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

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

	public void ConcluirTarefa(string nomeTarefa) {
      tarefasConcluidas.Add(nomeTarefa);
	}

}
