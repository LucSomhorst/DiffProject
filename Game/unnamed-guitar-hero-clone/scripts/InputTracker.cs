using Godot;

public partial class InputTracker : Node
{
	public enum InputDevice
	{
		Keyboard,
		Controller
	}

	public static InputDevice LastInputDevice { get; private set; } = InputDevice.Keyboard;

	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventKey)
		{
			LastInputDevice = InputDevice.Keyboard;
		}
		else if (@event is InputEventJoypadButton || @event is InputEventJoypadMotion)
		{
			LastInputDevice = InputDevice.Controller;
		}
	}
}
