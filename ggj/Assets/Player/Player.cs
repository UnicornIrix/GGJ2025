using Godot;
using System;

public partial class Player : CharacterBody2D
{
	[Export]
	Gun gun;

	private const float Speed = 300.0f;

	[Export]
	private int health = 1;

    public override void _Process(double delta)
    {
		if(Input.IsActionPressed("shoot"))
		{
			gun.Shoot();
		}
    }

    public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;

		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.
		Vector2 direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down").Normalized();
		if (direction != Vector2.Zero)
		{
			velocity = direction * Speed;
		}
		else
		{
			velocity = new Vector2(Mathf.MoveToward(Velocity.X, 0, Speed), Mathf.MoveToward(Velocity.Y, 0, Speed));
		}

		Velocity = velocity;
		MoveAndSlide();
	}

	public void Hitted(){
		if(health <= 0)
		{
			Dead();
			return;
		}

		health -= 1;
	}

	public void Dead()
	{
		GetTree().ReloadCurrentScene();
	}
}
