using Godot;
using System;

public partial class TapBlock : BlockBase
{
	private bool _isHit = false;

	public override void OnHit(Hitzone zone)
	{
		if (_isHit) return;
		_isHit = true;
		GD.Print("🎯 TAP HIT");
		QueueFree();
	}

	public override void OnHold(Hitzone zone)
	{
		// Tap block gebruikt dit niet
	}

	public override void OnHoldEnd(Hitzone zone)
	{
		// Tap block gebruikt dit niet
	}
}
