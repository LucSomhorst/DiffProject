using Godot;
using System;

public partial class Hitzone : Area2D
{
	private Key _key;
	private JoyButton _button;

	private ColorRect _visual;
	private Color _defaultColor = new Color(0.2f, 1f, 0.2f, 0.3f);
	private Color _hitColor = new Color(1f, 1f, 1f, 0.8f);

	ScoreManager scoreManager;
	private BlockBase _enteredBlock = null;
	
	private bool _isPressed = false;
	private bool _counting = false;
	private double _pressTime = 0.0;

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
			_enteredBlock.BlockMissed += OnMiss;
			_enteredBlock.BlockHit += OnHit;
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

	public void SetKey(string newKey)
	{
		if (Enum.TryParse(newKey, out Key parsedKey))
		{
			_key = parsedKey;
		}
		else
		{
			GD.PrintErr($"Invalid key in config: {newKey}. Using default Space.");
			_key = Key.Space;
		}
	}

	public void SetButton(string newButton)
	{
		if (Enum.TryParse(newButton, out JoyButton parsedButton))
		{
			_button = parsedButton;
		}
		else
		{
			GD.PrintErr($"Invalid key in config: {newButton}. Using default Space.");
			_button = JoyButton.A;
		}
	}


	public override void _Input(InputEvent @event)
	{
		bool pressed;

		if (@event is InputEventKey keyEvent)
		{
			if (keyEvent.Keycode != _key || keyEvent.Echo)
				return;

			pressed = keyEvent.Pressed;
		}
		else if (@event is InputEventJoypadButton joypadEvent)
		{
			if (joypadEvent.ButtonIndex != _button)
				return;

			pressed = joypadEvent.Pressed;
		}
		else if (@event is InputEventSerial serialEvent)
		{
			pressed = serialEvent.Pressed;
		}
		else
		{
			return;
		}

		// Debounce
		if (pressed == _isPressed)
			return;

		_isPressed = pressed;

		if (pressed)
		{
			if (_enteredBlock != null)
			{
				_counting = true;
				_pressTime = Time.GetTicksMsec();
				ButtonPress();
			}
		}
		else
		{
			if (_counting)
			{
				double heldMs = Time.GetTicksMsec() - _pressTime;
				double heldSeconds = heldMs / 1000.0;

				_counting = false;
				ButtonRelease(heldSeconds);
			}
		}
	}

	public void ButtonPress()
	{
		GD.Print("Button press");
		if (_enteredBlock is not null)
		{
			if (_enteredBlock.GetType() == typeof(HoldBlock))
			{
				var _holdBlock = (HoldBlock)_enteredBlock;
				_holdBlock.OnHold(this);
			}
			else
			{
				_enteredBlock.OnHit();
				OnHit(_enteredBlock);
			}
		}
		else
		{
			OnMiss();
		}
	}

	public void ButtonRelease(double timeHeld)
	{
		if (_enteredBlock is HoldBlock)
		{
			var holdBlock = (HoldBlock)_enteredBlock;
			holdBlock.OnHoldEnd(timeHeld);
		}
	}


	private void OnHit(BlockBase sender)
	{
		var blockType = sender.GetType();
		if (blockType == typeof(TapBlock))
		{
			scoreManager.BlockHit();
		}
		else if (blockType == typeof(HoldBlock))
		{
			scoreManager.LongBlockHit();
		}
	}

	private void OnMiss()
	{
		scoreManager.BlockMissed();
	}
	
	public override void _Process(double delta)
	{
		if (_enteredBlock is not null)
		{
			if (_enteredBlock.GetType() == typeof(HoldBlock))
			{
				var _holdBlock = (HoldBlock)_enteredBlock;
				_holdBlock.OnHold(this);
			}
		}
	}
}
