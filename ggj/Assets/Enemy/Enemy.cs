using Godot;
using System;
using System.Diagnostics;

public partial class Enemy : CharacterBody2D
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

	[Export]
	private float shootRange = 600;

	Vector2 lastDir = Vector2.Right;

	bool canShoot = true;

	Player playerTarget;

	enum States {
		Normal,
		Chase,
		Attack
	}

	States state = States.Attack;

    public override void _PhysicsProcess(double delta)
	{
		switch(state){
			case States.Normal:
				NormalState();
				break;
			case States.Chase:
				ChaseState();
				break;
			case States.Attack:
				AttackState();
				break;
		}
	}

	public void Move(Vector2 direction){
		Vector2 velocity = Velocity;

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

	public void NormalState(){
		//TODO: Move(Random direction)
	}

	public void ChaseState(){
		if(playerTarget == null)
			return;

		if(this.GlobalPosition.DistanceTo(playerTarget.GlobalPosition) <= shootRange)
		{
			state = States.Attack;
		}

		// TODO: Move(to player direction)
	}

	public void AttackState(){
		if (playerTarget == null)
			return;

		Random random = new Random();

		if(random.NextDouble() > 0.2){
			if(canShoot)
			{
				GD.Print(state);
				gun.Shoot(this.GlobalPosition.DirectionTo(playerTarget.GlobalPosition));
				canShoot = false;
				timeBetweenShootTimer.Start();
			}
		}
		else
		{
			
		}
	}

	public void ShootWaitTimeout(){
		canShoot = true;
	}

	public void Hitted(){
		if(health <= 0)
		{
			Dead();
			return;
		}

		health -= 1;
	}

	public void Area2DBodyEntered(Node2D body){
		if(body is Player)
		{
			playerTarget = (Player)body;
			state = States.Chase;
		}
	}

	public void Area2DBodyExited(Node2D body){
		if(body is Player)
		{
			playerTarget = null;
			state = States.Normal;
		}
	}

	public void Dead()
	{
		// TODO: Kill enemy
		
	}
	
}
