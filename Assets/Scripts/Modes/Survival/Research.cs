using UnityEngine;

public class Research : MonoBehaviour
{
    // Research variables
    public static int damage;
    public static int burning;
    public static int freezing;
    public static int poisoning;
    public static int shield;
    public static int health;
    public static int wall_health;
    public static int pierce;
    public static float range;
    public static float firerate;
    public static float bulletspeed;
    public static float gold_time = 1f;
    public static int gold_yield = 5;
    public static int gold_storage = 1000;
    public static float essence_time = 2f;
    public static int essence_yield = 5;
    public static int essence_storage = 500;
    public static float iridium_time = 5f;
    public static int iridium_yield = 1;
    public static int iridium_storage = 100;

    // Drone research variables
    public static int drone_tile_coverage = 5;
    public static float drone_deployment_speed = 3f;
    public static float drone_movement_speed = 25f;

    // Currency get variables (I hate this, and will redo it)
    public static int GetStorageAmount(Resource.CurrencyType type)
    {
        if (type == Resource.CurrencyType.Gold) return gold_storage;
        else if (type == Resource.CurrencyType.Essence) return essence_storage;
        else if (type == Resource.CurrencyType.Iridium) return iridium_storage;
        else return -1;
    }

    // Currency get variables (I hate this, and will redo it)
    public static int GetCollectionAmount(Resource.CurrencyType type)
    {
        if (type == Resource.CurrencyType.Gold) return gold_yield;
        else if (type == Resource.CurrencyType.Essence) return essence_yield;
        else if (type == Resource.CurrencyType.Iridium) return iridium_yield;
        else return -1;
    }

    // Currency get variables (I hate this, and will redo it)
    public static float GetCollectionRate(Resource.CurrencyType type)
    {
        if (type == Resource.CurrencyType.Gold) return gold_time;
        else if (type == Resource.CurrencyType.Essence) return essence_time;
        else if (type == Resource.CurrencyType.Iridium) return iridium_time;
        else return -1;
    }
}
