using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class MultiplayerLevel : Control
{
	private ConfigFile ConfigLocal = new();
	[Export] public PackedScene TapBlockScene { get; set; }
	[Export] public PackedScene HoldBlockScene { get; set; }
	[Export] public Hitzone Hitzone1 {get; set; }
	[Export] public Hitzone Hitzone2 {get; set; }
	[Export] public Hitzone Hitzone3 {get; set; }
	[Export] public Hitzone Hitzone4 {get; set; }
	private int blocksSent = 0;
	private int blocksToLevel = 10;
	private int level = 1;
	private bool patternActive = false;
	private List<string> patternToRun = new();
	private Timer blockTimer;
	[Signal]
	public delegate void EndGameEventHandler();
	public string LevelFile { get; set; }
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		ConfigLocal.Load("res://settings.cfg");
	}
	
	public void Constructor(string levelPath)
	{
		GD.Print(levelPath);
		List<string> values = LoadFile(levelPath);
		//var label = GetNode<Label>("LevelName");
		//label.Text = values[0];
		blockTimer = GetNode<Timer>("BlockTimer");
		blockTimer.WaitTime = Convert.ToSingle(values[1]);
		if (values[2] != null && values[2] != " ")
		{
			GD.Print(values[2]);
			string[] patterns = values[2].Split(",", StringSplitOptions.TrimEntries);
			LoadPatterns(patterns);
		}
		NewGame();
	}
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		
	}
	public void ChangeKeyBindForMP()
	{
		ConfigFile ConfigLocal = new ConfigFile();
		ConfigLocal.Load("res://settings.cfg");
		Hitzone1.SetKey(ConfigLocal.GetValue("Keybinds", "Player2Hitzone1").ToString());
		Hitzone2.SetKey(ConfigLocal.GetValue("Keybinds", "Player2Hitzone2").ToString());
		Hitzone3.SetKey(ConfigLocal.GetValue("Keybinds", "Player2Hitzone3").ToString());
		Hitzone4.SetKey(ConfigLocal.GetValue("Keybinds", "Player2Hitzone4").ToString());
		Hitzone1.SetButton(ConfigLocal.GetValue("Keybinds", "Player2Controller1").ToString());
		Hitzone2.SetButton(ConfigLocal.GetValue("Keybinds", "Player2Controller2").ToString());
		Hitzone3.SetButton(ConfigLocal.GetValue("Keybinds", "Player2Controller3").ToString());
		Hitzone4.SetButton(ConfigLocal.GetValue("Keybinds", "Player2Controller4").ToString());
		base._Ready();
	}
	public void ChangeLabelPostitionForMP()
	{
		
		var label = GetNode<Label>("ScoreManager/Score");
		var multiplier = GetNode<Label>("ScoreManager/Multiplier");
		label.Position = new Vector2(850,0);
		multiplier.Position = new Vector2(850, 100);
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
		// //tapBlock en holdBlock voor testen specifiek blok
		// TapBlock tapBlock = TapBlockScene.Instantiate<TapBlock>();
		// HoldBlock holdBlock = HoldBlockScene.Instantiate<HoldBlock>();
		SpawnBlock();
		blocksSent++;
		if (blocksToLevel <= blocksSent)
		{
			var nextLevel = Convert.ToDouble(blocksToLevel) * 1.5;
			blocksToLevel = Convert.ToInt32(nextLevel) ;
			level++;
			blockTimer.WaitTime *= 0.9f;
		}    
		GD.Print(blocksSent + " " + level);
	}
	
	private void SpawnBlock()
	{
		if (patternActive)
		{
			BlockBase patternBlock;
			int spawnIndex;
			var pattern =  patternToRun.First();
			var parameters = pattern.Split("-");
			if (parameters[0].ToInt() >= 0)
			{
				spawnIndex = parameters[0].ToInt();
			}
			else
			{
				RandomNumberGenerator rng = new RandomNumberGenerator();
				spawnIndex = rng.RandiRange(1, 4);
			}

			switch (parameters[1])
			{
				case "hold":
					patternBlock = HoldBlockScene.Instantiate<HoldBlock>();
						break;
				case "block":
					patternBlock = TapBlockScene.Instantiate<TapBlock>();
					break;
				default:
					patternBlock = TapBlockScene.Instantiate<TapBlock>();
					break;
			}
			
			var spawnPath = $"BlockSpawn/BlockSpawnLocation{spawnIndex}";
			var spawnLocation = GetNode<PathFollow2D>(spawnPath);
		
			patternBlock.Position = spawnLocation.Position;
			AddChild(patternBlock);
			patternToRun.RemoveAt(0);
			if (patternToRun.Count <= 0)
			{
				patternActive = false;
			}
			GD.Print(patternToRun.Count);
		}
		else
		{
			// block die gerandomized wordt
			BlockBase randomBlock;
		
			// 50% kans hold / tap
			RandomNumberGenerator rng = new RandomNumberGenerator();
			if (rng.RandiRange(0, 1) == 0){
				randomBlock = TapBlockScene.Instantiate<TapBlock>();
			}
			else{
				randomBlock = HoldBlockScene.Instantiate<HoldBlock>();
			}
		
			// Random spawnpoint tussen 1 en 4
			int spawnIndex = rng.RandiRange(1, 4);
			var spawnPath = $"BlockSpawn/BlockSpawnLocation{spawnIndex}";
			var spawnLocation = GetNode<PathFollow2D>(spawnPath);
		
			randomBlock.Position = spawnLocation.Position;
		
			AddChild(randomBlock);
		}
	}

	private void LoadPatterns(string[] patterns)
	{
		foreach (var pattern in patterns)
		{
			LoadPattern(pattern);
		}
	}
	
	private void LoadPattern(string patternName)
	{
		RandomNumberGenerator rng = new RandomNumberGenerator();
		var rngNumber = rng.RandiRange(1,4);
		var file = "res://Patterns/" + patternName + ".txt";
		using var f = FileAccess.Open(file, FileAccess.ModeFlags.Read);
		var pattern = f.GetAsText().Split(",");
		for (int i = 0; i < pattern.Length; i++)
		{
			var block = pattern[i];
			var parameters = block.Split("-");

			if (parameters[0] == "")
			{
				parameters[0] = rngNumber.ToString();
			}

			if (parameters[1] == "")
			{
				parameters[1] = rngNumber > 2 ? "block" : "hold";
			}

			// Write the updated string back
			pattern[i] = $"{parameters[0]}-{parameters[1]}";
			GD.Print(pattern[i]);
		}
		foreach (var variant in  pattern)
		{
			patternToRun.Add(variant);
		}
		patternActive = true;
	}
	

	private void SetBlockSpeed(double speed)
	{
		blockTimer.WaitTime = speed;
	}
}
