using System;
using Godot;

namespace UnnamedGuitarHeroClone.Scripts;

public partial class GameContainer : PanelContainer
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	
	private Node ChangeScene(string scene)
	{
		var children = GetChildren();
		foreach (var child in children)
		{
			child.QueueFree();
		}
		var packedScene = ResourceLoader.Load<PackedScene>($"res://Scenes/{scene}.tscn");
		var NodeScene = packedScene.Instantiate();
		AddChild(NodeScene);
		GD.Print(NodeScene.Name);
		return NodeScene;
	}
	
	public void OpenGameScreen()
	{
		var levelSelect = (LevelSelect)ChangeScene("LevelSelect");
		levelSelect.ExitBtn += ExitGame; 
	}

	internal void OpenSingleplayer(string path)
	{
		GD.Print("level opened");
		var level = (LevelScreen)ChangeScene("LevelScreen");
		GD.Print(path);
		level.Constructor(path);
		level.EndGame += OpenGameScreen;
	}

	internal void OpenMultiplayer(string path)
	{
		GD.Print("level opened");
		var level = (MultiplayerScreen)ChangeScene("MultiplayerScreen");
		GD.Print(path);
		level.Constructor(path);
		level.EndGame += OpenGameScreen;
	}
	
	public void ExitGame()
	{
		GetParent().GetTree().Quit();
		GD.Print("Exit");
	}

	public void StartGame()
	{
		OpenGameScreen();
	}
	
	public void EndGame()
	{
		var main = (Main)GetParent();
		main.EndGame();
	}

	
}
