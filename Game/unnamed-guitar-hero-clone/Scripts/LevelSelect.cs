using Godot;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using UnnamedGuitarHeroClone.Scripts;
using FileAccess = Godot.FileAccess;

public partial class LevelSelect : Control
{
	[Signal]
	public delegate void ExitBtnEventHandler();
	
	private Popup _popup;
	[Signal]
	public delegate void StartLevelEventHandler(string levelPath);
	public override void _Ready()
	{
		LoadLevels();
	}
	
	public override void _Process(double delta)
	{
	}
	public static string[] LoadResources(string path)
	{
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
			.Select(f => path + f)
			.ToArray();
		return files;
	}
	private void LoadLevels()
	{
		var levelBox = GetNode("%LevelBox");
		
		var filesToLoad = LoadResources("user://Levels/")
			.ToList();
		
		int existingCount = filesToLoad.Count();
		var rng = new Random();
		if (existingCount < 10)
		{
			for (int i = existingCount; i < 10; i++)
			{
				int levelNumber = i + 1;
				string number = levelNumber < 10 ? $"0{levelNumber}" : levelNumber.ToString();
				string path = $"user://Levels/Level {number}.json";

				if (!FileAccess.FileExists(path))
				{
					// Maak het bestand
					using var file = FileAccess.Open(path, FileAccess.ModeFlags.Write);
					if (file == null)
					{
						GD.PrintErr($"Failed to create file: {path}");
						continue;
					}

					file.StoreLine("level " + levelNumber);
					file.StoreLine(rng.Next(1, 5).ToString());
					file.StoreLine("dif1");
				}
			}

			filesToLoad = LoadResources("user://Levels/")
					.ToList();
		}

		// Buttons genereren
		foreach (var file in filesToLoad)
		{
			var packedScene = ResourceLoader.Load<PackedScene>("res://Scenes/LevelButton.tscn");
			var nodeScene = (LevelButton)packedScene.Instantiate();
			var variant = file.Split("/").Last();
			var levelName = variant.Replace(".json", "");
			nodeScene.Constructor(levelName, file);
			nodeScene.LevelButtonClicked += LevelSelected;
			levelBox.AddChild(nodeScene);
		}
	}

	private void LevelSelected(string levelPath)
	{
		_popup = GetNode<Popup>("Popup");
		_popup.LevelPath = levelPath;
		var container = (GameContainer)GetParent();
		_popup.OpenSingleplayer += container.OpenSingleplayer;
		_popup.OpenMultiplayer += container.OpenMultiplayer;
		_popup.PopupCentered();
		
		
	}

	private void OnExitBtnPressed()
	{
		EmitSignalExitBtn();
	}
	private void OnMenuBtnClicked()
	{
		var parent = (GameContainer)GetParent();
		parent.EndGame();
	}
}
