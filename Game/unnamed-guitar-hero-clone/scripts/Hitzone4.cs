using Godot;
using System;

public partial class Hitzone4 : Hitzone
{
	public override void _Ready()
	{
		ConfigLocal.Load("res://settings.cfg");
		HitKey = ConfigLocal.GetValue("Keybinds", "rightZone").ToString();
		base._Ready();
	}
}
