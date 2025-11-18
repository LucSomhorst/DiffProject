using System.Drawing;
using Godot;

namespace UnnamedGuitarHeroClone.Scripts;

public partial class Main : Node
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		ApplySettings();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void StartGame()
	{
		var menuNode = (MenusContainer)GetNode("MenusContainer");
		var children =  menuNode.GetChildren();
		foreach (var child in children)
		{
			child.QueueFree();
		}
		var gameNode = (GameContainer)GetNode("GameContainer"); 
		gameNode.StartGame();
		menuNode.Hide();
		gameNode.Show();
	}
	private void Exit()
	{
		GetTree().Quit();
	}

	public void EndGame()
	{
		var gameNode = (GameContainer)GetNode("GameContainer");
		gameNode.Hide();
		var children =  gameNode.GetChildren();
		foreach (var child in children)
		{
			child.QueueFree();
		}
		var menuNode = (MenusContainer)GetNode("MenusContainer");
		menuNode.OpenMenuScreen();
		menuNode.Show();
	}

	private void ApplySettings()
	{
		var config = new ConfigFile();
		config.Load("res://settings.cfg");
		var windowMode = config.GetValue("settings", "window");
		switch (windowMode.ToString())
		{
			case "windowed":
				DisplayServer.WindowSetMode(DisplayServer.WindowMode.Windowed);
				break;
			case "fullscreen":
				DisplayServer.WindowSetMode(DisplayServer.WindowMode.ExclusiveFullscreen);
				break;
			case "borderless":
				DisplayServer.WindowSetMode(DisplayServer.WindowMode.Fullscreen);
				break;
		}
		if (windowMode.ToString() != "Windowed")
		{
			var width = config.GetValue("settings", "reswidth").AsInt32();
			var height = config.GetValue("settings", "resheight").AsInt32();
			GetWindow().Size = new Vector2I(width, height);
		}
		//UI scale options
		
		//Volume options
		
		//Screenshake and other visual options
		

	}
}
