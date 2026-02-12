using Godot;
using System.Collections.Generic;
using System.Text.Json;

public partial class SaveManager : Node
{
	const string FolderPath = "user://Scores";
	const string FilePath = "user://Scores/scores.json";

	class ScoreEntry
	{
		public string name { get; set; }
		public string levelName { get; set; }
		public int score { get; set; }
		public double time { get; set; }
	}

	// Ensure folder exists
	void EnsureFolder()
	{
		var dir = DirAccess.Open("user://");
		if (!dir.DirExists("Scores"))
			dir.MakeDir("Scores");
	}

	// Load scores
	List<ScoreEntry> LoadScores()
	{
		if (!FileAccess.FileExists(FilePath))
			return new List<ScoreEntry>();

		using var file = FileAccess.Open(FilePath, FileAccess.ModeFlags.Read);
		var json = file.GetAsText();

		try
		{
			return JsonSerializer.Deserialize<List<ScoreEntry>>(json) 
				   ?? new List<ScoreEntry>();
		}
		catch
		{
			return new List<ScoreEntry>();
		}
	}

	// Save + sort + limit
	public void SaveScore(string playerName,string levelName ,int score, double time)
	{
		EnsureFolder();

		var scores = LoadScores();

		scores.Add(new ScoreEntry
		{
			name = playerName,
			levelName  = levelName,
			score = score,
			time  = time
			
		});

		// Sort highest first
		scores.Sort((a, b) => b.score.CompareTo(a.score));

		// Keep top 20
		if (scores.Count > 20)
			scores = scores.GetRange(0, 20);

		var json = JsonSerializer.Serialize(scores, new JsonSerializerOptions
		{
			WriteIndented = true
		});

		using var file = FileAccess.Open(FilePath, FileAccess.ModeFlags.Write);
		file.StoreString(json);
	}

	// Optional getter for UI
	public List<(string name, int score)> GetHighscores()
	{
		var list = new List<(string, int)>();
		foreach (var s in LoadScores())
			list.Add((s.name, s.score));
		return list;
	}
}
