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

		//PackedScene scenaprova = ResourceLoader.Load<PackedScene>("res://scenaprova.tscn").Instantiate();
		//GetTree().Root.AddChild(scenaprova);
	}
}
