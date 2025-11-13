using Godot;

namespace UnnamedGuitarHeroClone.Scripts;

public partial class Main : Node
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void StartGame()
	{
		var menuNode = (PanelContainer)GetNode("MenusContainer");
		menuNode.Hide();
		var gameNode = (PanelContainer)GetNode("GameContainer");
		gameNode.Show();
	}
	private void Exit()
	{
		GetTree().Quit();
	}
}
