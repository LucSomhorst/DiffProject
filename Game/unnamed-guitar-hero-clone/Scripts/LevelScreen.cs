using Godot;
using System;
using System.Collections.Generic;

public partial class LevelScreen : Control
{
	[Export]
	public PackedScene BlockScene { get; set; }
	[Signal]
	public delegate void EndGameEventHandler();
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		
	}
	
	public void Contructor(string levelPath)
	{
		List<string> values = LoadFile(levelPath);
		var label = GetNode<Label>("LevelName");
		label.Text = values[0];
		var timer = GetNode<Timer>("BlockTimer");
		timer.WaitTime = Convert.ToSingle(values[1]);
		
		NewGame();
	}
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		
	}
	public List<string> LoadFile(string file)
	{
		using var f = FileAccess.Open(file, FileAccess.ModeFlags.Read);
		int index = 1;
		List<string> values = new List<string>();
		while (!f.EofReached())
		{
			string line = f.GetLine() + " ";
			values.Add(line);
			index++;
		}

		return values;
	}
	public void StopGame()
	{
		GetNode<Timer>("BlockTimer").Stop();
		EmitSignalEndGame();
	}
	
	public void NewGame()
	{
		GetNode<Timer>("StartTimer").Start();
	}
	private void OnStartTimerTimeout()
	{
		GetNode<Timer>("BlockTimer").Start();
	}
	private void OnBlockTimerTimeout()
	{
		Block block = BlockScene.Instantiate<Block>();
		
		var blockSpawnLocation = GetNode<PathFollow2D>("BlockSpawn/BlockSpawnLocation");
		
		block.Position = blockSpawnLocation.Position;
		
		AddChild(block);
	}
}
