using Godot;
using System;

public partial class GameOverPopup : Control
{
	[Signal]
	public delegate void GameOverEventHandler(string name, int score, double time);
	
	[Export]
	public Label ScoreLabel;
	[Export]
	public Label TimeLabel;
	[Export] 
	public LineEdit NameBox;
	public int score = 0;
	public double time = 0;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void OnButtonPressed()
	{
		EmitSignalGameOver(NameBox.Text, score, time);
	}
}
