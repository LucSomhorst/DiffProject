using Godot;
using System;

public partial class Hitzone1 : Hitzone
{
	private ConfigFile ConfigLocal = new();
	
	public override void _Ready()
	{
		ConfigLocal.Load("res://settings.cfg");
		HitKey = ConfigLocal.GetValue("Keybinds", "Player1Hitzone1").ToString();
		base._Ready();
	}
}
