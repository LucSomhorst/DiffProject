 using Godot;
using System;

public partial class Block : CharacterBody2D
{
	[Export] public float Speed = 200f; // snelheid waarmee het blok valt
	
	public override void _Process(double delta)
	{
		// Blok beweegt naar beneden
		Position += new Vector2(0, Speed * (float)delta);

		// Als blok buiten beeld valt → verwijderen
		if (Position.Y > 800)
			QueueFree();
	}
}
