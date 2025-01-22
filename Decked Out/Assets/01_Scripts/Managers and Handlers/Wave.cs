using System;
using System.Collections.Generic;

[Serializable]
public class Wave
{
    public int numberOfEnemies = 5;
    public float timeBetweenEnemies = 2.0f;
    public float timeBetweenWaves = 10.0f;
    public List<EnemySpawnInfo> enemySpawnInfos = new List<EnemySpawnInfo>();
}