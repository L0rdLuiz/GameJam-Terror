
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
        "Jornal",
        "Garrafa plástica",
        "Casca de banana"
    };

    private readonly string[] nomesTipos =
    {
        "Papel",
        "Plástico",
        "Orgânico"
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

        // Todos os resíduos começam escondidos.
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

        // Escolhe aleatoriamente o tipo do próximo resíduo.
        tipoAtual = (int)GD.RandRange(0, 2);

        // Escolhe qual botão será usado para representar o resíduo.
        int indiceBotao = (int)GD.RandRange(0, 2);
        residuoAtual = residuos[indiceBotao];

        foreach (Button residuo in residuos)
        {
            residuo.Hide();
        }

        residuoAtual.Position = posicaoResiduo;
        residuoAtual.Text = nomesResiduos[tipoAtual];
        residuoAtual.Show();

        instrucao.Text = $"Resíduos separados: {acertos}/5";
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
            // Errou: o resíduo volta ao lugar e continua o mesmo.
            residuoAtual.Position = posicaoInicial;
            instrucao.Text =
                $"Lixeira incorreta! Tente novamente. ({acertos}/5)";
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
        textoConclusao.Text = "Lixo separado! Tarefa concluída!";
        textoConclusao.Show();

        await ToSignal(
            GetTree().CreateTimer(1.5),
            SceneTreeTimer.SignalName.Timeout
        );

        QueueFree();
    }
}
