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
		enemySceneInstance.Position = Position;
		if(GetParent() is SpawnPointHandler handler)
			((Enemy)enemySceneInstance).SpawnPointHandler = handler;

		AddChild(enemySceneInstance);
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
