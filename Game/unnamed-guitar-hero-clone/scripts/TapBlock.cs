using Godot;
using System;

public partial class TapBlock : BlockBase
{
	public override void OnHit()
	{
		QueueFree();
	}
}
