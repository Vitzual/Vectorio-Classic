using UnityEngine;
using Michsky.UI.ModernUIPack;
using TMPro;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
    // Active instance
    public List<Enemy> enemies;
    public static EnemySpawner active;
    public TextMeshProUGUI enemiesAmount;
    public int maxEnemiesAllowed = 250;

    // Group spawning
    public TextMeshProUGUI groupTimer;
    public float groupSpeed = 5f;
    public float timeUntilNextGroup = 360f;
    public Vector2 groupSpawnPos;
    public int groupEnemies = 0;

    public void Start()
    {
        active = this;
        InvokeRepeating("SpawnEnemies", 1, 1);
        InvokeRepeating("CheckGroupSpawning", 0.5f, 1);

        enemies.AddRange(ScriptableLoader.enemies.Values);
    }

    // Spawn group enemy each frame to offset calculation cost
    public void Update()
    {
        // If enemies still need to spawn, spawn them (duh)
        if (groupEnemies > 0)
        {
            // Get random enemy
            Enemy enemy = enemies[Random.Range(0, enemies.Count)];

            // If enemy is not a large enemy, spawn it
            if (!enemy.largeEnemy)
            {
                Vector2 calcPos = new Vector2(groupSpawnPos.x + Random.Range(-20, 20), groupSpawnPos.y + Random.Range(-20, 20));
                EnemyHandler.active.CreateEntity(enemy, Gamemode.stage.variant, calcPos, Quaternion.identity);
            }

            // Lower group spawn value
            groupEnemies -= 1;
        }
    }

    public void SpawnEnemies()
    {
        // Check if enemy handler is active
        if (EnemyHandler.active.enemies.Count > maxEnemiesAllowed) return;

        // Setup variables
        Vector2 spawnPos;
        float chance;

        // Calculate chance (or ignore)
        if (Debugger.active.ignoreSpawnValues) chance = 0f;
        else chance = Random.value;

        // Calculate heat percentage
        float percentage;
        if (Gamemode.stage.heat > 0) percentage = (float)Resource.active.GetHeat() / Gamemode.stage.heat;
        else percentage = 1;

        // Loop through all enemies
        foreach(Enemy enemy in enemies)
        {
            // If spawn percentage is above the chance value, spawn
            if (enemy.spawnPercentage <= percentage && (enemy.spawnChance * (percentage + 1)) >= chance)
            {
                // Get location around border
                if (Random.value > 0.5f)
                {
                    if (Random.value > 0.5f) spawnPos = new Vector2(Border.west, Random.Range(Border.south, Border.north));
                    else spawnPos = new Vector2(Border.east, Random.Range(Border.south, Border.north));
                }
                else
                {
                    if (Random.value > 0.5f) spawnPos = new Vector2(Random.Range(Border.west, Border.east), Border.north);
                    else spawnPos = new Vector2(Random.Range(Border.west, Border.east), Border.south);
                }

                // Create enemy
                EnemyHandler.active.CreateEntity(enemy, Gamemode.stage.variant, spawnPos, Quaternion.identity);
            }
        }

        // Update attacking enemies
        enemiesAmount.text = "<b>ENEMIES ATTACKING:</b> " + enemies.Count;
    }

    // Check if group spawning still active
    public void CheckGroupSpawning()
    {
        timeUntilNextGroup -= Time.deltaTime;

        if (timeUntilNextGroup <= 0)
        {
            // Get location around border
            if (Random.value > 0.5f)
            {
                if (Random.value > 0.5f) groupSpawnPos = new Vector2(Border.west, Random.Range(Border.south, Border.north));
                else groupSpawnPos = new Vector2(Border.east, Random.Range(Border.south, Border.north));
            }
            else
            {
                if (Random.value > 0.5f) groupSpawnPos = new Vector2(Random.Range(Border.west, Border.east), Border.north);
                else groupSpawnPos = new Vector2(Random.Range(Border.west, Border.east), Border.south);
            }

            groupEnemies = Resource.active.GetHeat() / 1000;
            if (groupEnemies < 20) groupEnemies = 20;
            else if (groupEnemies > 100) groupEnemies = 100;
            timeUntilNextGroup = 360f;
        }

        // Update attacking enemies
        groupTimer.text = "<b>NEXT GROUP ATTACK:</b> " + (int)timeUntilNextGroup + "s";
    }
}
