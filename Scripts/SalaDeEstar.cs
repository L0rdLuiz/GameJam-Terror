using Godot;
using System;

public partial class SalaDeEstar : Node2D
{
	public override void _Ready()
{
	Button cavalete = GetNode<Button>("Cavalete");
	Label texto = GetNode<Label>("Cavalete/TextoPintar");
	TelaPintura telaPintura = GetNode<TelaPintura>("TelaPintura");

	telaPintura.Hide();

	cavalete.MouseEntered += () => texto.Show();
	cavalete.MouseExited += () => texto.Hide();
	cavalete.Pressed += () => telaPintura.Show();

	

}
	public override void _Process(double delta)
	{
	}
}


