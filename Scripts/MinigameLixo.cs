
using Godot;

public partial class MinigameLixo : Control
{
    private Button[] residuos;
    private Button lixeiraPapel;
    private Button lixeiraPlastico;
    private Button lixeiraOrganico;
    private Label instrucao;
    private Label textoConclusao;

    private Button residuoAtual;
    private int tipoAtual;
    private int acertos = 0;

    private bool arrastando = false;
    private Vector2 posicaoInicial;
    private Vector2 distanciaMouse;

    public bool Concluido { get; private set; } = false;

    private readonly string[] nomesResiduos =
    {
        "Newspaper",
        "Plastic bottle",
        "Banana peel"
    };

    private readonly string[] nomesTipos =
    {
        "Paper",
        "Plastic",
        "Organic"
    };

    private Vector2 posicaoResiduo;

    public override void _Ready()
    {
        residuos = new Button[]
        {
            GetNode<Button>("AreaResiduos/ResiduoPapel"),
            GetNode<Button>("AreaResiduos/ResiduoPlastico"),
            GetNode<Button>("AreaResiduos/ResiduoOrganico")
        };

        lixeiraPapel = GetNode<Button>("LixeiraPapel");
        lixeiraPlastico = GetNode<Button>("LixeiraPlastico");
        lixeiraOrganico = GetNode<Button>("LixeiraOrganico");

        instrucao = GetNode<Label>("Instrucao");
        textoConclusao = GetNode<Label>("TextoConclusao");

        textoConclusao.Hide();

        // All waste items start hidden.
        foreach (Button residuo in residuos)
        {
            residuo.Hide();
        }

        posicaoResiduo = residuos[0].Position;

        GerarResiduo();
    }

    private void GerarResiduo()
    {
        if (acertos >= 5)
        {
            FinalizarMinigame();
            return;
        }

        // Randomly choose the type of the next waste item.
        tipoAtual = (int)GD.RandRange(0, 2);

        // Choose which button will represent the waste item.
        int indiceBotao = (int)GD.RandRange(0, 2);
        residuoAtual = residuos[indiceBotao];

        foreach (Button residuo in residuos)
        {
            residuo.Hide();
        }

        residuoAtual.Position = posicaoResiduo;
        residuoAtual.Text = nomesResiduos[tipoAtual];
        residuoAtual.Show();

        instrucao.Text = $"Waste sorted: {acertos}/5";
    }

    public override void _Input(InputEvent @event)
    {
        if (acertos >= 5 || residuoAtual == null)
            return;

        if (@event is InputEventMouseButton mouseButton &&
            mouseButton.ButtonIndex == MouseButton.Left)
        {
            Vector2 mouse = GetGlobalMousePosition();

            if (mouseButton.Pressed && !arrastando &&
                residuoAtual.GetGlobalRect().HasPoint(mouse))
            {
                arrastando = true;
                distanciaMouse = mouse - residuoAtual.GlobalPosition;
                posicaoInicial = residuoAtual.Position;
                GetViewport().SetInputAsHandled();
            }
            else if (!mouseButton.Pressed && arrastando)
            {
                arrastando = false;
                VerificarLixeira(mouse);
                GetViewport().SetInputAsHandled();
            }
        }

        if (@event is InputEventMouseMotion && arrastando)
        {
            Vector2 mouse = GetGlobalMousePosition();
            residuoAtual.GlobalPosition = mouse - distanciaMouse;

            GetViewport().SetInputAsHandled();
        }
    }

    private void VerificarLixeira(Vector2 mouse)
    {
        Button lixeiraCorreta = tipoAtual switch
        {
            0 => lixeiraPapel,
            1 => lixeiraPlastico,
            _ => lixeiraOrganico
        };

        if (lixeiraCorreta.GetGlobalRect().HasPoint(mouse))
        {
            acertos++;

            if (acertos >= 5)
            {
                FinalizarMinigame();
            }
            else
            {
                GerarResiduo();
            }
        }
        else
        {
            // Wrong bin: the waste item returns to its original position.
            residuoAtual.Position = posicaoInicial;
            instrucao.Text =
                $"Wrong bin! Try again. ({acertos}/5)";
        }
    }

    private async void FinalizarMinigame()
    {
        Concluido = true;

        foreach (Button residuo in residuos)
        {
            residuo.Hide();
        }

        lixeiraPapel.Hide();
        lixeiraPlastico.Hide();
        lixeiraOrganico.Hide();

        instrucao.Hide();
        textoConclusao.Text = "Waste sorted! Task completed!";
        textoConclusao.Show();

        await ToSignal(
            GetTree().CreateTimer(1.5),
            SceneTreeTimer.SignalName.Timeout
        );

        QueueFree();
    }
}
