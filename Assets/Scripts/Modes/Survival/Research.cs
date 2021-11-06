using UnityEngine;
using UnityEngine.UI;
using Michsky.UI.ModernUIPack;
using TMPro;

public class Research : MonoBehaviour
{
    // Research variables
    public static int research_damage;
    public static int research_burning;
    public static int research_freezing;
    public static int research_poisoning;
    public static int research_shield;
    public static int research_health;
    public static int research_wall_health;
    public static int research_pierce;
    public static float research_range;
    public static float research_firerate;
    public static float research_bulletspeed;
    public static float research_gold_time = 1f;
    public static int research_gold_yield = 5;
    public static int research_gold_storage = 1000;
    public static float research_essence_time = 2f;
    public static int research_essence_yield = 5;
    public static int research_essence_storage = 500;
    public static float research_iridium_time = 5f;
    public static int research_iridium_yield = 1;
    public static int research_iridium_storage = 100;
    public static float research_construction_speed;
    public static int research_construction_placements;
    public static float research_resource_speed;
    public static int research_resource_collections;
    public static int research_resource_amount;
    public static float research_resource_range;
    public static float research_combat_speed;
    public static float research_fixer_speed;
    public static int research_fixer_amount;
    public static int research_combat_targets;
    public static bool research_explosive_storages;
    public static bool research_explosive_defenses;
    public static bool research_explosive_collectors;
    public static int research_research_speed;
    public static bool research_fixer_drones;
    public static bool research_combat_drones;

    // Currency get variables (I hate this, and will redo it)
    public static int GetStorageAmount(Resource.CurrencyType type)
    {
        if (type == Resource.CurrencyType.Gold) return research_gold_storage;
        else if (type == Resource.CurrencyType.Essence) return research_essence_storage;
        else if (type == Resource.CurrencyType.Iridium) return research_iridium_storage;
        else return -1;
    }

    // Currency get variables (I hate this, and will redo it)
    public static int GetCollectionAmount(Resource.CurrencyType type)
    {
        if (type == Resource.CurrencyType.Gold) return research_gold_yield;
        else if (type == Resource.CurrencyType.Essence) return research_essence_yield;
        else if (type == Resource.CurrencyType.Iridium) return research_iridium_yield;
        else return -1;
    }

    // Currency get variables (I hate this, and will redo it)
    public static float GetCollectionRate(Resource.CurrencyType type)
    {
        if (type == Resource.CurrencyType.Gold) return research_gold_time;
        else if (type == Resource.CurrencyType.Essence) return research_essence_time;
        else if (type == Resource.CurrencyType.Iridium) return research_iridium_time;
        else return -1;
    }
}
