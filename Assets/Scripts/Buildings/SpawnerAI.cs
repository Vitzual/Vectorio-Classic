using UnityEngine;
using System.Collections;

public class SpawnerAI : EnemyClass
{
    // Units to spawn
    public Transform[] enemies;

    // Spawn enemy 
    public void SpawnEnemy()
    {
        for (int i = 0; i < enemies.Length; i++)
            Instantiate(enemies[i], new Vector2(transform.position.x + Random.Range(-10, 10), transform.position.y + Random.Range(-10, 10)), Quaternion.Euler(new Vector3(0, 0, 0)));
    }
}
