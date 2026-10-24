using Godot;
using System.Collections.Generic;

public partial class MinigamePrecisao : Node2D
{
    private float posicao = 0f;
    private float velocidade = 500f;
    private float direcao = 1f;

    private ColorRect barra;
    private ColorRect zonaVerde;
    private ColorRect sinal;

    private bool finalizado = false;
    private int acertos = 0;
    private int erros = 0;
    private const int ERROS_MAXIMOS = 3;

    private int acertosNecessarios = 4;
    private CanvasItem[] imagens = System.Array.Empty<CanvasItem>();

    private readonly RandomNumberGenerator rng = new RandomNumberGenerator();

    public override void _Ready()
    {
        barra = GetNodeOrNull<ColorRect>("Barra");
        zonaVerde = GetNodeOrNull<ColorRect>("ZonaVerde");
        sinal = GetNodeOrNull<ColorRect>("Sinal");

        if (barra == null || zonaVerde == null || sinal == null)
        {
            GD.PushError("MinigamePrecisao: confira se existem os nós Barra, ZonaVerde e Sinal com esses nomes.");
            finalizado = true;
            return;
        }

        // Procura as imagens dentro da cena, aceitando nomes com maiúsculas ou minúsculas.
        var lista = new List<CanvasItem>();
        for (int i = 1; i <= 3; i++)
        {
            Node no = FindChild("carne" + i, true, false)
                ?? FindChild("Carne" + i, true, false);

            if (no is CanvasItem item)
                lista.Add(item);
        }

        imagens = lista.ToArray();

        if (imagens.Length > 0)
        {
            acertosNecessarios = imagens.Length;
            MostrarImagem(0);
            GD.Print($"Minigame de precisão: {imagens.Length} imagens de carne encontradas.");
        }
        else
        {
            GD.PushWarning("Minigame de precisão: nenhuma imagem carne1/carne2/carne3 foi encontrada. O minigame funcionará sem trocar imagens.");
        }

        rng.Randomize();
        posicao = 0f;
        direcao = 1f;
        PosicionarZonaVerdeAleatoria();
        AtualizarPosicaoVisual();
    }

    private void MostrarImagem(int indice)
    {
        for (int i = 0; i < imagens.Length; i++)
            imagens[i].Visible = (i == indice);

        if (indice >= 0 && indice < imagens.Length)
            GD.Print($"Imagem atual: {imagens[indice].Name}");
    }

    public override void _Process(double delta)
    {
        if (finalizado || barra == null || sinal == null)
            return;

        float limiteEsquerdo = 0f;
        float limiteDireito = Mathf.Max(0f, barra.Size.X - sinal.Size.X);

        float tempoRestante = (float)delta;

        // Trata também o caso em que o marcador percorre mais de uma borda em um frame.
        while (tempoRestante > 0f)
        {
            float limiteAtual = direcao > 0f ? limiteDireito : limiteEsquerdo;
            float distanciaAteBorda = direcao > 0f
                ? limiteAtual - posicao
                : posicao - limiteAtual;

            if (distanciaAteBorda <= 0.001f)
            {
                posicao = limiteAtual;
                direcao *= -1f;
                continue;
            }

            float tempoAteBorda = distanciaAteBorda / velocidade;

            if (tempoRestante < tempoAteBorda)
            {
                posicao += velocidade * direcao * tempoRestante;
                tempoRestante = 0f;
            }
            else
            {
                posicao = limiteAtual;
                tempoRestante -= tempoAteBorda;
                direcao *= -1f;
            }
        }

        AtualizarPosicaoVisual();
    }

    private void AtualizarPosicaoVisual()
    {
        sinal.Position = new Vector2(
            barra.Position.X + posicao,
            sinal.Position.Y
        );
    }

    private void PosicionarZonaVerdeAleatoria()
    {
        float maximo = Mathf.Max(
            barra.Position.X,
            barra.Position.X + barra.Size.X - zonaVerde.Size.X
        );

        float novaPosicao = rng.RandfRange(barra.Position.X, maximo);

        zonaVerde.Position = new Vector2(
            novaPosicao,
            zonaVerde.Position.Y
        );
    }

    public override void _Input(InputEvent evento)
    {
        if (evento is InputEventKey tecla &&
            tecla.Pressed &&
            !tecla.Echo &&
            tecla.Keycode == Key.Escape)
        {
            QueueFree();
            return;
        }

        if (finalizado)
            return;

        if (evento is InputEventMouseButton mouse &&
            mouse.Pressed &&
            mouse.ButtonIndex == MouseButton.Left)
        {
            VerificarAcerto();
        }
    }

    private void VerificarAcerto()
    {
        if (barra == null || zonaVerde == null || sinal == null)
            return;

        // Compara os retângulos na coordenada global, evitando erros se os nós
        // estiverem deslocados dentro de um Node2D ou CanvasItem.
        Rect2 retanguloSinal = sinal.GetGlobalRect();
        Rect2 retanguloZona = zonaVerde.GetGlobalRect();

        float centroSinal = retanguloSinal.Position.X + retanguloSinal.Size.X / 2f;
        bool acertou = centroSinal >= retanguloZona.Position.X
            && centroSinal <= retanguloZona.End.X;

        if (acertou)
        {
            acertos++;
            GD.Print($"ACERTOU! {acertos}/{acertosNecessarios} acertos.");

            // Após cada acerto, mostra a próxima imagem, se houver.
            if (imagens.Length > 0 && acertos < imagens.Length)
                MostrarImagem(acertos);

            if (acertos >= acertosNecessarios)
            {
                finalizado = true;
                GD.Print("VITÓRIA! Você completou o minigame!");
                QueueFree();
                return;
            }

            // Diminui a largura da zona verde para aumentar a dificuldade.
            float novaLargura = Mathf.Max(35f, zonaVerde.Size.X - 15f);
            zonaVerde.Size = new Vector2(novaLargura, zonaVerde.Size.Y);

            PosicionarZonaVerdeAleatoria();
        }
        else
        {
            erros++;
            GD.Print($"ERROU! {erros}/{ERROS_MAXIMOS} erros.");

            if (erros >= ERROS_MAXIMOS)
            {
                finalizado = true;
                GD.Print("DERROTA! Você errou 3 vezes.");
                QueueFree();
            }
        }
    }
}
