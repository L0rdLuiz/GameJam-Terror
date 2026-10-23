using Godot;

public partial class MinigameRoupa : Control
{
	private Label titulo;
	private TextureButton areaRoupa;
	private Label instrucao;
	private AudioStreamPlayer somDobrar;
	private Label textoConclusao;
	private Sprite2D[] fundos;

	private int etapaDobra = 0;
	private const int TotalEtapas = 3;   // quantidade de cliques para terminar

	public override void _Ready()
	{
		titulo = GetNode<Label>("Titulo");
		instrucao = GetNode<Label>("Instrucao");
		somDobrar = GetNode<AudioStreamPlayer>("SomDobrar");
		textoConclusao = GetNode<Label>("TextoConclusao");

		// Por enquanto o AreaRoupa está DENTRO do FundoRoupa1.
		// Quando criar os outros fundos, tire o AreaRoupa de dentro do FundoRoupa1
		// (arraste para a raiz) e troque o caminho para "AreaRoupa".
		areaRoupa = GetNode<TextureButton>("AreaRoupa");
		fundos = new[]
		{
			GetNode<Sprite2D>("FundoRoupa1"),
			 GetNode<Sprite2D>("FundoRoupa2"),
			 GetNode<Sprite2D>("FundoRoupa3"),
			 GetNode<Sprite2D>("FundoRoupa4"),
		};

		MostrarFundo(0);   // começa com o FundoRoupa1

		titulo.Text = "Dobrar roupas";
		instrucao.Text = "Clique na roupa para começar a dobrar.";
		textoConclusao.Hide();

		areaRoupa.Pressed += AoClicarNaRoupa;
	}

	// Mostra só o fundo de índice i. Se esse fundo ainda não existe, não muda nada
	private void MostrarFundo(int i)
{
    if (i < 0 || i >= fundos.Length)
        return;

    for (int j = 0; j < fundos.Length; j++)
    {
        fundos[j].Visible = (j == i);
    }
}

	private async void AoClicarNaRoupa()
	{
		if (etapaDobra >= TotalEtapas)
			return;

		etapaDobra++;

		// 1º clique = FundoRoupa2, 2º = FundoRoupa3, 3º = FundoRoupa4
		// (só troca quando esses fundos forem descomentados acima)
		MostrarFundo(etapaDobra);

		if (somDobrar.Stream != null)
			somDobrar.Play();

		if (etapaDobra < TotalEtapas)
		{
			instrucao.Text = $"Dobrando roupa... ({etapaDobra}/{TotalEtapas})";
		}
		else
		{
			instrucao.Text = "Você terminou de dobrar a roupa!";
			areaRoupa.Disabled = true;
			textoConclusao.Text = "Tarefa concluída!";
			textoConclusao.Show();

			await ToSignal(
				GetTree().CreateTimer(1.0),
				SceneTreeTimer.SignalName.Timeout
			);

			QueueFree();
		}
	}
}