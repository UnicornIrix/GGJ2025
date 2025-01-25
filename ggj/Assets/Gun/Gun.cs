using Godot;
using System;

public partial class Gun : Node2D
{

	[Export]
	PackedScene bullet;

	[Export]
	Marker2D bulletGenPoint;


	public void Shoot(){
		Node2D bulletInstance = (Node2D)bullet.Instantiate();

		bulletInstance.Position = bulletGenPoint.GlobalPosition;

		GetTree().Root.GetNode("/root/World").AddChild(bulletInstance);
	}
}
