using Godot;
using System;

public partial class GameContainer : PanelContainer
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		OpenGameScreen();
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
		return NodeScene;
	}
	
	public void OpenGameScreen()
	{
		var levelScreen = (LevelSelect)ChangeScene("LevelSelect");
		levelScreen.Level1 += OpenLevel1;
		levelScreen.Level2 += OpenLevel2;
		levelScreen.Level3 += OpenLevel3;
		levelScreen.ExitBtn += ExitGame; 
	}
	
	private void OpenLevel1()
	{
		var level = (LevelScreen)ChangeScene("LevelScreen");
		level.LevelFile = "res://Levels/Level 1.txt";
		level.Contructor();
		level.EndGame += OpenGameScreen;
	}
	
	private void OpenLevel2()
	{
		var level = (LevelScreen)ChangeScene("LevelScreen");
		level.LevelFile = "res://Levels/Level 2.txt";
		level.Contructor();
		level.EndGame += OpenGameScreen;
	}
	
	private void OpenLevel3()
	{
		var level = (LevelScreen)ChangeScene("LevelScreen");
		level.LevelFile = "res://Levels/Level 3.txt";
		level.Contructor();
		level.EndGame += OpenGameScreen;
	}
	
	private void ExitGame()
	{
		GetParent().GetTree().Quit();
		GD.Print("Exit");
	}

	
}
