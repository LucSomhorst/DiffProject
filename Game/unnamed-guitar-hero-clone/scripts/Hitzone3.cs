using Godot;
using System;

public partial class Hitzone3 : Hitzone
{
	public override void _Ready()
	{
		HitKey = Key.D;
		base._Ready();
	}
}
