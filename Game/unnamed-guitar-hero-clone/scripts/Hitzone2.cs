using Godot;
using System;

public partial class Hitzone2 : Hitzone
{
	public override void _Ready()
	{
		HitKey = Key.S;
		base._Ready();
	}
}
