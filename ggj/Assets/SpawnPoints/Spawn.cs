using Godot;
using System;

public partial class Spawn : Node2D
{

	[Export]
	PackedScene enemyScene;

    public bool isOnScreen { get; private set; } = false;

    public void CreateEnemy()
	{
		Node2D enemySceneInstance = (Node2D)enemyScene.Instantiate();
		enemySceneInstance.GlobalPosition = GlobalPosition;
		
		((Enemy)enemySceneInstance).SpawnPointHandler = (SpawnPointHandler)GetParent();

		GetTree().Root.GetNode("/root/World").AddChild(enemySceneInstance);
	}

	public void EnteredOnScreen()
	{
		isOnScreen = true;
	}

	public void ExitedOffScreen()
	{
		isOnScreen = false;
	}

}
