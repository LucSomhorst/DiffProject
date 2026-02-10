using Godot;
using System;
using System.Globalization;

public partial class ScoreManager : Node
{
	double multiplier = 1.0;
	int score;
	private Label scoreLabel;
	private Label MultiplierLabel;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		score = 0;
		scoreLabel = GetNode<Label>("Score");
		MultiplierLabel = GetNode<Label>("Multiplier");
		scoreLabel.Text = score.ToString();
	}

	public void BlockHit()
	{
		AddScore(100);
		multiplier += 0.1;
		MultiplierLabel.Text = multiplier.ToString();
	}

	public void LongBlockHit()
	{
		AddScore(300);
		multiplier += 0.1;
		MultiplierLabel.Text = multiplier.ToString();
	}

	public void AddScore(int addScore)
	{
		double scoreToAdd =  addScore * multiplier;
		score += Convert.ToInt32(scoreToAdd);
		scoreLabel.Text = score.ToString();
	}

	public void BlockMissed()
	{
		multiplier =  1.0;
		MultiplierLabel.Text = multiplier.ToString(CultureInfo.InvariantCulture);
	}
	
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
