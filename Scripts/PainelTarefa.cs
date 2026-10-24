
using Godot;
using System.Collections.Generic;

public partial class PainelTarefa : CanvasLayer
{
    private GameState gameState;
    private Control areaTarefa;
    private Control painel;
    private Button botaoTarefa;
    private Label nomeTarefa;
    private Label dia;

    public override void _Ready()
    {
        gameState = GetNode<GameState>("/root/GameState");

        areaTarefa = GetNode<Control>("AreaTarefa");
        painel = GetNode<Control>("AreaTarefa/PanelContainer");
        botaoTarefa = GetNode<Button>("AreaTarefa/BotaoTarefa");

        nomeTarefa = GetNode<Label>(
            "AreaTarefa/PanelContainer/VBoxContainer/NomeTarefa"
        );

        dia = GetNode<Label>(
            "AreaTarefa/PanelContainer/VBoxContainer/Dia"
        );

        painel.Hide();

        areaTarefa.MouseEntered += AoEntrarNaArea;
        areaTarefa.MouseExited += AoSairDaArea;

        gameState.TarefaConcluida += AoTarefaConcluir;
        gameState.DiaMudou += AoDiaMudar;

        AtualizarPainel();
    }

    private void AoEntrarNaArea()
    {
        painel.Show();
    }

    private void AoSairDaArea()
    {
        painel.Hide();
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
        dia.Text = "Day " + gameState.Day;

        List<string> tarefas = gameState.PegarTarefasDoDia();

        foreach (string tarefa in tarefas)
        {
            if (!gameState.TarefaEstaFeita(tarefa))
            {
                nomeTarefa.Text = TraduzirTarefa(tarefa);
                return;
            }
        }

        nomeTarefa.Text = gameState.DiaCompleto()
            ? "Day completed!"
            : "No tasks available";
    }

    private string TraduzirTarefa(string tarefa)
    {
        return tarefa switch
        {
            GameState.Almoco => "Prepare lunch",
            GameState.LerLivro => "Read a book",
            GameState.DobrarRoupa => "Fold the laundry",
            GameState.Janta => "Prepare dinner",
            GameState.TarefaEscolar => "Help with homework",
            GameState.TomarRemedio => "Take medication",
            GameState.HistoriaFilho => "Read a bedtime story",
            GameState.LevarLixo => "Take out the trash",
            GameState.Pintar => "Paint",
            GameState.Brincar => "Play with Sugarplum",
            GameState.AjeitarBrinquedos => "Tidy up the toys",
            GameState.BuscarRemedio => "Get the medicine",
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

        if (areaTarefa != null)
        {
            areaTarefa.MouseEntered -= AoEntrarNaArea;
            areaTarefa.MouseExited -= AoSairDaArea;
        }
    }
}
