 using Godot;
using System;

public abstract partial class BlockBase : CharacterBody2D
{
	[Export] public float Speed = 200f; // snelheid waarmee het blok valt
	public bool IsInsideHitzone { get; set; } = false;
	[Signal]
	public delegate void BlockHitEventHandler(BlockBase sender);
	[Signal]
	public delegate void BlockMissedEventHandler();
	// Blok beweegt naar beneden
	public override void _Process(double delta)
	{
		Position += new Vector2(0, Speed * (float)delta);
	}
	
	// Wordt aangeroepen wanneer key wordt gedrukt in hitzone
	public abstract void OnHit();
	
	private void OnScreenExited()
	{
		EmitSignalBlockMissed();
		QueueFree();
	}
}
