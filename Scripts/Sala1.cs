using Godot;
using System;

public partial class Sala1 : Node2D
{
	GameState gameState;
	string[] falasEspelho =
{
	"Good morning, Cherry",
	"All right... what do we have planned for today?",
	"Let's see... lunch, getting Sugarplum ready for school, folding the laundry...",
	"His first day of school. I wonder if he's excited.",
	"He's growing up so fast.",
	"Well, I should get started. No time to waste!"
};
int falaAtual = 0;

	public override void _Ready()
	{
		gameState = GetNode<GameState>("/root/GameState");
		gameState.DiaMudou += AoDiaMudar;

		// estado inicial: começa no menu
		GetNode<Sprite2D>("Menu").Show();
		GetNode<Button>("SairEspelho").Hide();
		GetNode<Sprite2D>("Sprite2D").Hide();
		GetNode<Sprite2D>("Fundo2").Hide();
		GetNode<Node2D>("SalaDeEstar").Hide();
		GetNode<Sprite2D>("FundoEspelho").Hide();
		GetNode<Button>("Comecar").Show();

		GetNode<Button>("Botao").Hide();
		GetNode<Button>("Salaestar").Hide();
		GetNode<Button>("BotaoVoltar").Hide();
		GetNode<Button>("TarefaEscolar").Hide();
		GetNode<Button>("VoltarSalaQuarto").Hide();
		GetNode<Button>("Espelho").Hide();
		GetNode<CanvasLayer>("Dialogo").Hide();

		// Fechar a caixa de diálogo ao clicar nela
		GetNode<Button>("Dialogo/Caixa/Avancar").Pressed += () =>
		{
		falaAtual++;

		if (falaAtual < falasEspelho.Length)
		MostrarFala(falasEspelho[falaAtual]);
		else
		GetNode<CanvasLayer>("Dialogo").Hide();
		};
		// Comecar: menu -> quarto
		GetNode<Button>("Comecar").Pressed += () =>
		{
			GetNode<Sprite2D>("Menu").Hide();
			GetNode<Button>("Comecar").Hide();
			GetNode<Sprite2D>("Sprite2D").Show();
			GetNode<Sprite2D>("Fundo2").Hide();
			GetNode<Node2D>("SalaDeEstar").Hide();
			GetNode<Sprite2D>("FundoEspelho").Hide();

			GetNode<Button>("Botao").Show();
			GetNode<Button>("Salaestar").Show();
			GetNode<Button>("BotaoVoltar").Hide();
			GetNode<Button>("TarefaEscolar").Hide();
			GetNode<Button>("VoltarSalaQuarto").Hide();
			GetNode<Button>("Espelho").Hide();
		};

		// Botao: quarto -> banheiro
		GetNode<Button>("Botao").Pressed += () =>
		{
			GetNode<Button>("Comecar").Hide();
			GetNode<Sprite2D>("FundoEspelho").Hide();
			GetNode<Sprite2D>("Sprite2D").Hide();
			GetNode<Node2D>("SalaDeEstar").Hide();
			GetNode<Sprite2D>("Fundo2").Show();

			GetNode<Button>("Botao").Hide();
			GetNode<Button>("Salaestar").Hide();
			GetNode<Button>("BotaoVoltar").Show();
			GetNode<Button>("TarefaEscolar").Hide();
			GetNode<Button>("VoltarSalaQuarto").Hide();
			GetNode<Button>("Espelho").Show();
			GetNode<Button>("NextDay").Hide();
			GetNode<Button>("SairEspelho").Hide();
		};

		// Espelho: banheiro -> espelho
		GetNode<Button>("Espelho").Pressed += () =>
		{
			GetNode<Button>("Comecar").Hide();
			GetNode<Node2D>("SalaDeEstar").Hide();
			GetNode<Sprite2D>("Fundo2").Hide();
			GetNode<Sprite2D>("FundoEspelho").Show();
			GetNode<Sprite2D>("Sprite2D").Hide();
			GetNode<Button>("VoltarSalaQuarto").Hide();
			GetNode<Button>("TarefaEscolar").Hide();
			GetNode<Button>("BotaoVoltar").Hide();
			GetNode<Button>("SairEspelho").Show();
			falaAtual = 0;
			MostrarFala(falasEspelho[0]);
		};

		// SairEspelho: espelho -> banheiro
		GetNode<Button>("SairEspelho").Pressed += () =>
		{
			GetNode<Button>("Comecar").Hide();
			GetNode<Sprite2D>("FundoEspelho").Hide();
			GetNode<Sprite2D>("Sprite2D").Hide();
			GetNode<Node2D>("SalaDeEstar").Hide();
			GetNode<Sprite2D>("Fundo2").Show();
			GetNode<Button>("SairEspelho").Hide();

			GetNode<Button>("Botao").Hide();
			GetNode<Button>("Salaestar").Hide();
			GetNode<Button>("BotaoVoltar").Show();
			GetNode<Button>("TarefaEscolar").Hide();
			GetNode<Button>("VoltarSalaQuarto").Hide();
			GetNode<Button>("Espelho").Show();
			GetNode<Button>("NextDay").Hide();
		};

		// BotaoVoltar: banheiro -> quarto
		GetNode<Button>("BotaoVoltar").Pressed += () =>
		{
			GetNode<Button>("Comecar").Hide();
			GetNode<Sprite2D>("FundoEspelho").Hide();
			GetNode<Sprite2D>("Fundo2").Hide();
			GetNode<Node2D>("SalaDeEstar").Hide();
			GetNode<Sprite2D>("Sprite2D").Show();

			GetNode<Button>("TarefaEscolar").Hide();
			GetNode<Button>("BotaoVoltar").Hide();
			GetNode<Button>("VoltarSalaQuarto").Hide();
			GetNode<Button>("Botao").Show();
			GetNode<Button>("Salaestar").Show();
			GetNode<Button>("Espelho").Hide();
			GetNode<Button>("NextDay").Show();
		};

		// Salaestar: quarto -> sala de estar
		GetNode<Button>("Salaestar").Pressed += () =>
		{
			GetNode<Button>("Comecar").Hide();
			GetNode<Sprite2D>("FundoEspelho").Hide();
			GetNode<Sprite2D>("Sprite2D").Hide();
			GetNode<Sprite2D>("Fundo2").Hide();
			GetNode<Node2D>("SalaDeEstar").Show();

			GetNode<Button>("Salaestar").Hide();
			GetNode<Button>("Botao").Hide();
			GetNode<Button>("BotaoVoltar").Hide();
			GetNode<Button>("TarefaEscolar").Show();
			GetNode<Button>("VoltarSalaQuarto").Show();
			GetNode<Button>("Espelho").Hide();
			GetNode<Button>("NextDay").Hide();
		};

		// VoltarSalaQuarto: sala de estar -> quarto
		GetNode<Button>("VoltarSalaQuarto").Pressed += () =>
		{
			GetNode<Button>("Comecar").Hide();
			GetNode<Sprite2D>("FundoEspelho").Hide();
			GetNode<Node2D>("SalaDeEstar").Hide();
			GetNode<Sprite2D>("Fundo2").Hide();
			GetNode<Sprite2D>("Sprite2D").Show();

			GetNode<Button>("VoltarSalaQuarto").Hide();
			GetNode<Button>("TarefaEscolar").Hide();
			GetNode<Button>("BotaoVoltar").Hide();
			GetNode<Button>("Botao").Show();
			GetNode<Button>("Salaestar").Show();
			GetNode<Button>("Espelho").Hide();
			GetNode<Button>("NextDay").Show();
		};

		// Texto ao passar o mouse: Espelho
		Button espelho = GetNode<Button>("Espelho");
		Label textoEspelho = GetNode<Label>("Espelho/TextoEspelho");
		textoEspelho.Hide();
		espelho.MouseEntered += () => textoEspelho.Show();
		espelho.MouseExited += () => textoEspelho.Hide();

		// Texto ao passar o mouse: TarefaEscolar
		Button tarefaEscolar = GetNode<Button>("TarefaEscolar");
		Label textoTarefa = GetNode<Label>("TarefaEscolar/TextoTarefaEscolar");
		textoTarefa.Hide();
		tarefaEscolar.MouseEntered += () => textoTarefa.Show();
		tarefaEscolar.MouseExited += () => textoTarefa.Hide();

		// Próximo dia
		Button nextDay = GetNode<Button>("NextDay");
		Label textoNextDay = GetNode<Label>("NextDay/TextoNextDay");
		textoNextDay.Hide();
		nextDay.MouseEntered += () => textoNextDay.Show();
		nextDay.MouseExited += () => textoNextDay.Hide();
		nextDay.Pressed += OnButtonPressed;

		// Tarefa escolar
		GetNode<Button>("TarefaEscolar").Pressed += () =>
		{
			// if (!gameState.PossoFazerATarefa(GameState.TarefaEscolar))
			// {
			// 	GD.Print("Não pode fazer a tarefa escolar ainda!");
			// 	return;
			// }
			PackedScene tarefaEscolarScene = GD.Load<PackedScene>("res://Scenes/MinigameLicao/minigame_licao.tscn");
			Node tarefaEscolarInstance = tarefaEscolarScene.Instantiate();
			GetTree().Root.AddChild(tarefaEscolarInstance);
			Hide();
			tarefaEscolarInstance.TreeExited += () => Show();
		};
	}

	private void MostrarFala(string fala)
	{
		GetNode<Label>("Dialogo/Caixa/Texto").Text = fala;
		GetNode<CanvasLayer>("Dialogo").Show();
	}

	public override void _ExitTree()
	{
		if (gameState != null)
			gameState.DiaMudou -= AoDiaMudar;
	}

	private void OnButtonPressed()
	{
		gameState.NextDay();
	}

	private void AoDiaMudar(int dia)
	{
		GD.Print("Dia mudou para: " + dia);
	}

	public override void _UnhandledInput(InputEvent @event)
	{
		if (@event is InputEventMouseButton { Pressed: true })
			GD.Print("MOUSE CLICADO!");
	}
}