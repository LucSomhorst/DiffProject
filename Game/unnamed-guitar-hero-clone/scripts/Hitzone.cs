using Godot;
using System;

public partial class Hitzone : Area2D
{
	private ColorRect _visual;
	private Color _defaultColor = new Color(0.2f, 1f, 0.2f, 0.3f);
	private Color _hitColor = new Color(1f, 1f, 1f, 0.8f);
	private bool _inHitZone = false;
	private Block _enteredBlock;

	public override void _Ready()
	{
		_visual = GetNode<ColorRect>("ColorRect");
		_visual.Color = _defaultColor;

		// Signalen verbinden (als blocks binnenkomen of weggaan)
		BodyEntered += OnBlockEntered;
		BodyExited += OnBlockExited;
	}

	private void OnBlockEntered(Node body)
	{
		if (body is Block block)
		{
			// markeer dat de block in de zone is
			_inHitZone = true;
			_enteredBlock = block;
			
			// Verander kleur voor feedback
			_visual.Color = _hitColor;
			GD.Print("🎯 Block in hitzone!");
		}
	}

	private void OnBlockExited(Node body)
	{
		if (body is Block block)
		{
			// markeer dat de block eruit is
			_inHitZone = false;
			_enteredBlock = null;
			
			// Reset kleur
			_visual.Color = _defaultColor;
			GD.Print("⬅️ Block left hitzone");
		}
	}
	
	public override void _Input(InputEvent @event)
	{
		// Alleen reageren als het blok nog niet geraakt is
		if (!(@event is InputEventKey keyEvent)) return;
		
		// Check of de speler op de juiste toets drukt (bijv. spatie)
		if (keyEvent.Pressed && keyEvent.Keycode == Key.Space)
		{
			
			if (_inHitZone)
			{
				GD.Print("✅ Perfect hit!");
				_enteredBlock.QueueFree();
			}
			else
			{
				GD.Print("❌ Miss!");
			}
		}
	}
}
