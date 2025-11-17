using Godot;
using System;

public partial class ScoreManager : Node
{
		
	int score;
	Label scoreLabel;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		score = 0;
		scoreLabel = GetNode<Label>("Score");
		scoreLabel.Text = score.ToString();
	}

	public void AddPoint()
	{
		score += 100;
		scoreLabel.Text = score.ToString();
	}
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
