using UnityEngine;
using Mirror;
using Michsky.UI.ModernUIPack;
using TMPro;
using System.Collections.Generic;

public class EnemySpawner : NetworkBehaviour
{
    // Active instance
    public List<Enemy> enemies;
    public int maxEnemiesAllowed = 350;

    // Group spawning
    public float groupAmount = 1f;
    public float groupSpeed = 5f;
    private bool difficultySet = false;
    public int groupCooldown = 360;
    private int timeUntilNextGroup;
    private Vector2 groupSpawnPos;
    private int groupEnemies = 0;

    public void Start()
    {
        // Run authority check
        if (!hasAuthority) return;
        Debug.Log("[SERVER] Authority granted to " + transform.name + " for enemy spawning");

        // Setup spawner
        if (!difficultySet) timeUntilNextGroup = groupCooldown;
        enemies.AddRange(ScriptableLoader.enemies.Values);

        InvokeRepeating("SpawnEnemies", 1, 1);
        InvokeRepeating("CheckGroupSpawning", 0.5f, 1);

        Events.active.onSetEnemyDifficulty += SetDifficulty;
        Events.active.onEnemySpawnOnDeath += SyncDeathCreateEnemy;
    }

    public void SetDifficulty(float rate, float size)
    {
        groupCooldown = (int)((float)groupCooldown / rate);
        timeUntilNextGroup = groupCooldown;
        groupAmount = size;
        difficultySet = true;
    }

    // Internal enemy spawn call
    public void SyncDeathCreateEnemy(Enemy enemy, Vector2 position)
    {
        if (hasAuthority)
        {
            foreach (Enemy.EnemySpawn spawn in enemy.spawnsOnDeath)
            {
                if (spawn.enemy == enemy) continue;

                for (int i = 0; i < spawn.amount; i++)
                {
                    Vector2 spawnPos = new Vector2(position.x + Random.Range(0, spawn.radius), position.y + Random.Range(0, spawn.radius));
                    CreateEnemy(spawn.enemy.InternalID, Gamemode.stage.variant.InternalID, spawnPos, Quaternion.identity, -1, -1);
                }
            }
        }
    }

    [Command]
    public void CreateEnemy(string enemy_id, string variant_id, Vector2 pos, Quaternion rotation, float health, float speed)
    {
        if (!hasAuthority) return;
        Server.active.SrvSyncEnemy(enemy_id, variant_id, pos, rotation, health, speed);
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
                // Get position
                Vector2 calcPos = new Vector2(groupSpawnPos.x + Random.Range(-20, 20), groupSpawnPos.y + Random.Range(-20, 20));
                CreateEnemy(enemy.InternalID, Gamemode.stage.variant.InternalID, calcPos, Quaternion.identity, -1, groupSpeed);

                // Lower group spawn value
                groupEnemies -= 1;
            }
        }
    }

    public void SpawnEnemies()
    {
        // Check authority
        if (!hasAuthority) return;

        // Check if enemy handler is active
        if (EnemyHandler.active.enemies.Count > maxEnemiesAllowed || Settings.paused) return;

        // Setup variables
        Vector2 spawnPos;
        float chance;

        // Calculate chance (or ignore)
        chance = Random.value;

        // Calculate heat percentage
        float percentage;
        if (Gamemode.stage.infinite) percentage = 1;
        else if (Gamemode.stage.heat > 0) percentage = (float)Resource.active.GetHeat() / Gamemode.stage.heat;
        else percentage = 1;

        // Loop through all enemies
        foreach(Enemy enemy in enemies)
        {
            // If spawn percentage is above the chance value, spawn
            float chanceCalculation = enemy.spawnChance * Gamemode.difficulty.enemySpawnrateModifier;
            if (chanceCalculation < 0.15f) chanceCalculation = 0.15f;

            if (enemy.spawnPercentage <= percentage && chanceCalculation >= chance)
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
                CreateEnemy(enemy.InternalID, Gamemode.stage.variant.InternalID, spawnPos, Quaternion.identity, -1, -1);
            }
        }

        // Update attacking enemies
        // enemiesAmount.text = "<b>ENEMIES ATTACKING:</b> " + EnemyHandler.active.enemies.Count;
    }

    // Check if group spawning still active
    public void CheckGroupSpawning()
    {
        if (!Gamemode.active.useGroupSpawning || Settings.paused) return;

        timeUntilNextGroup -= 1;

        if (timeUntilNextGroup <= 0)
        {
            // Create new group notif string
            string groupNotif;

            // Get location around border
            if (Random.value > 0.5f)
            {
                if (Random.value > 0.5f)
                {
                    groupSpawnPos = new Vector2(Border.west, Random.Range(Border.south, Border.north));
                    groupNotif = "Attack coming from <b>West</b>";
                }
                else
                {
                    groupSpawnPos = new Vector2(Border.east, Random.Range(Border.south, Border.north));
                    groupNotif = "Attack coming from <b>East</b>";
                }
            }
            else
            {
                if (Random.value > 0.5f)
                {
                    groupSpawnPos = new Vector2(Random.Range(Border.west, Border.east), Border.north);
                    groupNotif = "Attack coming from <b>North</b>";
                }
                else
                {
                    groupSpawnPos = new Vector2(Random.Range(Border.west, Border.east), Border.south);
                    groupNotif = "Attack coming from <b>South</b>";
                }
            }

            // Get amount to spawn
            groupEnemies = (int)((Resource.active.GetHeat() / 1000) * groupAmount);
            if (groupEnemies < 20) groupEnemies = 20;
            else if (groupEnemies > 250) groupEnemies = 250;
            timeUntilNextGroup = groupCooldown;
            groupSpeed = 5f * Gamemode.stage.variant.speedModifier;

            // Display group notification
            Events.active.EnemyGroupSpawned(groupNotif);
        }
    }
}
