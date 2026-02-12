using Godot;
using System;
using System.Globalization;

public partial class ScoreManager : Node
{
	double multiplier = 1.0;
	int score;
	private Label scoreLabel;
	private Label MultiplierLabel;
	public int mistakes;

	public int Score
	{
		get => score;
		set => score = value;
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Score = 0;
		scoreLabel = GetNode<Label>("Score");
		MultiplierLabel = GetNode<Label>("Multiplier");
		scoreLabel.Text = Score.ToString();
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
		Score += Convert.ToInt32(scoreToAdd);
		scoreLabel.Text = Score.ToString();
	}

	public void BlockMissed()
	{
		multiplier =  1.0;
		MultiplierLabel.Text = multiplier.ToString(CultureInfo.InvariantCulture);
		mistakes++;

	}
	
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
