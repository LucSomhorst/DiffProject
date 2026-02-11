using Godot;
using System;
using System.Collections.Generic;
using System.Text.Json;
using Godot.Collections;

public partial class ScoreboardScreen : MarginContainer
{
	[Signal]
	public delegate void ReturnBtnEventHandler();
	
	[Export]
	public VBoxContainer scoreboardContainer;

	[Export] public PackedScene LeaderBoardLine;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		LoadMissions();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		
	}

	private void OnReturnBtnPressed()
	{
		EmitSignalReturnBtn();
	}
	public List<LeaderBoardDataLine> LeaderboardEntries = new List<LeaderBoardDataLine>();
	public class LeaderBoardDataLine
	{
		public string name { get; set; }
		public string levelName { get; set; }
		public int score { get; set; }
		public int time { get; set; }
	}
	public void LoadMissions()
	{
		var filePath = "user://Scores/scores.json";

		// Open file safely
		if (!FileAccess.FileExists(filePath))
		{
			GD.PrintErr("File not found: " + filePath);
			return;
		}

		using var file = FileAccess.Open(filePath, FileAccess.ModeFlags.Read);
		string jsonText = file.GetAsText();
		file.Close();

		try
		{
			// Deserialize JSON into objects
			LeaderboardEntries = JsonSerializer.Deserialize<List<LeaderBoardDataLine>>(jsonText);

			GD.Print("Loaded " + LeaderboardEntries.Count + " missions!");

			// For each entry, create a visual node and add as child
			foreach (var entry in LeaderboardEntries)
			{
				// Load your HighscoreEntry scene
				var entryScene = LeaderBoardLine;
				var entryNode = entryScene.Instantiate<LeaderBoardLine>();
				GD.Print(entry.name, entry.levelName, entry.score, entry.time);
				// Setup the visual node using data
				entryNode.Setup(entry.name, entry.levelName, entry.score, entry.time);
				
				// Add it to the container
				scoreboardContainer.AddChild(entryNode);
			}
		 }
		 catch (Exception e)
		 {
			GD.PrintErr("Failed to parse JSON: " + e.Message);
		 }
	}
}
