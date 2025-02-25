using Godot;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

public partial class Enemy : CharacterBody2D
{
	[Export]
	AudioStreamMP3 shootSound;

	[Export]
	AnimationTree animTree;

	[Export]
	Gun gun;

	[Export]
	Timer timeBetweenShootTimer;

	[Export]
	Timer attackSubStateTimer;

	[Export]
	private float Speed = 200.0f;

	[Export]
	private int health = 1;

	[Export]
	private float shootRange = 600;

	bool canShoot = true;
	bool canAttackSubState = true;

	Player playerTarget;

	enum States
	{
		Normal,
		Chase,
		Attack
	}

	States state = States.Normal;

	enum AttackStates
	{
		ShootAttack,
		MoveAttack
	}

	AttackStates attackState;

	Vector2 attackMoveDir;
	
	Vector2 lastDir;

	bool isOnCamera;

	bool isDead;

	AnimationNodeStateMachinePlayback stateMachine;

	SpawnPointHandler spawnPointHandler;

	public SpawnPointHandler SpawnPointHandler
	{
		get
		{
			return spawnPointHandler;
		}
		set
		{
			spawnPointHandler = value;
		}
	}

	public override void _Ready()
	{
		spawnPointHandler?.AddCurrentAliveEnemy();
		stateMachine = (AnimationNodeStateMachinePlayback)animTree.Get("parameters/playback");
		gun.shootSound = shootSound;
	}

	public override void _PhysicsProcess(double delta)
	{
		if(!isDead)
		{
			switch (state)
			{
				case States.Normal:
					NormalState();
					break;
				case States.Chase:
					ChaseState();
					break;
				case States.Attack:
					{
						AttackStateAsync();
						break;
					}
			}
		}
	}

	public void Move(Vector2 direction)
	{
		Vector2 velocity = Velocity;

		if (direction != Vector2.Zero)
		{
			velocity = direction * Speed;
			lastDir = direction;

		}
		else
		{
			velocity = new Vector2(Mathf.MoveToward(Velocity.X, 0, Speed), Mathf.MoveToward(Velocity.Y, 0, Speed));
		}

		Velocity = velocity;
		if(stateMachine.GetCurrentNode() != "Idle")	
		{
			stateMachine.Travel("Idle");
		}	
		animTree.Set("parameters/Idle/blend_position", lastDir);
		MoveAndSlide();
	}

	public void NormalState()
	{
		//TODO: Move(Random direction)
	}

	public void ChaseState()
	{
		if (playerTarget == null)
			return;

		if (isOnCamera)
		{
			if (this.GlobalPosition.DistanceTo(playerTarget.GlobalPosition) <= shootRange)
			{
				ChangeState(States.Attack);
			}
		}

		Move(this.GlobalPosition.DirectionTo(playerTarget.GlobalPosition));
	}

	public async void AttackStateAsync()
	{
		if (playerTarget == null)
		{
			return;
		}

		if (!isOnCamera || this.GlobalPosition.DistanceTo(playerTarget.GlobalPosition) > shootRange)
		{
			await ToSignal(GetTree().CreateTimer(0.5f), "timeout");
			if (playerTarget != null)
			{
				if (this.GlobalPosition.DistanceTo(playerTarget.GlobalPosition) > shootRange)
				{
					ChangeState(States.Chase);
					return;
				}
			}
		}

		if (canAttackSubState)
		{
			Random random = new Random();

			canAttackSubState = false;
			attackSubStateTimer.Start();

			if (random.NextDouble() < 0.5)
			{
				attackState = AttackStates.MoveAttack;
				double randomN = random.NextDouble();

				if (randomN < 0.25)
					attackMoveDir = Vector2.Left;
				else if (randomN < 0.5)
					attackMoveDir = Vector2.Right;
				else if (randomN < 0.75)
					attackMoveDir = Vector2.Up;
				else
					attackMoveDir = Vector2.Down;
			}
			else
				attackState = AttackStates.ShootAttack;
		}
		else
		{
			if (attackState == AttackStates.MoveAttack)
				AttackMoveState();
		}


		if (attackState == AttackStates.ShootAttack)
			AttackShootState();
	}

	public void AttackShootState()
	{
		if (playerTarget == null)
			return;

		if (canShoot)
		{
			gun.Shoot(this.GlobalPosition.DirectionTo(playerTarget.GlobalPosition));
			canShoot = false;
			timeBetweenShootTimer.Start();
		}
	}

	public void AttackMoveState()
	{

		Move(attackMoveDir);
	}

	public void ShootWaitTimeout()
	{
		canShoot = true;
	}

	public void AttackSubStateTimerTimeout()
	{
		canAttackSubState = true;
	}

	public void Hitted()
	{
		health -= 1;

		if (health <= 0)
		{
			isDead = true;
			stateMachine.Travel("Dead");
		}

	}

	public void Area2DBodyEntered(Node2D body)
	{
		if (body is Player)
		{
			playerTarget = (Player)body;
			ChangeState(States.Chase);
		}
	}

	public void Area2DBodyExited(Node2D body)
	{
		if (body is Player)
		{
			playerTarget = null;
			ChangeState(States.Normal);
		}
	}

	public void EnteredCameraViewport()
	{
		isOnCamera = true;
	}

	public void ExitedCameraViewport()
	{
		isOnCamera = false;
	}

	public void Dead()
	{

		spawnPointHandler?.SubtractCurrentAliveENemy();
		QueueFree();

	}

	private void ChangeState(States newState)
	{
		state = newState;
		// await
	}
}
