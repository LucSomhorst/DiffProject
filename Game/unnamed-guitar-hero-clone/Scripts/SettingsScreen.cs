using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Godot;
using FileAccess = Godot.FileAccess;

namespace UnnamedGuitarHeroClone.Scripts;

public partial class SettingsScreen : MarginContainer
{
	private List<string> WindowOptions = ["windowed", "fullscreen", "borderless"];
	private List<string> resolutionOptions = ["3840", "2560", "1920", "1600", "1536","1440", "1366", "1280"];
	private List<string> uiscaleOptions = ["150", "100", "50"];
	private int height;
	private int width;
	private string window;
	private ConfigFile ConfigLocal = new();
	private int musicVolume;
	private int soundVolume;	
	[Signal]
	public delegate void ReturnBtnEventHandler();

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		ConfigLocal.Load("res://settings.cfg");
		SetSettingsUI();
		SetKeybindsUI();
		var uploadContainer = (BpmAnalyzer)GetNode("MainWindowMargin/VBoxContainer/TabContainer/Upload song/MarginContainer/MarginContainer/UploadContainer");
		uploadContainer.SongUploaded += SaveCustomSong;
		SetCustomSongUI();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	// Gets all editable keybinds and sets their value according to saved keybinds

	private void SetCustomSongUI()
	{
		var SongsBox = GetNode("%CustomSongContainer");
		var filesToLoad = GetCustomSongs();
		foreach (var file in filesToLoad)
		{
			var packedScene = ResourceLoader.Load<PackedScene>("res://Scenes/CustomLevelButton.tscn");
			var nodeScene = (CustomLevelButton)packedScene.Instantiate();
			var variant = file.Split("/").Last();
			var levelName = variant.Replace(".mp3", "");
			nodeScene.Name = levelName;
			nodeScene.OnButtonPressed += OpenCustomSongEditor;
			SongsBox.AddChild(nodeScene);
		}

	}

	private void OpenCustomSongEditor()
	{
		
	}

	private string[] GetCustomSongs()
	{
		string path = "user://CustomSongs";
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

		var files = dir_access.GetFiles().Where(x => x.EndsWith(".json", StringComparison.OrdinalIgnoreCase)).ToArray();
		return files;
	}

	private void SaveCustomSong(string path)
	{
		var file = FileAccess.Open(path, FileAccess.ModeFlags.Read);
		if (file == null)
		{
			GD.PrintErr("Failed to open file: " + path);
			return;
		}
		var fileName = Path.GetFileName(path);
		var savePath = "user://CustomSongs/" + fileName;
		var bytes = file.GetBuffer((long)file.GetLength());
		file.Close();
		var savefile = FileAccess.Open(savePath, FileAccess.ModeFlags.Write);
		savefile.StoreBuffer(bytes);
		savefile.Close();
	}
	private void SetKeybindsUI()
	{
		var keybindsContainer = (VBoxContainer)GetNode("%KeybindVBox");
		var keybinds = ConfigLocal.GetSectionKeys("Keybinds");
		foreach (var keybind in keybinds)
		{
			var value = ConfigLocal.GetValue("Keybinds", keybind).ToString();
			var packedScene = ResourceLoader.Load<PackedScene>($"res://Scenes/KeybindLine.tscn");
			var NodeScene = (KeybindLine)packedScene.Instantiate();
			NodeScene.Constructor(keybind, value, this,GetKeyDisplayName(keybind));
			keybindsContainer.AddChild(NodeScene);
		}
	}
	
	private string GetKeyDisplayName(string key)
	{
		switch (key)
		{
			//
			case "Player1Hitzone1":
				return "P1 Hit zone 1";
			case "Player1Hitzone2":
				return "P1 Hit zone 2";
			case "Player1Hitzone3":
				return "P1 Hit zone 3";
			case "Player1Hitzone4":
				return "P1 Hit zone 4";
			case "Player2Hitzone1":
				return "P2 Hit zone 1";
			case "Player2Hitzone2":
				return "P2 Hit zone 2";
			case "Player2Hitzone3":
				return "P2 Hit zone 3";
			case "Player2Hitzone4":
				return "P2 Hit zone 4";
		}
		return key + "unnamed";
	}
	
	public void KeyBindChanged(string key, string value)
	{
		ConfigLocal.SetValue("Keybinds",key,value);
	}
	
	// Gets all setting option nodes and sets their value according to saved settings
	private void SetSettingsUI()
	{
		var windowNode = (OptionButton)GetNode("%WindowMode");
		var resolutionNode = (OptionButton)GetNode("%Resolution");
		var uiScaleNode = (OptionButton)GetNode("%UIScale");
		var screenshakeNode = (CheckButton)GetNode("%Screenshake");
		var muteAllNode = (Button)GetNode("%MuteAll");
		var musicVolumeLabel = (Label)GetNode("%MusicValueLabel");
		var soundVolumeLabel = (Label)GetNode("%SoundValueLabel");
		var musicVolumeSlider = (Slider)GetNode("%MusicSlider");
		var soundVolumeSlider = (Slider)GetNode("%SoundSlider");
		windowNode.Selected = WindowOptions.FindIndex(
			x => x.Equals(ConfigLocal.GetValue("settings", "window", "Windowed").AsString()));
		resolutionNode.Selected = resolutionOptions.FindIndex(
			x => x.Equals(ConfigLocal.GetValue("settings", "reswidth", "1280").AsString()));
		uiScaleNode.Selected = uiscaleOptions.FindIndex(
			x => x.Equals(ConfigLocal.GetValue("settings", "uiscale", "100").AsString()));
		screenshakeNode.ButtonPressed = ConfigLocal.GetValue("settings", "screenshake", false).AsBool();
		muteAllNode.Text= ConfigLocal.GetValue("settings", "muteAll", "Mute all").ToString();
		musicVolumeLabel.Text = ConfigLocal.GetValue("settings", "musicvolume", 0).ToString();
		soundVolumeLabel.Text = ConfigLocal.GetValue("settings", "soundvolume", 0).ToString();
		musicVolumeSlider.Value = ConfigLocal.GetValue("settings", "musicvolume", 0).AsInt32();
		soundVolumeSlider.Value = ConfigLocal.GetValue("settings", "soundvolume", 0).AsInt32();
	}

	private void MusicSliderValueChanged(float variant)
	{
		musicVolume = Convert.ToInt32(variant); 
		var musiclabel = (Label)GetNode("%MusicValueLabel");
		musiclabel.Text = musicVolume.ToString();
	}

	private void SoundSliderValueChanged(float variant)
	{
		soundVolume = Convert.ToInt32(variant);
		var soundlabel = (Label)GetNode("%SoundValueLabel");
		soundlabel.Text = soundVolume.ToString();
	}

	private void OnReturnBtnPressed()
	{
		EmitSignalReturnBtn();
	}

	private void SaveChangesBtnPressed()
	{
		ConfigLocal.Save("res://settings.cfg");
	}

	private void RevertToStandardBtnPressed()
	{
		
	}
	
	private void SettingChanged(Variant variant, string settingChanged)
	{
		GD.Print(settingChanged + variant);
		switch (settingChanged)
		{
			case "resolution":
				switch (variant.ToString())
				{
					case "0" :
						height = 3840;
						width = 2160;
						ConfigLocal.SetValue("settings", "reswidth", "3840");
						ConfigLocal.SetValue("settings", "resheight", "2160");
						break;
					case "1" :
						height = 2560;
						width = 1440;
						ConfigLocal.SetValue("settings", "reswidth", "2560");
						ConfigLocal.SetValue("settings", "resheight", "1440");
						break;
					case "2" :
						height = 1920;
						width = 1080;
						ConfigLocal.SetValue("settings", "reswidth", "1920");
						ConfigLocal.SetValue("settings", "resheight", "1080");
						break;
					case "3" :
						height = 1600;
						width = 900;
						ConfigLocal.SetValue("settings", "reswidth", "1600");
						ConfigLocal.SetValue("settings", "resheight", "900");
						break;
					case "4" :
						height = 1536;
						width = 864;
						ConfigLocal.SetValue("settings", "reswidth", "1536");
						ConfigLocal.SetValue("settings", "resheight", "864");
						break;
					case "5" :
						height = 1440;
						width = 900;
						ConfigLocal.SetValue("settings", "reswidth", "1440");
						ConfigLocal.SetValue("settings", "resheight", "900");
						break;
					case "6" :
						height = 1366;
						width = 768;
						ConfigLocal.SetValue("settings", "reswidth", "1366");
						ConfigLocal.SetValue("settings", "resheight", "768");	
						break;
					case "7" :
						height = 1280;
						width = 720;
						ConfigLocal.SetValue("settings", "reswidth", "1280");
						ConfigLocal.SetValue("settings", "resheight", "720");
						break;
				}

				GetWindow().Size = new Vector2I(height, width);
				break;
			case "window" :
				switch (variant.ToString())
				{
					case "0":
						DisplayServer.WindowSetMode(DisplayServer.WindowMode.Windowed);
						ConfigLocal.SetValue("settings", "window", WindowOptions[variant.AsInt32()]);
						break;
					case "1":
						DisplayServer.WindowSetMode(DisplayServer.WindowMode.ExclusiveFullscreen);
						ConfigLocal.SetValue("settings", "window", WindowOptions[variant.AsInt32()]);
						break;
					case "2":
						DisplayServer.WindowSetMode(DisplayServer.WindowMode.Fullscreen);
						ConfigLocal.SetValue("settings", "window", WindowOptions[variant.AsInt32()]);
						break;
				}
				break;
			case "uiscale":
				switch (variant.ToString())
				{
					case "0":
						ConfigLocal.SetValue("settings", "uiscale", uiscaleOptions[variant.AsInt32()]);
						break;
					case "1":
						ConfigLocal.SetValue("settings", "uiscale", uiscaleOptions[variant.AsInt32()]);
						break;
					case "2":
						ConfigLocal.SetValue("settings", "uiscale", uiscaleOptions[variant.AsInt32()]);
						break;
				}
				break;
			case "screenshake":
				switch (variant.ToString())
				{
					case "true":
						ConfigLocal.SetValue("settings", "screenshake", "true");
						break;
					case "false":
						ConfigLocal.SetValue("settings", "screenshake", "false");
						break;
				}
				break;
			case "muteall" :
				switch (variant.ToString())
				{
					case "true":
						ConfigLocal.SetValue("settings", "muteall", "true");
						break;
					case "false":
						ConfigLocal.SetValue("settings", "muteall", "false");
						break;
				}
				break;
			case "musicvolume" :
				if ((bool)variant)
				{
					ConfigLocal.SetValue("settings", "musicvolume", musicVolume);
				}
				break;
			case "soundvolume" :
				if ((bool)variant)
				{
					ConfigLocal.SetValue("settings", "soundvolume", soundVolume);
				}
				break;
		}
	}
}
