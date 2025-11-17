using Godot;
using System;

public partial class Hitzone : Area2D
{
	[Export] public Key HitKey = Key.Space; // toets waarop gedrukt moet worden
	private ColorRect _visual;
	private Color _defaultColor = new Color(0.2f, 1f, 0.2f, 0.3f);
	private Color _hitColor = new Color(1f, 1f, 1f, 0.8f);
	ScoreManager scoreManager;
	private Block _enteredBlock = null;

	public override void _Ready()
	{
		_visual = GetNode<ColorRect>("ColorRect");
		_visual.Color = _defaultColor;
		scoreManager = GetNode<ScoreManager>("../ScoreManager");
		
		// Signalen verbinden (als blocks binnenkomen of weggaan)
		BodyEntered += OnBlockEntered;
		BodyExited += OnBlockExited;
	}

	private void OnBlockEntered(Node body)
	{
		if (body is Block block)
		{
			// markeer dat de block in de zone is
			_enteredBlock = block;
			
			// Verander kleur voor feedback
			_visual.Color = _hitColor;
			GD.Print("🎯 Block in hitzone!");
		}
	}

	private void OnBlockExited(Node body)
	{
		if (body == _enteredBlock)
		{
			// markeer dat de block eruit is
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
		if (keyEvent.Pressed && keyEvent.Keycode == HitKey)
		{
			if (_enteredBlock != null)
			{
				GD.Print("✅ Perfect hit!");
				scoreManager.AddPoint();
				_enteredBlock.QueueFree();
				_enteredBlock = null;
				_visual.Color = _defaultColor;
			}
			else
			{
				GD.Print("❌ Miss!");
			}
		}
	}
}
