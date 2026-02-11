using Godot;
using System;

public partial class Hitzone2 : Hitzone
{
	private ConfigFile ConfigLocal = new();
	
	public override void _Ready()
	{
		// ConfigLocal.Load("res://settings.cfg");
		// HitKey = ConfigLocal.GetValue("Keybinds", "Player1Hitzone2").ToString();
		// base._Ready();
	}
}
