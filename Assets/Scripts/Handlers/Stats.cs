using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Stats class
//
// Central point for all scripts that affect
// a buildings stats. I know there are smarter
// ways to do this, but this is the most simple
// way I can think of for the time being

public class Stats
{
    public class BuildingStats
    {
        // Set variables
        public void BaseSet(Building building)
        {
            health = building.health;
            costModifier = 1f;
            powerModifier = 1f;
            heatModifier = 1f;
        }

        // Base turret stat variables
        public int health;
        public float costModifier;
        public float powerModifier;
        public float heatModifier;
    }

    public class CollectorStats : BuildingStats
    {
        // Set variables
        public void Set(Collector collector)
        {
            rate = collector.rate;
            amount = collector.amount;
            storage = collector.storage;

            BaseSet(collector);
        }

        // Collector variables
        public float rate;
        public int amount;
        public int storage;
        public bool auto;
    }

    public class DefenseStats : BuildingStats
    {
        // Set variables
        public void Set(Turret turret)
        {
            damage = turret.damage;
            range = turret.range;
            rotationSpeed = turret.rotationSpeed;
            cooldown = turret.cooldown;

            bullet = turret.bullet;
            bulletPierces = turret.bulletPierces;
            bulletAmount = turret.bulletAmount;
            bulletSpeed = turret.bulletSpeed;
            bulletSpread = turret.bulletSpread;
            bulletTime = turret.bulletTime;
            bulletLock = turret.bulletLock;

            BaseSet(turret);
        }

        // Base turret stat variables
        public float damage;
        public float range;
        public float rotationSpeed;
        public float cooldown;

        // Bullet variables
        public DefaultBullet bullet;
        public int bulletPierces;
        public int bulletAmount;
        public float bulletSpeed;
        public float bulletSpread;
        public float bulletTime;
        public bool bulletLock;
    }

    // Instantiated buildings grab the corresponding stat class at runtime
    public static Dictionary<Building, BuildingStats> buildings;
    public static Dictionary<Collector, CollectorStats> collectors;
    public static Dictionary<Turret, DefenseStats> defenses;

    // Create building stat method
    public static void SetupBuilding(Building building)
    {
        BuildingStats newStat = new BuildingStats();
        newStat.BaseSet(building);
        buildings.Add(building, newStat);
    }

    // Create building stat method
    public static void SetupCollector(Collector collector)
    {
        CollectorStats newStat = new CollectorStats();
        newStat.Set(collector);
        collectors.Add(collector, newStat);
    }

    // Create building stat method
    public static void SetupTurret(Turret turret)
    {
        DefenseStats newStat = new DefenseStats();
        newStat.Set(turret);
        defenses.Add(turret, newStat);
    }
}
