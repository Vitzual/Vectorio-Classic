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
        public TurretStats(Transform obj, int damage, int range, float rotationSpeed, float fireRate, int bulletPierces, int bulletAmount, float bulletSpeed, float bulletSpread, float bulletSize, Material bulletMaterial, AudioClip sound)
        {
            this.obj = obj;
            this.damage = damage;
            this.range = range;
            this.rotationSpeed = rotationSpeed;
            this.fireRate = fireRate;
            this.bulletSize = bulletSize;
            this.bulletMaterial = bulletMaterial;
            this.bulletPierces = bulletPierces;
            this.bulletAmount = bulletAmount;
            this.bulletSpeed = bulletSpeed;
            this.bulletSpread = bulletSpread;
            this.sound = sound;
        }
        
        // Turret info
        public string name;
        public Transform obj;
        public bool animEnabled;

        // Contains turret stats
        public int damage;
        public int range;
        public float rotationSpeed;
        public float fireRate;
        public int bulletPierces;
        public int bulletAmount;
        public float bulletSpeed;
        public float bulletSpread;
        public float bulletSize;
        public Material bulletMaterial;

        // Contains turret variables
        public AudioClip sound;
    }

    // Contains a list of all tile stats 
    [System.Serializable]
    public class BuildingStats
    {
        // Constructor
        public BuildingStats(Transform obj, int ID, int health, int maxHealth, int cost, int power, int heat)
        {
            this.obj = obj;
            this.ID = ID;
            this.health = health;
            this.maxHealth = maxHealth;
            this.cost = cost;
            this.power = power;
            this.heat = heat;
        }

        // Tile info
        public string name;
        public Transform obj;
        public int ID;
        [TextArea] public string description;

        // Contains tile stats
        public int health;
        public int maxHealth;
        public int cost;
        public int power;
        public int heat;

        // Contains death particle
        public ParticleSystem deathParticle;
    }

    // Create class lists
    public List<TurretStats> turretStats;
    public List<BuildingStats> buildingStats;

    // Register ID's to IDDB 
    public void RegisterBuildingIDs()
    {
        foreach (BuildingStats building in buildingStats)
            IDDB.RegisterID(building.obj, building.ID);
    }

    // Grab turret stats
    public TurretStats GetTurretStats(Transform turret)
    {
        // Iterates through class list
        foreach (TurretStats stat in turretStats)
            if (turret == stat.obj) return stat;
        return null;
    }

    // Grab building stats
    public BuildingStats GetBuildingStats (Transform tile)
    {
        // Iterates through class list
        foreach (BuildingStats stat in buildingStats)
            if (tile == stat.obj) return stat;
        return null;
    }

    // Add to turret stats
    // If a duplicate transform is found, it will be replaced
    public void AddTurretStats(Transform obj, int damage, int range, float rotationSpeed, float fireRate, int bulletPierces, int bulletAmount, float bulletSpeed, float bulletSpread, float bulletSize, Material bulletMaterial, AudioClip sound = null)
    {
        // Remove older classes
        TurretStats stat = GetTurretStats(obj);
        if (stat != null) turretStats.Remove(stat);

        // Add the new stat class
        turretStats.Add(new TurretStats(obj, damage, range, rotationSpeed, fireRate, bulletPierces, bulletAmount, bulletSpeed, bulletSpread, bulletSize, bulletMaterial, sound));
    }

    // Add to turret stats
    // If a duplicate transform is found, it will be replaced
    public void AddBuildingStats(Transform obj, int ID, int health, int maxHealth, int cost, int power, int heat)
    {
        // Remove older classes
        BuildingStats stat = GetBuildingStats(obj);
        if (stat != null) buildingStats.Remove(stat);

        // Add the new stat class
        buildingStats.Add(new BuildingStats(obj, ID, health, maxHealth, cost, power, heat));
    }
}
