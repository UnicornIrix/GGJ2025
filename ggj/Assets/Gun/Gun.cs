using Godot;
using System;

public partial class Gun : Node2D
{

	[Export]
	PackedScene bullet;

	[Export]
	Marker2D bulletGenPoint;


	public void Shoot(){
		var bulletInstance = bullet.Instantiate();
		 GetTree().Root.GetNode("/world").AddChild(bulletInstance);
	}
}
