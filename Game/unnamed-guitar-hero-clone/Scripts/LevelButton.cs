using Godot;
using System;

public partial class LevelButton : MarginContainer
{
	[Signal]
	public delegate void LevelButtonClickedEventHandler(string path);

	private string levelPath;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void Constructor(string name, string path)
	{
		levelPath = path;
		var button = (Button)GetNode("%PlayLevelButton");
		button.Text = name;
	}

	private void OnLevelButtonClicked()
	{
		EmitSignalLevelButtonClicked(levelPath);
		GD.Print("Level Button clicked");
	}
}
