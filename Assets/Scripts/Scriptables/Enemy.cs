using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy", menuName = "Enemies/Normal")]
public class Enemy : Entity
{
    // Enemy stats
    public float damage;
    public float moveSpeed;
    public float explosiveRadius;
    public float explosiveDamage;
    public bool useRaycasts;
    public float rotationSpeed;

    // Spawn on death
    [System.Serializable]
    public class EnemySpawn
    {
        public Enemy enemy;
        public int amount;
        public float radius;
    }
    public EnemySpawn[] spawns;

    // Variant
    public Variant variant;

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

    public virtual void MoveTowards(Transform obj, Transform target)
    {
        float step = moveSpeed * Time.deltaTime;
        obj.position = Vector2.MoveTowards(obj.position, target.position, step);
    }
}
