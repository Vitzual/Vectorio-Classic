using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy", menuName = "Enemy/Enemy")]
public class Enemy : Entity
{
    // Enemy stats
    public float damage;
    public float moveSpeed;
    public float explosiveRadius;
    public float explosiveDamage;
    public float rotationSpeed;
    public float spawnChance;

    // Spawn percentage
    //
    // This is what tells the enemy spawner when to spawn this enemy. For example,
    // say the active variant is phantom enemies. The heat value for that variant
    // is 25,000 - 50,000. That means there's 25,000 heat to cover. So, if the
    // player was at 30,000 heat, that would mean they're 20% through the phantom
    // variant. Thus, if the spawn percentage was below that (say 0.1), the system 
    // would try and spawn it (based off whatever it's chance variable is above)

    public float spawnPercentage;

    // Spawn on death
    [System.Serializable]
    public class EnemySpawn
    {
        public Enemy enemy;
        public int amount;
        public float radius;
    }
    public EnemySpawn[] spawnsOnDeath;//

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
