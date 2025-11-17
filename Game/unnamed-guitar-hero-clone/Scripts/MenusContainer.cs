using Godot;

namespace UnnamedGuitarHeroClone.Scripts;

public partial class MenusContainer : PanelContainer
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		OpenMenuScreen();
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
	

	public void OpenMenuScreen()
	{
		var menuScreen = (MenuScreen)ChangeScene("MenuScreen");
		menuScreen.SettingsBtn += OpenSettingsScreen;
		menuScreen.ExitBtn += ExitGame;
		menuScreen.ScoreBoardBtn += OpenScoreboardScreen;
		menuScreen.StartBtn += StartGame;
	}

	private void OpenSettingsScreen()
	{
		var settingsScreen = (SettingsScreen)ChangeScene("SettingsScreen");
		settingsScreen.ReturnBtn += OpenMenuScreen;
	}
	private void OpenScoreboardScreen()
	{
		var  scoreboardScreen = (ScoreboardScreen)ChangeScene("ScoreboardScreen");
		scoreboardScreen.ReturnBtn += OpenMenuScreen;
	}
	
	private void StartGame()
	{
		var main = (Main)GetParent();
		
		main.StartGame();
	}

	private void ExitGame()
	{
		GetParent().GetTree().Quit();
		GD.Print("Exit");
	}
}
