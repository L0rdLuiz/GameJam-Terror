using Godot;

public partial class Pagina2 : Sprite2D
{

	GameState gameState;

	public override void _Ready()
	{
		GetNode<Button>("Confirmar").Pressed += () =>
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
				
			}
			else
			{
				GD.Print("ERRADO");
			}
		};
	}
}