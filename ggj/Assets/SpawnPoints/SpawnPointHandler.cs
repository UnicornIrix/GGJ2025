using Godot;
using System;
using System.Collections.Generic;

public partial class SpawnPointHandler : Node2D
{

	[Export]
	PackedScene spawnScene;

	[Export]
	Vector2 mapSize;

	[Export]
	int spawnsNum;

	[Export]
	bool hasMaxEnemyNum;

	[Export]
	int maxEnemyTotalNum;

	[Export]
	int maxEnemyCurrentNum;
	
	
	int spawnedEnemies=0;

	int enemyCurrentNum=0;

	List<Node2D> spawns = new List<Node2D>();

	Random random = new Random();
	
	public override void _Ready()
	{
		for(int i=0; i < spawnsNum; i++)
		{
			Vector2 randPos = new Vector2((float)random.NextDouble() * mapSize.X, (float)random.NextDouble() * mapSize.Y);

			if(i > 0)
			{
				while(randPos.DistanceTo(spawns[i-1].Position) > 500)
				{
					randPos = new Vector2((float)random.NextDouble() * mapSize.X, (float)random.NextDouble() * mapSize.Y);
				}
			}

			Node2D spawnInstance = (Node2D)spawnScene.Instantiate();
			spawnInstance.Position = randPos;
			AddChild(spawnInstance); 
			spawns.Add(spawnInstance);
		}
	}

    public override void _Process(double delta)
    {
        if(enemyCurrentNum < maxEnemyCurrentNum)
		{
			int spawnToUse = random.Next(0, spawns.Count);

			((Spawn)spawns[spawnToUse]).CreateEnemy();
			spawnedEnemies += 1; 
			
		}
    }

	public void AddCurrentAliveEnemy()
	{
		enemyCurrentNum += 1;
	}

	public void SubtractCurrentAliveENemy()
	{
		enemyCurrentNum -= 1;
	}

}
