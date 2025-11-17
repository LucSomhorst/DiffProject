using Godot;
using System;

public partial class Hitzone1 : Hitzone
{
	public override void _Ready()
	{
		HitKey = Key.A;
		base._Ready();
	}
}
