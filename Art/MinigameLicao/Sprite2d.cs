using Godot;

public partial class Sprite2d : Sprite2D
{
	public override void _Ready()
	{
		GetNode<Button>("Confirmar").Pressed += () =>
		{
			string r1 = GetNode<LineEdit>("resposta").Text;
			string r2 = GetNode<LineEdit>("resposta2").Text;
			string r3 = GetNode<LineEdit>("resposta3").Text;

			if (r1 == "4" && r2 == "8" && r3 == "5")
			{
				Hide();
				GetNode<Sprite2D>("../Pagina2").Show();
			}
			else
			{
				GD.Print("ERRADO");
			}
		};
	}
}