using System;
using System.Linq;
using Godot;



public partial class MenuScreen : Control
{
	[Signal]
	public delegate void StartBtnEventHandler();
	[Signal]
	public delegate void SettingsBtnEventHandler();
	[Signal]
	public delegate void ScoreBoardBtnEventHandler();
	[Signal]
	public delegate void ExitBtnEventHandler();
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		ConfigFile config = new ConfigFile();

		var err = config.Load("res://settings.cfg"); // use user:// for runtime data
		if (err != Error.Ok)
		{
			GD.Print("No config found yet — creating new one");
		}
		
		bool isDone = (bool)config.GetValue("GameState", "leveltransferred", false);

		if (!isDone)
		{
			CopyDefaultLevels();

			// Mark as done so it only runs once
			config.SetValue("GameState", "leveltransferred", true);
			config.Save("user://settings.cfg");
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		
	}

	private void OnStartButtonPressed()
	{
		EmitSignalStartBtn();
	}

	private void OnSettingsButtonPressed()
	{
		EmitSignalSettingsBtn();
	}

	private void OnLeaderboardButtonPressed()
	{
		EmitSignalScoreBoardBtn();
	}

	private void OnExitButtonPressed()
	{
		EmitSignalExitBtn();
	}
	
	public void CopyDefaultLevels()
	{
		string sourcePath = "res://Levels";
		string targetPath = "user://Levels";

		// Create user folder if missing
		if (!DirAccess.DirExistsAbsolute(targetPath))
			DirAccess.MakeDirRecursiveAbsolute(targetPath);

		using var dir = DirAccess.Open(sourcePath);
		if (dir == null)
		{
			GD.PrintErr("Could not open: " + sourcePath);
			return;
		}

		dir.ListDirBegin();

		while (true)
		{
			string file = dir.GetNext();
			if (file == "") break;
			if (dir.CurrentIsDir()) continue;

			string from = $"{sourcePath}/{file}";
			string to = $"{targetPath}/{file}";

			// Only copy if not already present
			if (!FileAccess.FileExists(to))
			{
				using var src = FileAccess.Open(from, FileAccess.ModeFlags.Read);
				using var dst = FileAccess.Open(to, FileAccess.ModeFlags.Write);

				dst.StoreBuffer(src.GetBuffer((long)src.GetLength()));

				GD.Print($"Copied: {file}");
			}
		}

		dir.ListDirEnd();
	}
	
}
