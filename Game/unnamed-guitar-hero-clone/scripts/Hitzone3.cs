using Godot;
using System;

public partial class Hitzone3 : Hitzone
{
	public override void _Ready()
	{
		ConfigLocal.Load("res://settings.cfg");
		HitKey = ConfigLocal.GetValue("Keybinds", "middleRightZone").ToString();
		base._Ready();
	}
}
