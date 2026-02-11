using Godot;
using System;
using System.Globalization;

public partial class LeaderBoardLine : Control
{
	[Export] public Label NameLabel;
	[Export] public Label ScoreLabel;
	[Export] public Label LevelNameLabel;
	[Export] public Label TimeLabel;

	public string name { get; set; }
	public string levelName { get; set; }
	public int score { get; set; }
	public double time { get; set; }

	public override void _Ready()
	{
	}

	public void Setup(string name, string levelName, int score, double time)
	{
		this.name = name;
		this.levelName = levelName;
		this.score = score;
		this.time = time;

		NameLabel.Text = name;
		LevelNameLabel.Text = levelName;
		ScoreLabel.Text = score.ToString();
		TimeLabel.Text = Math.Ceiling(time).ToString(CultureInfo.InvariantCulture); // rounds up
	}
}
