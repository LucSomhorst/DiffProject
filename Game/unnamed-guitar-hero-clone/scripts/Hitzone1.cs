using Godot;
using System;

public partial class Hitzone1 : Hitzone
{
	private ConfigFile ConfigLocal = new();
	
	public override void _Ready()
	{
		ConfigLocal.Load("res://settings.cfg");
		var value = ConfigLocal.GetValue("Keybinds", "leftzone").ToString();
		if (Enum.TryParse<Key>(value, out Key parsedKey))
		{
			HitKey = parsedKey;
		}
		else
		{
			GD.PrintErr($"Invalid key in config: {value}. Using default A.");
			HitKey = Key.A;
		}
		base._Ready();
	}
}
