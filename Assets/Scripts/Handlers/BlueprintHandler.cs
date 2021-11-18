using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueprintHandler : MonoBehaviour
{
    // Active blueprint class
    public class ActiveBlueprint
    {
        // Constructor
        public ActiveBlueprint(BlueprintObj blueprint)
        {
            this.blueprint = blueprint;
            timer = 60f;
        }

        public BlueprintObj blueprint;
        public float timer;
    }

    // Get blueprint object
    public BlueprintObj blueprintObj;
    public List<ActiveBlueprint> activeBlueprints;
    public List<Blueprint> collectedBlueprints;

    // Register events and setup dictionary
    public void Start()
    {
        Events.active.onEnemyDestroyed += TrySpawnBlueprint;
    }

    // Update active blueprints
    public void Update()
    {
        if (!Settings.paused)
        {
            for(int i = 0; i < activeBlueprints.Count; i++)
            {
                if (activeBlueprints[i].blueprint != null)
                {
                    activeBlueprints[i].timer -= Time.deltaTime;
                    if (activeBlueprints[i].timer <= 0f)
                    {
                        Destroy(activeBlueprints[i].blueprint);
                        activeBlueprints.RemoveAt(i);
                        i--;
                    }
                }
                else
                {
                    activeBlueprints.RemoveAt(i);
                    i--;
                }
            }
        }
    }

    // Iterate through blueprint table and try to spawn 
    public void TrySpawnBlueprint(DefaultEnemy enemy)
    {
        // Check if blueprint spawning is enabled
        if (!Gamemode.active.spawnBlueprints) return;

        // Grab list of drops
        List<Blueprint> blueprints = enemy.enemy.drops;
        
        // Loop variables
        float random;

        // Iterate through blueprints
        foreach(Blueprint blueprint in blueprints)
        {
            // Generate random value
            random = Random.value;

            // Iterate through blueprints (highest to lowest rarity)
            foreach (Blueprint.Rarity rarity in blueprint.rarities)
            {
                // If value lower then drop chance, spawn and return
                if (random < rarity.dropChance)
                {
                    BlueprintObj newBlueprint = Instantiate(blueprintObj, enemy.transform.position, Quaternion.identity).GetComponent<BlueprintObj>();
                    activeBlueprints.Add(new ActiveBlueprint(newBlueprint));
                    newBlueprint.Setup(blueprint, rarity);
                    return;
                }
            }
        }
    }
}
