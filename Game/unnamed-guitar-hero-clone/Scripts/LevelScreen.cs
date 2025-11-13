using Godot;
using System;

public partial class LevelScreen : Control
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		var label = GetNode<Label>("LevelName");
		label.Text = "Level 1";//will be read from txtfile, with the block spawn speed
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		
	}
}
