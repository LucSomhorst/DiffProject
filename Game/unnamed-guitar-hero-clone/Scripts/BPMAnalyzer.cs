using Godot;
using System;
using System.Diagnostics;
using System.IO;
using System.Text.Json;
using System.Collections.Generic;

public partial class BpmAnalyzer : Control
{
	// ===== CONFIG =====
	private const string PythonExe = @"C:\Python313\python.exe"; // Full path to Python with librosa
	private const string ScriptPath = "res://External/GetBPM.py"; // Python BPM script

	// ===== NODES =====
	[Export] private FileDialog fileDialog;
	[Export] private Label bpmLabel;
	[Export] private Button selectButton;

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
		RunBpmAnalysis(systemPath);
	}

	// ===== RUN PYTHON BPM ANALYSIS =====
	private void RunBpmAnalysis(string audioPath)
	{
		var systemScriptPath = ProjectSettings.GlobalizePath(ScriptPath);
		GD.Print(audioPath, systemScriptPath);
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

			using (var process = Process.Start(psi))
			{
				process.WaitForExit();

				string stdout = process.StandardOutput.ReadToEnd();
				string stderr = process.StandardError.ReadToEnd();

				GD.Print("Python stdout:\n", stdout);
				if (!string.IsNullOrWhiteSpace(stderr))
					GD.PrintErr("Python stderr:\n", stderr);

				if (process.ExitCode != 0)
				{
					bpmLabel.Text = "Error: Analysis failed";
					GD.PushError($"Python exited with code {process.ExitCode}");
					return;
				}
			}

			// Read JSON output
			string jsonPath = Path.ChangeExtension(audioPath, ".json");
			if (!File.Exists(jsonPath))
			{
				bpmLabel.Text = "Error: JSON not found";
				GD.PushError($"BPM JSON not found: {jsonPath}");
				return;
			}

			string jsonText = File.ReadAllText(jsonPath);
			using (JsonDocument doc = JsonDocument.Parse(jsonText))
			{
				if (doc.RootElement.TryGetProperty("bpm", out JsonElement bpmElement))
				{
					float bpm = bpmElement.GetSingle();
					bpmLabel.Text = $"BPM: {bpm}";
					GD.Print($"Detected BPM: {bpm}");
				}
				else
				{
					bpmLabel.Text = "Error: Invalid JSON";
					GD.PushError("BPM property missing in JSON");
				}
			}
		}
		catch (Exception ex)
		{
			bpmLabel.Text = "Error: Exception occurred";
			GD.PushError(ex.ToString());
		}
	}
}
