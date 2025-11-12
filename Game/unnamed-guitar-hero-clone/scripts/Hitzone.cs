using Godot;
using System;

public partial class Hitzone : Area2D
{
	private ColorRect _visual;
	private Color _defaultColor = new Color(0.2f, 1f, 0.2f, 0.3f);
	private Color _hitColor = new Color(1f, 1f, 1f, 0.8f);

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
			block.SetMeta("in_hit_zone", true);
			
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
			block.SetMeta("in_hit_zone", false); 
			
			// Reset kleur
			_visual.Color = _defaultColor;
			GD.Print("⬅️ Block left hitzone");
		}
	}
}
