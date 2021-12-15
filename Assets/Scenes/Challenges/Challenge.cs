using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Challenge : Gamemode
{
    public Hotbar hotbar;
    public static bool started = false;

    // TEMP
    public float groupMoveSpeed = 5f;
    public GameObject completeUI;
    public GameObject startButton;
    public int[] resources;
    public DefaultEnemy speeder;
    public Vector2 spawnArea;
    public Vector2 guardianSpawn;
    public float speederCooldown = 0.01f;
    public bool complete = false;
    public Guardian guardian;
    public DefaultGuardian activeGuardian;

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
        started = false;

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
        if (speederCooldown <= 0 && activeGuardian.target != null)
        {
            // Set time
            speederCooldown = 0.015f;

            // Create new enemy
            Vector2 position = new Vector2(spawnArea.x + Random.Range(-150f, 150f), spawnArea.y);
            DefaultEnemy newEnemy = Instantiate(speeder, position, Quaternion.identity);

            // Create holder entity
            newEnemy.variant = stage.variant;
            newEnemy.Setup();

            // Set difficulty values
            newEnemy.health *= difficulty.enemyHealthModifier;
            newEnemy.maxHealth = newEnemy.health;
            newEnemy.moveSpeed = 90f;

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

        // Create a new guardian
        activeGuardian = Instantiate(guardian.obj, guardianSpawn, Quaternion.identity).GetComponent<DefaultGuardian>();
        activeGuardian.name = guardian.name;

        // Create holder entity
        activeGuardian.guardian = guardian;
        activeGuardian.variant = stage.variant;
        activeGuardian.Setup();

        // Set difficulty values
        activeGuardian.health = 18000;
        activeGuardian.maxHealth = activeGuardian.health;
        activeGuardian.moveSpeed = groupMoveSpeed;

        // Setup entity
        EnemyHandler.active.enemies.Add(activeGuardian);

        // Assign runtime ID
        Server.AssignRuntimeID(activeGuardian);

        // Start bool
        started = true;
        refundResources = false;
        startButton.SetActive(false);
    }

    // Restart the challenge
    public void RestartChallenge()
    {
        // Reset booleans
        started = false;
        complete = false;
        refundResources = true;
        startButton.SetActive(true);

        // Reset all entities
        EnemyHandler.active.DestroyAllEnemies();
        BaseTile[] baseTiles = FindObjectsOfType<BaseTile>();
        foreach(BaseTile tile in baseTiles)
            InstantiationHandler.active.RpcDestroyBuilding(tile.transform.position);

        // Reset resources
        Resource.active.SetAmount(Resource.CurrencyType.Gold, 10000);
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
