using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Challenge : Gamemode
{
    public Hotbar hotbar;
    public List<DefaultEnemy> enemies;
    public static bool started = false;

    // TEMP
    public float groupMoveSpeed = 5f;
    public GameObject completeUI;
    public GameObject startButton;
    public int[] resources;
    public DefaultEnemy speeder;
    public Vector2 spawnArea;
    public float speederCooldown = 0.01f;
    public bool complete = false;
    public DefaultGuardian guardian;

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

        // Enable resource object
        resource.gameObject.SetActive(true);

        // TEMPORARY METHOD
        for(int a = 0; a < hotbar.defaultEntities.Length; a++)
        {
            Entity entity = hotbar.defaultEntities[a];
            if (Buildables.active.ContainsKey(entity))
            {
                for(int b = 0; b < Buildables.active[entity].resources.Length; b++)
                {
                    if (Buildables.active[entity].resources[b].type == Resource.CurrencyType.Gold)
                        Buildables.active[entity].resources[b].amount = resources[a];
                }
            }
        }

        // Set default slots
        hotbar.SetDefaultSlots();

        // Disable online connection
        NetworkManagerSF.active.maxConnections = 1;

        // Setup event
        Events.active.onGuardianDestroyed += OnGuardianDestroyed;
    }

    // Override update method
    public override void UpdateMethod()
    {
        // Check if active
        if (!started || complete || Settings.paused) return;

        // Check cooldown
        speederCooldown -= Time.deltaTime;
        if (speederCooldown <= 0 && guardian.target != null)
        {
            // Set time
            speederCooldown = 0.02f;

            // Create new enemy
            Vector2 position = new Vector2(spawnArea.x + Random.Range(-150f, 150f), spawnArea.y);
            DefaultEnemy newEnemy = Instantiate(speeder, position, Quaternion.identity);

            // Create holder entity
            newEnemy.variant = stage.variant;
            newEnemy.Setup();

            // Set difficulty values
            newEnemy.health *= difficulty.enemyHealthModifier;
            newEnemy.maxHealth = newEnemy.health;
            newEnemy.moveSpeed = 80f;

            // Setup entity
            EnemyHandler.active.enemies.Add(newEnemy);

            // Assign runtime ID
            Server.AssignRuntimeID(newEnemy);
        }
    }

    // Start gamemode
    public void StartChallenge()
    {
        // Check if started
        if (started) return;

        // Set the gameobject to active
        guardian.gameObject.SetActive(true);

        // Create holder entity
        guardian.variant = stage.variant;
        guardian.Setup();

        // Set difficulty values
        guardian.health *= difficulty.enemyHealthModifier;
        guardian.maxHealth = guardian.health;
        guardian.moveSpeed = groupMoveSpeed;

        // Setup entity
        EnemyHandler.active.enemies.Add(guardian);

        // Assign runtime ID
        Server.AssignRuntimeID(guardian);

        // Start bool
        started = true;
        refundResources = false;
        startButton.SetActive(false);
    }

    // On guardian destroyed, you win!
    public void OnGuardianDestroyed(DefaultGuardian guardian)
    {
        Debug.Log("Guardian destroyed, rewarding achievement");
        complete = true;
        completeUI.SetActive(true);
        Achievements.Unlock("PEPPERMINT_TURRET_01");
    }
}
