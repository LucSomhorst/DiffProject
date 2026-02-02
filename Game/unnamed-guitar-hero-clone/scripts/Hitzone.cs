using Godot;
using System;

public partial class Hitzone : Area2D
{
	[Export] public String HitKey = "Space"; // toets waarop gedrukt moet worden
	private Key _key;
	
	private ColorRect _visual;
	private Color _defaultColor = new Color(0.2f, 1f, 0.2f, 0.3f);
	private Color _hitColor = new Color(1f, 1f, 1f, 0.8f);
	
	ScoreManager scoreManager;
	private BlockBase _enteredBlock = null;

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
		if (body is BlockBase block)
		{
			// markeer dat de block in de zone is
			_enteredBlock = block;
			block.IsInsideHitzone = true;
			_enteredBlock.BlockHit += OnHit;
			_enteredBlock.BlockMissed += OnMiss;
			// Verander kleur voor feedback
			_visual.Color = _hitColor;
		}
	}

	private void OnBlockExited(Node body)
	{
		if (body is BlockBase block && body == _enteredBlock)
		{
			// markeer dat de block eruit is
			_enteredBlock = null;
			block.IsInsideHitzone = false;
			
			// Reset kleur
			_visual.Color = _defaultColor;
			//GD.Print("⬅️ Block left hitzone");
		}
	}
	
	public override void _Input(InputEvent @event)
	{
		// Alleen reageren als het blok nog niet geraakt is
		if (!(@event is InputEventKey keyEvent)) return;
		
		if (Enum.TryParse<Key>(HitKey, out Key parsedKey))
		{
			_key = parsedKey;
		}
		else
		{
			GD.PrintErr($"Invalid key in config: {HitKey}. Using default Space.");
			_key = Key.Space;
		}
		
		// Check of de speler op de juiste toets drukt (bijv. spatie)
		if (keyEvent.Pressed && keyEvent.Keycode == _key)
		{
			if (_enteredBlock != null)
			{
				_enteredBlock.OnHit(this);
				if (_enteredBlock == null){
					_visual.Color = _defaultColor;
				}
			}
			else
			{
				OnMiss();
			}
		}
		
		// Detect key release (voor HoldBlock)
		if (!keyEvent.Pressed && keyEvent.Keycode == _key)
		{
			if (_enteredBlock != null)
			{
				_enteredBlock.OnHoldEnd(this);
				_visual.Color = _defaultColor;
			}
		}
	}

	private void OnHit(BlockBase sender)
	{
		var blockType = sender.GetType();
		if (blockType == typeof(TapBlock))
		{
			scoreManager.BlockHit();
		}
			
	}

	private void OnMiss()
	{
		scoreManager.BlockMissed();
	}
	
	public override void _Process(double delta)
	{
		// Als block in de zone zit -> hold check
		if (_enteredBlock != null)
		{
			_enteredBlock.OnHold(this);
		}
	}
}
