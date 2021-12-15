using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Challenge : Gamemode
{
    public Hotbar hotbar;
    public List<DefaultEnemy> enemies;
    public static bool started = false;

    // Initiate the gamemode
    public override void Initiate()
    {
        // Setup resources
        resource.Setup(perSeconds, currencyElements);
        resource.gameObject.SetActive(true);
    }

    // Setup game
    public override void Setup()
    {
        // Check difficulty variable
        if (difficulty == null)
        {
            Debug.Log("Difficulty data missing. Creating new one");
            difficulty = _difficulty.SetData(new DifficultyData());
        }

        // Check stage variable
        if (stage == null)
        {
            Debug.Log("Stage data missing. Setting to default");
            stage = _stage;
        }

        resource.gameObject.SetActive(true);
        hotbar.SetDefaultSlots();
    }

    // Start gamemode
    public void StartChallenge()
    {
        // Iterate through and setup all entites
        foreach (DefaultEnemy enemy in enemies)
        {
            // Set the gameobject to active
            enemy.gameObject.SetActive(true);

            // Create holder entity
            enemy.variant = stage.variant;
            enemy.Setup();

            // Set difficulty values
            enemy.health *= difficulty.enemyHealthModifier;
            enemy.maxHealth = enemy.health;
            enemy.moveSpeed *= difficulty.enemySpeedModifier;

            // Setup entity
            EnemyHandler.active.enemies.Add(enemy);

            // Assign runtime ID
            Server.AssignRuntimeID(enemy);
        }

        // Start bool
        started = true;
    }
}
