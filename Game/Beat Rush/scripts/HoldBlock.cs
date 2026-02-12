using Godot;
using System;

public partial class HoldBlock : BlockBase
{
	private bool _isHolding = false;
	private float _holdTime = 0f;
	private float _requiredHoldTime = 0.3f; // voorbeeld: 0.3 seconden vasthouden
	
	public override void OnHit()
	{
		_isHolding = true;
	}

	public void OnHold(Hitzone zone)
	{
		if (!_isHolding) return;

		_holdTime += (float)zone.GetProcessDeltaTime();


	}

	public void OnHoldEnd(double timeHeld)
	{
		if ( timeHeld < _requiredHoldTime)
		{
			EmitSignalBlockMissed();
		}
		else if (timeHeld >= _requiredHoldTime)
		{
			EmitSignalBlockHit(this);
			QueueFree();
		}

		_isHolding = false;
	}
}
