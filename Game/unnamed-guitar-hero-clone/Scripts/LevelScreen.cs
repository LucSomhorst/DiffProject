using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class LevelScreen : Control
{
	[Export] public PackedScene TapBlockScene { get; set; }
	[Export] public PackedScene HoldBlockScene { get; set; }

	[Export] public Hitzone Hitzone1 {get; set; }
	[Export] public Hitzone Hitzone2 {get; set; }
	[Export] public Hitzone Hitzone3 {get; set; }
	[Export] public Hitzone Hitzone4 {get; set; }

	[Signal]
	public delegate void EndGameEventHandler();
	private int blocksSent = 0;
	private int blocksToLevel = 10;
	private int level = 1;
	private bool patternActive;
	private bool followPattern;
	private List<string> patternToRun = new();
	private Timer blockTimer;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		ConfigFile ConfigLocal = new ConfigFile();
		ConfigLocal.Load("res://settings.cfg");
		if (Hitzone1 != null && Hitzone2 != null && Hitzone3 != null && Hitzone4 != null)
		{
			Hitzone1.SetKey(ConfigLocal.GetValue("Keybinds", "Player1Hitzone1").ToString());
			Hitzone2.SetKey(ConfigLocal.GetValue("Keybinds", "Player1Hitzone2").ToString());
			Hitzone3.SetKey(ConfigLocal.GetValue("Keybinds", "Player1Hitzone3").ToString());
			Hitzone4.SetKey(ConfigLocal.GetValue("Keybinds", "Player1Hitzone4").ToString());
			Hitzone1.SetButton(ConfigLocal.GetValue("Keybinds", "Player1Controller1").ToString());
			Hitzone2.SetButton(ConfigLocal.GetValue("Keybinds", "Player1Controller2").ToString());
			Hitzone3.SetButton(ConfigLocal.GetValue("Keybinds", "Player1Controller3").ToString());
			Hitzone4.SetButton(ConfigLocal.GetValue("Keybinds", "Player1Controller4").ToString());
		}
		else
		{
			GD.Print("cant load");
		}
		base._Ready();
	}
	
	public void Constructor(string levelPath)
	{
		List<string> values = LoadFile(levelPath);
		var label = GetNode<Label>("LevelName");
		label.Text = values[0];
		blockTimer = GetNode<Timer>("BlockTimer");
		blockTimer.WaitTime = Convert.ToSingle(values[1]);
		if (values[2] != null && values[2] != " ")
		{
			string[] patterns = values[2].Split(",", StringSplitOptions.TrimEntries);
			LoadPatterns(patterns);
		}
		if (values[3] != null && values[3] != " ")
		{
			followPattern = Convert.ToBoolean(values[3]);
		}
		NewGame();
	}
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		
	}
	
	public List<string> LoadFile(string file)
	{
		using var f = FileAccess.Open(file, FileAccess.ModeFlags.Read);
		List<string> values = new List<string>();
		while (!f.EofReached())
		{
			string line = f.GetLine() + " ";
			values.Add(line);
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
	}

	private string[] GetPatterns()
	{
		string path = "Patterns/";
			DirAccess dir_access = DirAccess.Open(path);
			if (dir_access == null)
			{
			
				// Directory doesn't exist -> create it
				var result = DirAccess.MakeDirRecursiveAbsolute(path);

				if (result != Error.Ok)
				{
					GD.PrintErr($"Failed to create directory: {path}, Error: {result}");
				}
				else
				{
					dir_access = DirAccess.Open(path);
				}

			}

			if (dir_access.GetFiles() == null) { return null; }
			string[] files = dir_access.GetFiles()
				.Select(f => path + f.Replace(".txt", ""))
				.ToArray();
			return files;
		
	}
	
	private void SpawnBlock()
	{
		int spawnIndex;
		BlockBase patternBlock;
		if (!patternActive && followPattern)
		{
			var patterns = GetPatterns();
			int nrOfPatterns =  patterns.Length;
			RandomNumberGenerator rng =  new RandomNumberGenerator();
			var variant = patterns[rng.RandiRange(0, nrOfPatterns - 1)];
			var patternToLoad = variant.Split("/").Last();
			LoadPattern(patternToLoad);
		}
		
		if (patternActive)
		{
			//load in pattern
			var pattern =  patternToRun.First();
			var parameters = pattern.Split("-");
			
			//get block type and spawn location
			spawnIndex = DetermineSpawnLocation(parameters[0]);
			patternBlock = DetermineBlockType(parameters[1]);
			
			//set if there are more patterns  to execute
			patternToRun.RemoveAt(0);
			if (patternToRun.Count <= 0)
			{
				patternActive = false;
			}
		}
		else
		{
			//get random block type and spawn location
			patternBlock = DetermineBlockType("random");
			spawnIndex = DetermineSpawnLocation("5");
		}
		
		//set block spawn location and spawn block
		var spawnPath = $"BlockSpawn/BlockSpawnLocation{spawnIndex}";
		var spawnLocation = GetNode<PathFollow2D>(spawnPath);
		patternBlock.Position = spawnLocation.Position;
		AddChild(patternBlock);
	}

	private int DetermineSpawnLocation(string location)
	{
		if (location.ToInt() >= 0 && location.ToInt() <= 4)
		{
			return location.ToInt();
		}

		if (location.ToInt() == 5)
		{
			RandomNumberGenerator rng  = new RandomNumberGenerator();
			return rng.RandiRange(1, 4);
		}

		GD.Print("Unknown spawn location");
		return 1;
	}

	private BlockBase DetermineBlockType(string blockType)
	{
		
		switch (blockType)
		{
			case "hold":
				return HoldBlockScene.Instantiate<HoldBlock>();
			
			case "block":
				return TapBlockScene.Instantiate<TapBlock>();
			
			case "random":
				RandomNumberGenerator rng = new RandomNumberGenerator();
				if (rng.RandiRange(0, 1) == 0){
					return TapBlockScene.Instantiate<TapBlock>();
				}
				return HoldBlockScene.Instantiate<HoldBlock>();
			
			default:
				return TapBlockScene.Instantiate<TapBlock>();
			
		}
	}

	private void LoadPatterns(string[] patterns)
	{
		//Load all patterns from the level into the pattern lis
		foreach (var pattern in patterns)
		{
			LoadPattern(pattern);
		}
	}
	
	private void LoadPattern(string patternName)
	{
		GD.Print(patternName);
		RandomNumberGenerator rng = new RandomNumberGenerator();
		var rngNumber = rng.RandiRange(1,4);
		var file = "res://Patterns/" + patternName + ".txt";
		using var f = FileAccess.Open(file, FileAccess.ModeFlags.Read);
		var pattern = f.GetAsText().Split(",");
		//Check if all parameters are filled, else randomize value
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
				parameters[1] = rng.RandiRange(1,2) > 1 ? "block" : "hold";
			}

			// Write the updated string back
			pattern[i] = $"{parameters[0]}-{parameters[1]}";
		}
		// Add the patterns to the end of the pattern list 
		foreach (var variant in  pattern)
		{
			patternToRun.Add(variant);
			GD.Print($"Loading {variant}");
		}
		patternActive = true;
	}
	

	private void SetBlockSpeed(double speed)
	{
		blockTimer.WaitTime = speed;
	}
}
