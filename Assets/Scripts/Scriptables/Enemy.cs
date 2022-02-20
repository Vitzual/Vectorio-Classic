using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy", menuName = "Vectorio/Enemy/Enemy")]
public class Enemy : Entity
{
    // Enemy stats
    public List<Varian> _variants;
    private Dictionary<string, Variant> variants;

    // Spawn percentage
    //
    // This is what tells the enemy spawner when to spawn this enemy. For example,
    // say the active variant is phantom enemies. The heat value for that variant
    // is 25,000 - 50,000. That means there's 25,000 heat to cover. So, if the
    // player was at 30,000 heat, that would mean they're 20% through the phantom
    // variant. Thus, if the spawn percentage was below that (say 0.1), the system 
    // would try and spawn it (based off whatever it's chance variable is above)

    [FoldoutGroup("Enemy Stats")]
    public float spawnPercentage;

    // Spawn on death
    [System.Serializable]
    public class EnemySpawn
    {
        public Enemy enemy;
        public int amount;
        public float radius;
    }
    [FoldoutGroup("Enemy Stats")]
    public EnemySpawn[] spawnsOnDeath;

    // Loot table
    [FoldoutGroup("Enemy Stats")]
    public List<Blueprint> drops;

    // Generate variants on runtime
    public void GenerateVariants()
    {

    }

    // Set panel stats
    // This gets used to set the stats on the building menu panel
    public override void CreateStats(Panel panel)
    {
        panel.CreateStat(new Stat("Health", health, 0, Sprites.GetSprite("Health")));
        panel.CreateStat(new Stat("Damage", damage, 0, Sprites.GetSprite("Damage")));
        panel.CreateStat(new Stat("Speed", moveSpeed, 0, Sprites.GetSprite("Rotation Speed")));

        // Base method
        base.CreateStats(panel);
    }
}
