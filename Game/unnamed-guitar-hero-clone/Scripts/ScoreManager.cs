using Godot;
using System;

public partial class ScoreManager : Node
{
	double multiplier = 1.0;
	int score;
	Label scoreLabel;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		score = 0;
		scoreLabel = GetNode<Label>("Score");
		scoreLabel.Text = score.ToString();
	}

	public void BlockHit()
	{
		AddScore(100);
		multiplier += 0.1;
		scoreLabel.Text = score.ToString();
	}

	public void AddScore(int addScore)
	{
		double scoreToAdd =  addScore * multiplier;
		score += Convert.ToInt32(scoreToAdd);
	}

	public void BlockMissed()
	{
		multiplier =  1.0;
	}
	
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
