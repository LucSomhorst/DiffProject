 using Godot;
using System;

public abstract partial class BlockBase : CharacterBody2D
{
	[Export] public float Speed = 200f; // snelheid waarmee het blok valt
	public bool IsInsideHitzone { get; set; } = false;
	
	// Blok beweegt naar beneden
	public override void _Process(double delta)
	{
		Position += new Vector2(0, Speed * (float)delta);
	}
	
	// Wordt aangeroepen wanneer key wordt gedrukt in hitzone
	public abstract void OnHit(Hitzone zone);
	
	// Wordt elke frame aangeroepen als block in hitzone blijft zitten
	public abstract void OnHold(Hitzone zone);
	
	// Wordt aangeroepen wanneer key wordt losgelaten
	public abstract void OnHoldEnd(Hitzone zone);
	
	private void OnScreenExited()
	{
		QueueFree();
		GD.Print("Removed");
	}
}
