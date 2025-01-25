using Godot;
using System;

public partial class Player : CharacterBody2D
{
	[Export]
	AnimationTree animTree;

	[Export]
	Gun gun;

	[Export]
	Timer timeBetweenShootTimer;

	private const float Speed = 300.0f;

	[Export]
	private int health = 1;

	Vector2 lastDir = Vector2.Right;

	bool canShoot = true;

    public override void _Process(double delta)
    {
		if(Input.IsActionPressed("shoot"))
		{
			if(canShoot){
				gun.Shoot(lastDir);
				canShoot = false;
				timeBetweenShootTimer.Start();
			}
		}
    }

    public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;

		
		Vector2 direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down").Normalized();
		if (direction != Vector2.Zero)
		{
			velocity = direction * Speed;
			lastDir = direction;
			if(Mathf.Abs(direction.X) > 0 && Mathf.Abs(direction.Y) > 0)
				lastDir.X = 0;
		}
		else
		{
			velocity = new Vector2(Mathf.MoveToward(Velocity.X, 0, Speed), Mathf.MoveToward(Velocity.Y, 0, Speed));
		}

		Velocity = velocity;
		animTree.Set("parameters/blend_position", velocity);
		MoveAndSlide();
	}

	public void ShootWaitTimeout(){
		canShoot = true;
	}

	public void Hitted(){
		health -= 1;

		if(health <= 0)
			Dead();

	}

	public void Dead()
	{
		// TODO: Game Over
		GetTree().CallDeferred("reload_current_scene");
	}
	
}
