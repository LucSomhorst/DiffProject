using Godot;
using System;

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
	
	public void StartGame()
	{
		ChangeScene("Test Scene Thomas");
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
}
