using Godot;
using System;

public partial class CustomLevelButton : Control
{
	[Signal]
	public delegate void OnButtonPressedEventHandler();
	[Export] private Label NameLabel;
	[Export] private Button EditButton;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		if (EditButton != null)
			EditButton.Pressed += _on_Button_pressed;
		if  (NameLabel != null)
			NameLabel.Text = Name;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void _on_Button_pressed()
	{
		EmitSignalOnButtonPressed();
	}
}
