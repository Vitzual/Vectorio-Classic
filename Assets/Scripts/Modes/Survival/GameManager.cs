using UnityEngine;

public class GameManager
{
    [Header("Save Settings")]
    public static string name;
    public static string mode;
    public static string version;

    [Header("World Settings")]
    public static string seed;
    public static float goldSpawnModifier;
    public static float essenceSpawnModifier;
    public static float iridiumSpawnModifier;

    [Header("Starting Resources")]
    public static int startingGold;
    public static int startingEssence;
    public static int startingIridium;
    public static int startingPower;
    public static int startingHeat;

    [Header("Building Difficulty")]
    public static float buildingCostModifier;
    public static float buildingHealthModifier;
    public static float buildingDamageModifier;

    [Header("Enemy Difficulty")]
    public static float enemyHealthModifier;
    public static float enemyDamageModifier;
    public static float enemySpeedModifier;
    public static float enemySpawnrateModifier;

    // Sets up a game
    public static void SetupGame(Difficulty difficulty, string name = "Unnamed Save", string mode = "Survival", string version = "v0.3", string seed = "")
    {
        // Set save variables
        GameManager.name = name;
        GameManager.mode = mode;
        GameManager.version = version;
        GameManager.seed = seed;

        // Set modifier variables 
        goldSpawnModifier = difficulty.goldSpawnModifier;
        essenceSpawnModifier = difficulty.essenceSpawnModifier;
        iridiumSpawnModifier = difficulty.iridiumSpawnModifier;

        // Generate world
        if (WorldGenerator.active != null)
            WorldGenerator.active.GenerateWorldData();

        startingGold = difficulty.startingGold;
        startingEssence = difficulty.startingEssence;
        startingIridium = difficulty.startingIridium;
        startingPower = difficulty.startingPower;
        startingHeat = difficulty.startingHeat;

        buildingCostModifier = difficulty.buildingCostModifier;
        buildingHealthModifier = difficulty.buildingHealthModifier;
        buildingDamageModifier = difficulty.buildingDamageModifier;

        enemyHealthModifier = difficulty.enemyHealthModifier;
        enemyDamageModifier = difficulty.enemyDamageModifier;
        enemySpeedModifier = difficulty.enemySpeedModifier;
        enemySpawnrateModifier = difficulty.enemySpawnrateModifier;
    }
}
