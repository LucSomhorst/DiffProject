using Godot;
using System;
using System.Diagnostics;
using System.Text.Json;
using System.Collections.Generic;
using System.Linq;

public partial class BpmAnalyzer : Control
{
	// ===== CONFIG =====
	private const string PythonExe = @"C:\Python313\python.exe"; // Full path to Python with librosa
	private const string ScriptPath = "res://External/GetBPM.py"; // Python BPM script

	// ===== NODES =====
	[Export] private FileDialog fileDialog;
	[Export] private Label bpmLabel;
	[Export] private Button selectButton;

	[Signal]
	public delegate void SongUploadedEventHandler(string path);

	public override void _Ready()
	{
		if (selectButton != null)
			selectButton.Pressed += OnSelectButtonPressed;

		if (fileDialog != null)
			fileDialog.FileSelected += OnFileSelected;
	}

	private void OnSelectButtonPressed()
	{
		fileDialog.PopupCentered();
	}

	private void OnFileSelected(string path)
	{
		bpmLabel.Text = "Analyzing...";
		var systemPath = ProjectSettings.GlobalizePath(path);
		EmitSignalSongUploaded(systemPath);
		RunBpmAnalysis(systemPath);
	}

	// ===== RUN PYTHON BPM ANALYSIS =====
	// ===== RUN PYTHON BPM / BEAT / BAR ANALYSIS =====
	private async void RunBpmAnalysis(string audioPath)
	{
		var systemScriptPath = ProjectSettings.GlobalizePath(ScriptPath);

		try
		{
			var psi = new ProcessStartInfo
			{
				FileName = PythonExe,
				Arguments = $"\"{systemScriptPath}\" \"{audioPath}\"",
				RedirectStandardOutput = true,
				RedirectStandardError = true,
				UseShellExecute = false,
				CreateNoWindow = true
			};

			using var process = Process.Start(psi);

			string stdout = await process.StandardOutput.ReadToEndAsync();
			string stderr = await process.StandardError.ReadToEndAsync();

			await process.WaitForExitAsync();

			if (!string.IsNullOrWhiteSpace(stderr))
				GD.PrintErr("Python stderr:\n", stderr);

			if (process.ExitCode != 0)
			{
				bpmLabel.Text = "Error: Analysis failed";
				GD.PushError($"Python exited with code {process.ExitCode}");
				return;
			}

			using JsonDocument doc = JsonDocument.Parse(stdout);
			JsonElement root = doc.RootElement;

			// ---- BPM ----
			float bpm = root.GetProperty("tempo_bpm").GetSingle();
			bpmLabel.Text = $"BPM: {bpm:F2}";

			// ---- BEATS ----
			List<float> beatTimes = new();
			foreach (JsonElement e in root.GetProperty("beat_times_seconds").EnumerateArray())
				beatTimes.Add(e.GetSingle());

			// ---- BARS (DOWNBEATS) ----
			List<float> barTimes = new();
			foreach (JsonElement e in root.GetProperty("bar_times_seconds").EnumerateArray())
				barTimes.Add(e.GetSingle());

			int beatsPerBar = root.GetProperty("beats_per_bar").GetInt32();

			GD.Print($"BPM: {bpm}");
			GD.Print($"Beats detected: {beatTimes.Count}");
			GD.Print($"Bars detected: {barTimes}");
			GD.Print($"Meter: {beatsPerBar}/4");
			
			var songName = audioPath.Split("/").Last().Replace(".mp3", "");
			string path = $"user://CustomSongs/{songName}.json";
			if (!FileAccess.FileExists(path))
			{
				// Maak het bestand
				using var file = FileAccess.Open(path, FileAccess.ModeFlags.Write);
				if (file == null)
				{
					GD.PrintErr($"Failed to create file: {path}");
				}

				file.StoreLine(songName);
				file.StoreLine(bpm.ToString());
				file.StoreLine("");
				file.StoreLine("false");
			}
		}
		catch (Exception ex)
		{
			bpmLabel.Text = "Error: Exception occurred";
			GD.PushError(ex.ToString());
		}
	}

}
