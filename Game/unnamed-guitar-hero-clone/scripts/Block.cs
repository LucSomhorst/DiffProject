 using Godot;
using System;

public partial class Block : CharacterBody2D
{
	[Export] public float Speed = 200f; // snelheid waarmee het blok valt
	private bool _isHit = false;
	
	public override void _Process(double delta)
	{
		// Blok beweegt naar beneden
		Position += new Vector2(0, Speed * (float)delta);

		// Als blok buiten beeld valt → verwijderen
		if (Position.Y > 800)
			QueueFree();
	}

	public override void _Input(InputEvent @event)
	{
		// Alleen reageren als het blok nog niet geraakt is
		if (_isHit || !(@event is InputEventKey keyEvent)) return;
		
		// Check of de speler op de juiste toets drukt (bijv. spatie)
		if (keyEvent.Pressed && keyEvent.Keycode == Key.Space)
		{
			bool inHitZone = HasMeta("in_hit_zone") && GetMeta("in_hit_zone").AsBool();
			
			if (inHitZone)
			{
				_isHit = true;
				GD.Print("✅ Perfect hit!");
				QueueFree();
			}
			else
			{
				GD.Print("❌ Miss!");
			}
		}
	}
}
