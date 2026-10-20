using Godot;

public partial class MinigameLicao : Node2D
{
	public override void _Ready()
	{
		// Página 1 -> Página 2
		GetNode<Button>("Pagina1/Confirmar").Pressed += () =>
		{
			int.TryParse(GetNode<LineEdit>("Pagina1/resposta").Text, out int r1);
			int.TryParse(GetNode<LineEdit>("Pagina1/resposta2").Text, out int r2);
			int.TryParse(GetNode<LineEdit>("Pagina1/resposta3").Text, out int r3);

			if (r1 == 4 && r2 == 8 && r3 == 5)
			{
				GetNode<Sprite2D>("Pagina1").Hide();
				GetNode<Sprite2D>("Pagina2").Show();
			}
			else
			{
				GD.Print("ERRADO (pagina 1)");
			}
		};

		// Página 2 -> Lição concluída
		GetNode<Button>("Pagina2/Confirmar").Pressed += () =>
		{
			int.TryParse(GetNode<LineEdit>("Pagina2/resposta").Text, out int r1);
			int.TryParse(GetNode<LineEdit>("Pagina2/resposta2").Text, out int r2);
			int.TryParse(GetNode<LineEdit>("Pagina2/resposta3").Text, out int r3);

			if (r1 == 3 && r2 == 9 && r3 == 2)
			{
				GetNode<Sprite2D>("LicaoConcluida").Show();
			}
			else
			{
				GD.Print("ERRADO (pagina 2)");
			}
		};
		GetNode<Button>("LicaoConcluida/Voltar").Pressed += () =>
		{
			GetNode<Sprite2D>("SalaDeEstar").Show();
			GetNode<Sprite2D>("MinigameLicao").Hide();
			GetNode<Sprite2D>("LicaoConcluida").Hide();
		};
	}
}