
using Godot;

public partial class MinigamePincel : Control
{
	private Sprite2D pincelArrumado;
	private Sprite2D pincelDesarrumado;
	private TextureButton areaPinceis;

	private bool tarefaConcluida = false;

	public override void _Ready()
	{
		GD.Print("Iniciando minigame dos pincéis.");

		// Busca os nós diretamente dentro do nó raiz.
		pincelArrumado = GetNodeOrNull<Sprite2D>("PincelArrumado");
		pincelDesarrumado = GetNodeOrNull<Sprite2D>("PincelDesarrumado");
		areaPinceis = GetNodeOrNull<TextureButton>("AreaPinceis");

		// Verifica se os caminhos estão corretos.
		if (pincelArrumado == null ||
			pincelDesarrumado == null ||
			areaPinceis == null)
		{
			GD.PrintErr("Não foi possível encontrar todos os nós do minigame.");

			foreach (Node filho in GetChildren())
				GD.Print($"Nó encontrado: {filho.Name} ({filho.GetType().Name})");

			return;
		}

		pincelArrumado.Hide();
		pincelDesarrumado.Show();

		areaPinceis.Pressed += ArrumarPinceis;

		GD.Print("Minigame iniciado. Botão conectado!");
	}

	private async void ArrumarPinceis()
	{
		if (tarefaConcluida)
			return;

		tarefaConcluida = true;

		pincelDesarrumado.Hide();
		pincelArrumado.Show();

		GD.Print("Pincéis arrumados!");

		await ToSignal(
			GetTree().CreateTimer(1.0),
			SceneTreeTimer.SignalName.Timeout
		);

		QueueFree();
	}
}
