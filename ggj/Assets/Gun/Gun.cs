using Godot;
using System;

public partial class Gun : Node2D
{

	[Export]
	PackedScene bullet;

	[Export]
	Marker2D bulletGenPoint;


	public void Shoot(Vector2 shootDir){
		Node2D bulletInstance = (Node2D)bullet.Instantiate();

		bulletInstance.Position = bulletGenPoint.GlobalPosition;
		((Bullet)bulletInstance).ChangeDir(shootDir);

		GetTree().Root.GetNode("/root/World").AddChild(bulletInstance);
	}
}
