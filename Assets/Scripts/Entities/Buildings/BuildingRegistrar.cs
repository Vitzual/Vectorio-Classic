using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// The building registrar contains a list of stats for each building
// This can be added to at run time to support in-game modding

public class BuildingRegistrar : MonoBehaviour
{
    // Contains a list of all turret stats
    [System.Serializable]
    public class TurretStats
    {
        // Constructor
        public TurretStats(int damage, int range, float rotationSpeed, float fireRate, int bulletPierces, int bulletAmount, float bulletSpeed, float bulletSpread, AudioClip sound)
        {
            this.damage = damage;
            this.range = range;
            this.rotationSpeed = rotationSpeed;
            this.fireRate = fireRate;
            this.bulletPierces = bulletPierces;
            this.bulletAmount = bulletAmount;
            this.bulletSpeed = bulletSpeed;
            this.bulletSpread = bulletSpread;
            this.sound = sound;
        }
        
        // Turret info
        public string name;
        public Transform turretObj;

        // Contains turret stats
        public int damage;
        public int range;
        public float rotationSpeed;
        public float fireRate;
        public int bulletPierces;
        public int bulletAmount;
        public float bulletSpeed;
        public float bulletSpread;

        // Contains turret variables
        public AudioClip sound;
    }

    // Contains a list of all tile stats 
    [System.Serializable]
    public class TileStats
    {
        // Constructor
        public TileStats(int health, int maxHealth, int cost, int power, int heat)
        {
            this.health = health;
            this.maxHealth = maxHealth;
            this.cost = cost;
            this.power = power;
            this.heat = heat;
        }

        // Tile info
        public string name;
        public Transform tileObj;

        // Contains tile stats
        public int health;
        public int maxHealth;
        public int cost;
        public int power;
        public int heat;
    }

    // Create class lists
    public List<TurretStats> turretStats;
    public List<TileStats> tileStats;

    // Grab turret stats
    public TurretStats getTurretStats(Transform turret)
    {
        foreach (TurretStats stat in turretStats)
            if (turret == stat.turretObj) return stat;
        return null;
    }

    // Grab tile stats
    public TileStats getTileStats (Transform tile)
    {
        foreach (TileStats stat in tileStats)
            if (tile == stat.tileObj) return stat;
        return null;
    }
}
