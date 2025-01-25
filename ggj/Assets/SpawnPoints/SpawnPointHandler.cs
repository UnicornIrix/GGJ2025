using Godot;
using System;

public partial class SpawnPointHandler : Node2D
{
	[Export]
	Vector2 mapSize;

	[Export]
	int spawnsNum;

	[Export]
	int maxEnemyNum;
	
	public override void _Ready()
	{
	}

}
