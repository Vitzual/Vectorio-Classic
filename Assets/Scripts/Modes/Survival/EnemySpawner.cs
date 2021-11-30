using UnityEngine;
using Michsky.UI.ModernUIPack;
using TMPro;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
    // Active instance
    public List<Enemy> enemies;
    public static EnemySpawner active;
    public int maxEnemiesAllowed = 250;
    public TextMeshProUGUI enemiesAmount;

    // Group spawning
    public NotificationManager groupNotif;
    public AudioSource groupSound;
    public TextMeshProUGUI groupTimer;
    public float groupSpeed = 5f;
    public int groupCooldown = 360;
    private int timeUntilNextGroup;
    private Vector2 groupSpawnPos;
    private int groupEnemies = 0;

    public void Start()
    {
        active = this;
        timeUntilNextGroup = groupCooldown;

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
                // Get position
                Vector2 calcPos = new Vector2(groupSpawnPos.x + Random.Range(-20, 20), groupSpawnPos.y + Random.Range(-20, 20));
                EnemyHandler.active.CreateEntity(enemy, Gamemode.stage.variant, calcPos, Quaternion.identity, -1, groupSpeed);

                // Lower group spawn value
                groupEnemies -= 1;
            }
        }
    }

    public void SpawnEnemies()
    {
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
        enemiesAmount.text = "<b>ENEMIES ATTACKING:</b> " + EnemyHandler.active.enemies.Count;
    }

    // Check if group spawning still active
    public void CheckGroupSpawning()
    {
        if (!Gamemode.active.useGroupSpawning || Settings.paused) return;

        timeUntilNextGroup -= 1;

        if (timeUntilNextGroup <= 0)
        {
            // Get location around border
            if (Random.value > 0.5f)
            {
                if (Random.value > 0.5f)
                {
                    groupSpawnPos = new Vector2(Border.west, Random.Range(Border.south, Border.north));
                    groupNotif.description = "Attack coming from <b>West</b>";
                }
                else
                {
                    groupSpawnPos = new Vector2(Border.east, Random.Range(Border.south, Border.north));
                    groupNotif.description = "Attack coming from <b>East</b>";
                }
            }
            else
            {
                if (Random.value > 0.5f)
                {
                    groupSpawnPos = new Vector2(Random.Range(Border.west, Border.east), Border.north);
                    groupNotif.description = "Attack coming from <b>North</b>";
                }
                else
                {
                    groupSpawnPos = new Vector2(Random.Range(Border.west, Border.east), Border.south);
                    groupNotif.description = "Attack coming from <b>South</b>";
                }
            }

            // Get amount to spawn
            groupEnemies = Resource.active.GetHeat() / 1000;
            if (groupEnemies < 20) groupEnemies = 20;
            else if (groupEnemies > 100) groupEnemies = 100;
            timeUntilNextGroup = groupCooldown;
            groupSpeed = 5f * Gamemode.stage.variant.speedModifier;

            // Display group notification
            if (groupNotif != null)
            {
                groupNotif.UpdateUI();
                groupNotif.OpenNotification();
            }
            if (groupSound != null)
            {
                groupSound.volume = Settings.sound;
                groupSound.Play();
            }
        }

        // Update attacking enemies
        groupTimer.text = "<b>NEXT GROUP ATTACK:</b> " + timeUntilNextGroup + "s";
    }
}
