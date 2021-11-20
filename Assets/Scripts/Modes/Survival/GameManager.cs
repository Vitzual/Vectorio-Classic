using UnityEngine;

public class GameManager
{
    [Header("Save Settings")]
    public static string name;
    public static string mode;
    public static string version;

    [Header("World Settings")]
    public static string seed;
    public static float goldSpawnModifier = 0;
    public static float essenceSpawnModifier = 0;
    public static float iridiumSpawnModifier = 0;

    [Header("Starting Resources")]
    public static int startingGold = 0;
    public static int startingEssence = 0;
    public static int startingIridium = 0;
    public static int startingPower = 0;
    public static int startingHeat = 0;

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
    public static void SetupGame(Difficulty difficulty, bool loadingGame)
    {
        // Set resource modifier variables 
        if (!difficulty.isCreative)
        {
            goldSpawnModifier = difficulty.goldSpawnModifier;
            essenceSpawnModifier = difficulty.essenceSpawnModifier;
            iridiumSpawnModifier = difficulty.iridiumSpawnModifier;

            startingGold = difficulty.startingGold;
            startingEssence = difficulty.startingEssence;
            startingIridium = difficulty.startingIridium;
            startingPower = difficulty.startingPower;
            startingHeat = difficulty.startingHeat;

            if (!loadingGame)
            {
                Resource.active.Add(Resource.CurrencyType.Gold, startingGold, true);
                Resource.active.Add(Resource.CurrencyType.Essence, startingEssence, true);
                Resource.active.Add(Resource.CurrencyType.Iridium, startingIridium, true);
                Resource.active.AddStorage(Resource.CurrencyType.Power, startingPower);
                Resource.active.AddStorage(Resource.CurrencyType.Heat, startingHeat);
            }
        }

        buildingCostModifier = difficulty.buildingCostModifier;
        buildingHealthModifier = difficulty.buildingHealthModifier;
        buildingDamageModifier = difficulty.buildingDamageModifier;

        enemyHealthModifier = difficulty.enemyHealthModifier;
        enemyDamageModifier = difficulty.enemyDamageModifier;
        enemySpeedModifier = difficulty.enemySpeedModifier;
        enemySpawnrateModifier = difficulty.enemySpawnrateModifier;
    }
}
