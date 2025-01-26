using Godot;
using System;
using Godot.Collections;
using System.Collections.Generic;

public partial class SpawnPointHandler : Node2D
{

	[Export]
	PackedScene spawnScene;

	[Export]
	Vector2 mapSize;

	[Export]
	int spawnsNum;

	public int SpawnsNum {
		get{return spawnsNum;}
		set{
			if(value > 8)
			{
				spawnsNum = 8;
			}
			else
				spawnsNum = value;
		}
	}

	[Export]
	bool hasMaxEnemyNum;

	[Export]
	int maxEnemyTotalNum;

	[Export]
	int maxEnemyCurrentNumAlive;

	[Export]
	Array<Vector2> spawnPositions;
	
	
	int totalSpawnedEnemies=0;

	int enemyCurrentNum=0;

	List<Node2D> spawns = new List<Node2D>();

	Random random = new Random();
	
	public override void _Ready()
	{

		// Spawn spawns with distance of 500
		for(int i=0; i < spawnsNum; i++)
		{
			int spawnPoint = random.Next(0, spawnPositions.Count);

			Node2D spawnInstance = (Node2D)spawnScene.Instantiate();
			spawnInstance.Position = spawnPositions[spawnPoint];
			AddChild(spawnInstance); 

			spawns.Add(spawnInstance);
			spawnPositions.Remove(spawnPositions[spawnPoint]);
		}
	}

    public override void _Process(double delta)
    {
        if(enemyCurrentNum < maxEnemyCurrentNumAlive)
		{
			if(hasMaxEnemyNum)
			{
				if(totalSpawnedEnemies < maxEnemyTotalNum)
				{
					int spawnToUse = random.Next(0, spawns.Count);
					if(!((Spawn)spawns[spawnToUse]).isOnScreen)
					{
						((Spawn)spawns[spawnToUse]).CreateEnemy();
						totalSpawnedEnemies += 1;
					}
				}
			}
			else
			{
				int spawnToUse = random.Next(0, spawns.Count);

				if(!((Spawn)spawns[spawnToUse]).isOnScreen)
				{
					((Spawn)spawns[spawnToUse]).CreateEnemy();
					
				}
			}
		}
    }

	public void AddCurrentAliveEnemy()
	{
		enemyCurrentNum += 1;
	}

	public void SubtractCurrentAliveENemy()
	{
		enemyCurrentNum -= 1;
		GD.Print("Died");
	}

}
