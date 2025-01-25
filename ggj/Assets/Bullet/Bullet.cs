using Godot;
using System;

public partial class Bullet : RigidBody2D
{

	[Export]
	private float speed = 800.0f;



	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

    public override void _PhysicsProcess(double delta)
    {

    }

    public void ChangeDir(Vector2 dir){
		LinearVelocity = dir * speed;
	}
}
