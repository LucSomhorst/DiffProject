using Godot;
using System;

public partial class Hitzone2 : Hitzone
{
	public override void _Ready()
	{
		ConfigLocal.Load("res://settings.cfg");
		HitKey = ConfigLocal.GetValue("Keybinds", "middleLeftZone").ToString();
		base._Ready();
	}
}
