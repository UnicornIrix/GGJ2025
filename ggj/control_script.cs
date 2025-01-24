using Godot;
using System;

public partial class control_script : Control
{

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	
	public void start_game(){

		//PackedScene scenaprova = (PackedScene).Instantiate();
		//GetTree().Root.AddChild(scenaprova);
		GetTree().ChangeSceneToPacked(ResourceLoader.Load<PackedScene>("res://scenaprova.tscn"));
	}
}
