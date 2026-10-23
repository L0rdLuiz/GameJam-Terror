using Godot;

public partial class MinigameRoupa : Control
{
    private Label titulo;
    private TextureButton areaRoupa;
    private Label instrucao;
    private AudioStreamPlayer somDobrar;
    private Label textoConclusao;

    private int etapaDobra = 0;
    private const int TotalEtapas = 3;

    public override void _Ready()
    {
        titulo = GetNode<Label>("Titulo");
        areaRoupa = GetNode<TextureButton>("AreaRoupa");
        instrucao = GetNode<Label>("Instrucao");
        somDobrar = GetNode<AudioStreamPlayer>("SomDobrar");
        textoConclusao = GetNode<Label>("TextoConclusao");

        titulo.Text = "Dobrar roupas";
        instrucao.Text = "Clique na roupa para começar a dobrar.";
        textoConclusao.Hide();

        // Temporário: botão visível até termos a imagem da roupa.
        areaRoupa.TextureNormal = null;
		areaRoupa.CustomMinimumSize = new Vector2(250, 250);
		areaRoupa.Size = new Vector2(250, 250);
        areaRoupa.Pressed += AoClicarNaRoupa;
    }

    private async void AoClicarNaRoupa()
	{
		if (etapaDobra >= TotalEtapas)
			return;

		etapaDobra++;

		if (somDobrar.Stream != null)
			somDobrar.Play();

		if (etapaDobra < TotalEtapas)
		{
			instrucao.Text =
				$"Dobrando roupa... ({etapaDobra}/{TotalEtapas})";
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
