using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueprintHandler : MonoBehaviour
{
    // Get blueprint object /
    public BlueprintObj blueprintObj;

    // Register events and setup dictionary
    public void Start()
    {
        Events.active.onEnemyDestroyed += TrySpawnBlueprint;
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
                    newBlueprint.Setup(blueprint, rarity.rarity);
                    return;
                }
            }
        }
    }
}
