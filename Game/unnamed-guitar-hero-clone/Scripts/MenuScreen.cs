using Godot;



public partial class MenuScreen : Control
{
	[Signal]
	public delegate void StartBtnEventHandler();
	[Signal]
	public delegate void SettingsBtnEventHandler();
	[Signal]
	public delegate void ScoreBoardBtnEventHandler();
	[Signal]
	public delegate void ExitBtnEventHandler();
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		
	}

	private void OnStartButtonPressed()
	{
		EmitSignalStartBtn();
	}

	private void OnSettingsButtonPressed()
	{
		EmitSignalSettingsBtn();
	}

	private void OnLeaderboardButtonPressed()
	{
		EmitSignalScoreBoardBtn();
	}

	private void OnExitButtonPressed()
	{
		EmitSignalExitBtn();
	}
	
}
