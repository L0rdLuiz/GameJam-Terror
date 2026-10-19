using Godot;

public partial class Pagina2 : Sprite2D
{

	GameState gameState;

	public override void _Ready()
	{
		GetNode<Button>("Confirmar").Pressed += async () =>
		{
			int.TryParse(GetNode<LineEdit>("resposta").Text, out int r1);
			int.TryParse(GetNode<LineEdit>("resposta2").Text, out int r2);
			int.TryParse(GetNode<LineEdit>("resposta3").Text, out int r3);

			if (r1 == 3 && r2 == 9 && r3 == 2)
			{
				GD.Print("ACERTOU");
				gameState = GetNode<GameState>("/root/GameState");
				gameState.ConcluirTarefa(GameState.TarefaEscolar);
				GetNode<Sprite2D>("../LicaoConcluida").Show();
				await ToSignal(GetTree().CreateTimer(2.0), SceneTreeTimer.SignalName.Timeout);
				GetParent().QueueFree();
			}
			else
			{
				GD.Print("ERRADO");
			}

		};

		
	}
}