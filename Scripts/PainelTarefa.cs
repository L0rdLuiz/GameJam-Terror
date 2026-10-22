
using Godot;
using System.Collections.Generic;

public partial class PainelTarefa : CanvasLayer
{
    private GameState gameState;
    private Label nomeTarefa;
    private Label dia;

    public override void _Ready()
    {
        gameState = GetNode<GameState>("/root/GameState");

        nomeTarefa = GetNode<Label>(
            "PanelContainer/VBoxContainer/NomeTarefa"
        );

        dia = GetNode<Label>(
            "PanelContainer/VBoxContainer/Dia"
        );

        gameState.TarefaConcluida += AoTarefaConcluir;
        gameState.DiaMudou += AoDiaMudar;

        AtualizarPainel();
    }

    private void AoTarefaConcluir(string nome)
    {
        AtualizarPainel();
    }

    private void AoDiaMudar(int novoDia)
    {
        AtualizarPainel();
    }

    private void AtualizarPainel()
    {
        dia.Text = "Dia " + gameState.Day;

        List<string> tarefas = gameState.PegarTarefasDoDia();

        foreach (string tarefa in tarefas)
        {
            if (tarefa == GameState.FinalizarDia)
                continue;

            if (!gameState.TarefaEstaFeita(tarefa))
            {
                nomeTarefa.Text = TraduzirTarefa(tarefa);
                return;
            }
        }

        if (gameState.PossoFinalizarDia())
        {
            nomeTarefa.Text = "Dia concluído!";
        }
        else
        {
            nomeTarefa.Text = "Nenhuma tarefa disponível";
        }
    }

    private string TraduzirTarefa(string tarefa)
    {
        return tarefa switch
        {
            GameState.Almoco => "Preparar o almoço",
            GameState.DobrarRoupa => "Dobrar as roupas",
            GameState.Janta => "Preparar o jantar",
            GameState.TarefaEscolar => "Ajudar na tarefa escolar",
            GameState.LevarLixo => "Levar o lixo",
            GameState.Brincar => "Brincar com Sugarplum",
            GameState.AjeitarBrinquedos => "Arrumar os brinquedos",
            GameState.BuscarRemedio => "Buscar o remédio",
            GameState.Pintar => "Pintar",
            _ => tarefa
        };
    }

    public override void _ExitTree()
    {
        if (gameState != null)
        {
            gameState.TarefaConcluida -= AoTarefaConcluir;
            gameState.DiaMudou -= AoDiaMudar;
        }
    }
}
