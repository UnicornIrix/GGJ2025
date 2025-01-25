using Godot;
using System;

public partial class Bullet : RigidBody2D
{

	[Export]
	private float speed = 800.0f;


    public void ChangeDir(Vector2 dir){
		LinearVelocity = dir * speed;
	}

	public void OnBodyEntered(Node2D body)
	{
		if(body is Player)
		{
			((Player)body).Hitted();
		}
		else if(body is Enemy){
			((Enemy)body).Hitted();
		}

		QueueFree();
	}

}
