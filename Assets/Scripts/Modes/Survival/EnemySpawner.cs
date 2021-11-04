using UnityEngine;
using Michsky.UI.ModernUIPack;
using TMPro;

public class EnemySpawner : MonoBehaviour
{
    // Active instance
    public float borderSize = 750f;
    public static EnemySpawner active;

    public void Start()
    {
        active = this;
        InvokeRepeating("SpawnEnemies", 1, 1);
    }

    public void SpawnEnemies()
    {
        // Check if enemy handler is active
        if (EnemyHandler.active.variant == null) return;

        // Calculate chance and heat percentage
        Vector2 spawnPos;
        float chance = Random.value;
        float percentage = (float)Resource.active.GetHeat() / (EnemyHandler.active.variant.maxHeat - EnemyHandler.active.variant.minHeat);
        
        // Loop through all enemies
        foreach(Enemy enemy in ScriptableManager.enemies)
        {
            // If spawn percentage is above the chance value, spawn
            if (enemy.spawnPercentage >= percentage && (enemy.spawnChance * (percentage + 1)) >= chance)
            {
                // Get location around border
                if (Random.value > 0.5f)
                {
                    if (Random.value > 0.5f) spawnPos = new Vector2(borderSize, Random.Range(-borderSize, borderSize));
                    else spawnPos = new Vector2(-borderSize, Random.Range(-borderSize, borderSize));
                }
                else
                {
                    if (Random.value > 0.5f) spawnPos = new Vector2(Random.Range(-borderSize, borderSize), borderSize);
                    else spawnPos = new Vector2(Random.Range(-borderSize, borderSize), -borderSize);
                }

                // Create enemy
                InstantiationHandler.active.CreateEnemy(enemy, spawnPos, Quaternion.identity);
            }
        }
    }
}
