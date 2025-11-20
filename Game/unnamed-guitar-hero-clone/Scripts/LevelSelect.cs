using Godot;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Mime;
using UnnamedGuitarHeroClone.Scripts;
using FileAccess = Godot.FileAccess;

public partial class LevelSelect : Control
{
	[Signal]
	public delegate void ExitBtnEventHandler();
	
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
		if (dir_access == null) { return null; }

		string[] files = dir_access.GetFiles();
		if (files == null) { return null; }

		return files;
	}
	private void LoadLevels()
	{
		var levelBox = GetNode("%LevelBox");
		var filesToLoad = LoadResources("res://Levels/");
		var newfiles = FileAccess.Open("Users://Levels", FileAccess.ModeFlags.WriteRead);
		for (int i = filesToLoad.Length; i < 10; i++)
		{
			string number = "error";
			var levelnumber = i + 1;
			if (levelnumber < 10)
			{
				number = "0" + levelnumber;
			}
			else
			{
				number  = "" + levelnumber;
			}
			var file = FileAccess.Open("res://Levels/Level " + number + ".json", FileAccess.ModeFlags.Write);
			file.StoreLine(i.ToString());
			Random rnd = new Random();
			file.StoreLine(rnd.Next(1,5).ToString());
		}
		foreach (var file in filesToLoad)
		{
			var packedScene = ResourceLoader.Load<PackedScene>("res://Scenes/LevelButton.tscn");
			var NodeScene = (LevelButton)packedScene.Instantiate();
			
			NodeScene.Constructor(file.Replace(".json",""),file);
			NodeScene.LevelButtonClicked += LevelSelected;
			levelBox.AddChild(NodeScene);
		}
	}

	private void LevelSelected(string levelPath)
	{
		EmitSignalStartLevel(levelPath);
		GD.Print("Level Selected");
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
