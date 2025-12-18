using Godot;
using System;

public partial class Popup : Godot.Popup
{
	[Signal]
	public delegate void OpenSingleplayerEventHandler(string levelPath);
	
	[Signal]
	public delegate void OpenMultiplayerEventHandler(string levelPath);

	public string LevelPath { get; set; }
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void OnSingleplayerPressed()
	{
		EmitSignalOpenSingleplayer(LevelPath);
	}
	
	private void OnMultiplayerPressed()
	{
		EmitSignalOpenMultiplayer(LevelPath);
	}
}
