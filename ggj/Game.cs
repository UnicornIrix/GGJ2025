using Godot;
using System;

public partial class Game : Node
{
	
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if(Input.IsActionJustPressed("exit_game"))
			GetTree().Quit();
	}
}
