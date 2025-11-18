using Godot;
using System;

public partial class LevelSelect : Control
{
	// Called when the node enters the scene tree for the first time.
	[Signal]
	public delegate void Level1EventHandler();
	[Signal]
	public delegate void Level2EventHandler();
	[Signal]
	public delegate void Level3EventHandler();
	[Signal]
	public delegate void ExitBtnEventHandler();
	public override void _Ready()
	{
		
	}
	
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	
	private void OnLevel1Click()
	{
		EmitSignalLevel1();
	}
	private void OnLevel2Click()
	{
		EmitSignalLevel2();
	}
	private void OnLevel3Click()
	{
		EmitSignalLevel3();
	}

	private void OnExitBtnPressed()
	{
		EmitSignalExitBtn();
	}
	private void MenuBtnClicked()
	{
		var yes = GetParent();
		var main = yes.GetParent();
		
		main.EndGame();
	}
}
