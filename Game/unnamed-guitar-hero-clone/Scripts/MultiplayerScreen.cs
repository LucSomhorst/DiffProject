using Godot;
using System;
using System.Collections.Generic;

public partial class MultiplayerScreen : Control
{
	[Signal]
	public delegate void EndGameEventHandler();
	public string LevelFile { get; set; }
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		CallDeferred(nameof(Contructor));
	}
	public void Constructor()
	{
		//List<string> values = LoadFile(LevelFile);
		var level1 = GetNode<MultiplayerLevel>("MultiplayerLevel");
		level1.Constructor();
		var level2 = GetNode<MultiplayerLevel>("MultiplayerLevel2");
		level2.Constructor();
		level2.ChangeKeyBindForMP();
		level2.ChangeLabelPostitionForMP();
		
		//NewGame();
	}
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
