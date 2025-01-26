using Godot;
using System;

public partial class Environment : StaticBody2D
{
	[Export]
	Texture2D environment;

	Sprite2D sprite2D;

	public override void _Ready()
	{
		sprite2D = (Sprite2D)GetNode("sprite2D");
	}

}
