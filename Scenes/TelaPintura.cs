using Godot;
using System;

public partial class TelaPintura : CanvasLayer
{
	private int _largura;
	private int _altura;

	private Image _imagem;
	private ImageTexture _textura;
	private TextureRect _tela;

	//cor e tamanho do pincel
	private Color _corAtual = Colors.Black;
	private int _raioPincel = 6;
	public GameState gameState;

	// Avisa o Sala1 quando o jogador fecha a pintura
	public event Action PinturaFechada;

	public override void _Ready()
	{
		_tela = GetNode<TextureRect>("Tela");
		Control fundo = GetNode<Control>("Fundo");

		// tamanho da janela do jogo
		Vector2 tamanho = GetViewport().GetVisibleRect().Size;
		_largura = (int)tamanho.X;
		_altura = (int)tamanho.Y;

		// Tela e Fundo ocupam a janela inteira
		_tela.CustomMinimumSize = Vector2.Zero;
		_tela.ExpandMode = TextureRect.ExpandModeEnum.IgnoreSize;
		_tela.StretchMode = TextureRect.StretchModeEnum.Keep;
		_tela.Position = Vector2.Zero;
		_tela.Size = tamanho;

		fundo.Position = Vector2.Zero;
		fundo.Size = tamanho;

		//criando fundo branco pra tela, que cria uma memória pixel por pixel
		_imagem = Image.CreateEmpty(_largura, _altura, false, Image.Format.Rgba8);
		_imagem.Fill(Colors.White);

		//convertendo a imagem em uma textura para conseguir exibir na tela
		_textura = ImageTexture.CreateFromImage(_imagem);
		_tela.Texture = _textura;

		GetNode<Button>("FecharPintura").Pressed += () =>
		{
			Hide();
			PinturaFechada?.Invoke();   // quem decide a tarefa é o Sala1
		};
	}

	// Apaga o desenho (usado quando começa um novo dia)
	public void Limpar()
	{
		if (_imagem == null) return;
		_imagem.Fill(Colors.White);
		_textura.Update(_imagem);
	}

	public override void _Process(double delta)
	{
		// se a tela estiver escondida, não pinta
		if (!Visible)
			return;

		if (!Input.IsMouseButtonPressed(MouseButton.Left))
			return;

		// posição do mouse relativa ao canto superior esquerdo da Tela
		Vector2 pos = _tela.GetLocalMousePosition();
		Pintar((int)pos.X, (int)pos.Y);
	}

	private void Pintar(int cx, int cy)
	{
		for (int x = -_raioPincel; x <= _raioPincel; x++)
		{
			for (int y = -_raioPincel; y <= _raioPincel; y++)
			{
				// só pinta se estiver dentro do círculo (pincel redondo)
				if (x * x + y * y > _raioPincel * _raioPincel)
					continue;

				int px = cx + x;
				int py = cy + y;

				// não deixa pintar fora da imagem
				if (px < 0 || px >= _largura || py < 0 || py >= _altura)
					continue;

				_imagem.SetPixel(px, py, _corAtual);
			}
		}

		// avisa a textura que a imagem mudou
		_textura.Update(_imagem);
	}
}