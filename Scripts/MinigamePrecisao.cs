
using Godot;

public partial class MinigamePrecisao : Node2D
{
	private float posicao = 0f;
	private float velocidade = 200f;
	private float direcao = 1f;

	private ColorRect barra;
	private ColorRect zonaVerde;
	private ColorRect sinal;

	private bool finalizado = false;

	private int acertos = 0;
	private int erros = 0;

	private RandomNumberGenerator rng = new RandomNumberGenerator();

	public override void _Ready()
	{
		barra = GetNode<ColorRect>("Barra");
		zonaVerde = GetNode<ColorRect>("ZonaVerde");
		sinal = GetNode<ColorRect>("Sinal");

		rng.Randomize();

		PosicionarZonaVerdeAleatoria();
		AtualizarPosicaoVisual();
	}

	public override void _Process(double delta)
	{
		if (finalizado)
			return;

		float tempoRestante = (float)delta;

		float limiteEsquerdo = 0f;
		float limiteDireito = barra.Size.X - sinal.Size.X;

		while (tempoRestante > 0f)
		{
			float limiteAtual;
			float distanciaAteBorda;

			if (direcao > 0f)
			{
				limiteAtual = limiteDireito;
				distanciaAteBorda = limiteAtual - posicao;
			}
			else
			{
				limiteAtual = limiteEsquerdo;
				distanciaAteBorda = posicao - limiteAtual;
			}

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
		float minimo = barra.Position.X;
		float maximo = barra.Position.X
			+ barra.Size.X
			- zonaVerde.Size.X;

		float novaPosicao = rng.RandfRange(minimo, maximo);

		zonaVerde.Position = new Vector2(
			novaPosicao,
			zonaVerde.Position.Y
		);
	}

	public override void _Input(InputEvent @event)
	{
		if (finalizado)
			return;

		if (@event is InputEventMouseButton mouseButton
			&& mouseButton.Pressed
			&& mouseButton.ButtonIndex == MouseButton.Left)
		{
			VerificarAcerto();
		}
	}

	private void VerificarAcerto()
	{
		float centroSinal = barra.Position.X
			+ posicao
			+ sinal.Size.X / 2f;

		float inicioZona = zonaVerde.Position.X;
		float fimZona = inicioZona + zonaVerde.Size.X;

		if (centroSinal >= inicioZona && centroSinal <= fimZona)
		{
			acertos++;

			GD.Print($"ACERTOU! {acertos}/4 acertos.");

			if (acertos >= 4)
			{
				finalizado = true;
				GD.Print("VITÓRIA! Você completou o minigame!");
				return;
			}

			// Diminui a zona verde em 15 pixels.
			float novaLargura = Mathf.Max(
				35f,
				zonaVerde.Size.X - 15f
			);

			zonaVerde.Size = new Vector2(
				novaLargura,
				zonaVerde.Size.Y
			);

			// Sorteia uma nova posição para a zona verde.
			PosicionarZonaVerdeAleatoria();
		}
		else
		{
			erros++;

			GD.Print($"ERROU! {erros}/3 erros.");

			if (erros >= 3)
			{
				finalizado = true;
				GD.Print("DERROTA! Você errou 3 vezes.");
			}
		}
	}
}
