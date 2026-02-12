using System;
using Godot;

public partial class KeybindLine : HBoxContainer
{
	private UnnamedGuitarHeroClone.Scripts.SettingsScreen parentNode;
	private string key;
	private string value;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		
	}

	public void Constructor(string KeyName, string keyBindValue, UnnamedGuitarHeroClone.Scripts.SettingsScreen parent, string displayName)
	{
		var label = (Label)GetNode("KeybindNameLabel");
		label.Text = displayName;
		var button = (Button)GetNode("MarginContainer/ChangeKeybindButton");
		button.Text = keyBindValue;
		key = KeyName;
		value = keyBindValue;
		parentNode = parent;
	}
	
	public void KeyChanged(string newValue)
	{
		value = newValue;
		var button = (Button)GetNode("MarginContainer/ChangeKeybindButton");
		button.Text = newValue;
		parentNode.KeyBindChanged(key, newValue);
	}

	private void OpenPopupReader()
	{
		var packedScene = ResourceLoader.Load<PackedScene>($"res://Scenes/PopupKeyReader.tscn");
		var NodeScene = (PopupKeyReader)packedScene.Instantiate();
		NodeScene.Constructor(this);
		AddChild(NodeScene);
		NodeScene.Popup();
		NodeScene.PopupCentered();
		NodeScene.GrabFocus();     // ensures keyboard input goes to the popup
		NodeScene.Exclusive = true;

	}
}
