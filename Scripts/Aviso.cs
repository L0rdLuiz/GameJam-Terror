
using Godot;
using System;

public partial class Aviso : CanvasLayer
{
    private Label label;
    private PanelContainer panelContainer;
    private GameState gameState;
	private Tween tweenAviso;

    public override void _Ready()
    {
        gameState = GetNode<GameState>("/root/GameState");
        label = GetNode<Label>("PanelContainer/Label");
        panelContainer = GetNode<PanelContainer>("PanelContainer");
        gameState.AvisoPedido += OnAvisoPedido;
    }

    public override void _ExitTree()
    {
        if (gameState != null)
        gameState.AvisoPedido -= OnAvisoPedido;
    }

    private async void OnAvisoPedido(string texto)
	{
		label.Text = texto;
		tweenAviso?.Kill();
		panelContainer.Modulate = Colors.White;
		label.Modulate = Colors.White;

		panelContainer.Position = new Vector2(
			panelContainer.Position.X,
			panelContainer.Position.Y - 30
		);

		panelContainer.Modulate = new Color(1, 1, 1, 0);
		tweenAviso = CreateTween();

		tweenAviso.SetParallel(true);
		tweenAviso.TweenProperty(
			panelContainer,
			"position:y",
			panelContainer.Position.Y + 30,
			0.4
		);

		tweenAviso.TweenProperty(
			panelContainer,
			"modulate:a",
			1.0,
			0.4
		);

		await ToSignal(tweenAviso, Tween.SignalName.Finished);

		await ToSignal(GetTree().CreateTimer(2.0), SceneTreeTimer.SignalName.Timeout);

		tweenAviso = CreateTween();
		tweenAviso.SetParallel(true);

		tweenAviso.TweenProperty(
			panelContainer,
			"position:y",
			panelContainer.Position.Y - 20,
			0.4
		);

		tweenAviso.TweenProperty(
			panelContainer,
			"modulate:a",
			0.0,
			0.4
		);

		await ToSignal(tweenAviso, Tween.SignalName.Finished);
	}
}
