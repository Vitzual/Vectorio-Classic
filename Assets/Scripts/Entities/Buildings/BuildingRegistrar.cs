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
        // Turret info
        public string name;
        public Transform turretObj;

        // Contains turret stats
        public int damage;
        public int range;
        public int rotationSpeed;
        public float fireRate;
        public int bulletPierces;
        public int bulletAmount;
        public float bulletSpeed;
        public float bulletSpread;
    }

    // Contains a list of all tile stats 
    [System.Serializable]
    public class TileStats
    {
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
