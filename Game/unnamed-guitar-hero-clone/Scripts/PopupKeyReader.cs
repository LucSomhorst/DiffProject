using Godot;
using System;

public partial class PopupKeyReader : Popup
{
	private KeybindLine parent;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	private double inputCooldown = 0.0;

	public override void _Process(double delta)
	{
		if (inputCooldown > 0)
			inputCooldown -= delta;
	}

	public override void _Input(InputEvent @event)
	{
		// Ignore all input during cooldown
		if (inputCooldown > 0)
			return;

		// Mouse button (pressed only)
		if (@event is InputEventMouseButton mouseButton && mouseButton.Pressed)
		{
			GD.Print($"Mouse button: {mouseButton.ButtonIndex} pressed");
			parent.KeyChanged(mouseButton.ButtonIndex.ToString());

			// Start 1-second cooldown
			inputCooldown = 1.0;
			QueueFree();
		}
		// Keyboard (pressed only)
		else if (@event is InputEventKey keyEvent && keyEvent.Pressed)
		{
			parent.KeyChanged(keyEvent.Keycode.ToString());
			// Start 1-second cooldown
			inputCooldown = 1.0;
			QueueFree();
		}
	}
	public void Constructor(KeybindLine _parent)
	{
		parent = _parent;
	}
}
