using Godot;
using System.Collections.Generic;
using System.Linq;

public partial class GameState : Node
{
    public int Day { get; private set; } = 1;

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
    public const string LerLivro = "ler_livro";
    public const string Pintar = "pintar";

    private readonly Dictionary<int, List<string>> tarefasDoDia =
        new()
        {
            { 1, new List<string>
                {
                    Almoco, LerLivro, DobrarRoupa, Janta,
                    TarefaEscolar, TomarRemedio, HistoriaFilho
                }
            },
            { 2, new List<string>
                {
                    Almoco, LevarLixo, LerLivro, Janta,
                    Pintar, TomarRemedio, HistoriaFilho
                }
            },
            // Dia 3: almoço -> arrumar brinquedos -> livro -> janta -> remédio (vazio) -> história
            { 3, new List<string>
                {
                    Almoco, AjeitarBrinquedos, LerLivro, Janta,
                    TomarRemedio, HistoriaFilho
                }
            },
            { 4, new List<string>
                {
                    Almoco, LevarLixo, Janta, BuscarRemedio,
                    Brincar, TomarRemedio, HistoriaFilho
                }
            },
            { 5, new List<string>
                {
                    Almoco, AjeitarBrinquedos, Janta,
                    TarefaEscolar, TomarRemedio, HistoriaFilho
                }
            }
        };

    private readonly HashSet<string> tarefasConcluidas = new();

    private readonly Dictionary<int, List<string>> tarefasPreConcluidas =
        new()
        {
            { 2, new List<string> { Pintar } }
        };

    public override void _Ready()
    {
        AplicarTarefasPreConcluidas();
    }

    private void AplicarTarefasPreConcluidas()
    {
        if (!tarefasPreConcluidas.TryGetValue(Day, out var lista))
            return;

        foreach (string tarefa in lista)
            tarefasConcluidas.Add(tarefa);
    }

    public void ConcluirTarefa(string nomeTarefa)
    {
        if (tarefasConcluidas.Add(nomeTarefa))
        {
            EmitSignal(SignalName.TarefaConcluida, nomeTarefa);
            GD.Print("Tarefa concluída: " + nomeTarefa);
        }
    }

    public bool TarefaEstaFeita(string nomeTarefa)
    {
        return tarefasConcluidas.Contains(nomeTarefa);
    }

    public List<string> PegarTarefasDoDia()
    {
        return tarefasDoDia.TryGetValue(Day, out var lista)
            ? lista
            : new List<string>();
    }

    public bool DiaCompleto()
    {
        return PegarTarefasDoDia().All(TarefaEstaFeita);
    }

    public bool PossoFazerATarefa(string nomeTarefa)
    {
        List<string> tarefas = PegarTarefasDoDia();
        int posicao = tarefas.IndexOf(nomeTarefa);

        if (posicao == -1 || TarefaEstaFeita(nomeTarefa))
            return false;

        for (int i = 0; i < posicao; i++)
        {
            if (!TarefaEstaFeita(tarefas[i]))
                return false;
        }

        return true;
    }

    public bool PodeVirarODia()
    {
        return DiaCompleto();
    }

    public void TentarVirarODia()
    {
        if (!DiaCompleto())
        {
            PedirAviso("Cherry ainda precisa terminar as tarefas do dia.");
            GD.Print("Ainda existem tarefas pendentes.");
            return;
        }

        NextDay();
    }

    public void NextDay()
    {
        tarefasConcluidas.Clear();
        Day++;

        AplicarTarefasPreConcluidas();

        GD.Print("Day: " + Day);
        PedirAviso("Day: " + Day);
        EmitSignal(SignalName.DiaMudou, Day);
    }

    public void DebugPularDia()
    {
        foreach (string tarefa in PegarTarefasDoDia())
            tarefasConcluidas.Add(tarefa);

        NextDay();
    }

    public void DebugConcluirTarefasDoDia()
    {
        foreach (string tarefa in PegarTarefasDoDia())
            tarefasConcluidas.Add(tarefa);

        EmitSignal(SignalName.TarefaConcluida, "debug");
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
