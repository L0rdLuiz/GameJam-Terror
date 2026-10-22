using Godot;
using System;

public partial class Aviso : CanvasLayer
{
	private Label label;
	private PanelContainer panelContainer;
	GameState gameState;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		gameState = GetNode<GameState>("/root/GameState");
		gameState.AvisoPedido += OnAvisoPedido;
		label = GetNode<Label>("PanelContainer/Label");
		panelContainer = GetNode<PanelContainer>("PanelContainer");
	}

	public override void _ExitTree()
	{
		gameState.AvisoPedido -= OnAvisoPedido;
	}

	private void OnAvisoPedido(string texto)
	{
		label.Text = texto;
		GD.Print("Aviso recebido: " + texto);
	}

}
