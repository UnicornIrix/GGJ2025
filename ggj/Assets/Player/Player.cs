using Godot;
using System;
using Godot.Collections;

public partial class Player : CharacterBody2D
{
	[Export]
	AnimationTree animTree;

	[Export]
	Gun gun;

	[Export]
	Timer timeBetweenShootTimer;

	[Export]
	private float Speed = 450.0f;

	[Export]
	private int health = 1;

	[Export]
	Dictionary<String, AudioStreamMP3> sounds;

	Vector2 lastDir = Vector2.Right;

	bool canShoot = true;

	AnimationNodeStateMachinePlayback stateMachine;

    private bool isDead;

    public override void _Ready()
	{
		stateMachine = (AnimationNodeStateMachinePlayback)animTree.Get("parameters/playback");
		gun.shootSound = sounds["shoot"];
	}

	public override void _Process(double delta)
	{

		if (Input.IsActionPressed("shoot"))
		{
			if (canShoot)
			{
				gun.Shoot(lastDir);
				canShoot = false;
				timeBetweenShootTimer.Start();
			}
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		if(!isDead)
		{
			Vector2 velocity = Velocity;

			Vector2 direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down").Normalized();
			if (direction != Vector2.Zero)
			{
				velocity = direction * Speed;
				lastDir = direction;
				if (Mathf.Abs(direction.X) > 0 && Mathf.Abs(direction.Y) > 0)
					lastDir.X = 0;

				if(stateMachine.GetCurrentNode() != "Walk")	
				{
					stateMachine.Travel("Walk");
				}	
				animTree.Set("parameters/Walk/blend_position", direction);
			}
			else
			{
				velocity = new Vector2(Mathf.MoveToward(Velocity.X, 0, Speed), Mathf.MoveToward(Velocity.Y, 0, Speed));
				if(stateMachine.GetCurrentNode() != "Idle")	
				{
					stateMachine.Travel("Idle");
				}	
				animTree.Set("parameters/Idle/blend_position", lastDir);
			}

			Velocity = velocity;
			MoveAndSlide();
		}
	}

	public void ShootWaitTimeout()
	{
		canShoot = true;
	}

	public void Hitted()
	{
		health -= 1;
		if (health <= 0)
		{
			stateMachine.Travel("Dead");
			isDead = true;
		}
	}

	public void GameOver(){

		GetTree().CallDeferred("reload_current_scene");
	}

}
