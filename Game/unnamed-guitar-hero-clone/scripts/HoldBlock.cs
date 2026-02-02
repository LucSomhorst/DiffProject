using Godot;
using System;

public partial class HoldBlock : BlockBase
{
	private bool _isHolding = false;
	private float _holdTime = 0f;
	private float _requiredHoldTime = 0.3f; // voorbeeld: 0.3 seconden vasthouden
	
	public override void OnHit(Hitzone zone)
	{
		_isHolding = true;
		GD.Print("⏳ HOLD START");
	}

	public override void OnHold(Hitzone zone)
	{
		if (!_isHolding) return;

		_holdTime += (float)zone.GetProcessDeltaTime();

		if (_holdTime >= _requiredHoldTime)
		{
			EmitSignalBlockHit(this);
			QueueFree();
		}
	}

	public override void OnHoldEnd(Hitzone zone)
	{
		if (_isHolding && _holdTime < _requiredHoldTime)
		{
			EmitSignalBlockMissed();
		}

		_isHolding = false;
	}
}
