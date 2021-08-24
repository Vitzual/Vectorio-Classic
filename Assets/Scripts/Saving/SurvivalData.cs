[System.Serializable]
public class SurvivalData
{
    // Location data
    public int[,] buildings;
    public float[,] enemies;
    public int[] hotbar;
    
    // Resource data
    public int gold;
    public int essence;
    public int iridium;
    public int heat;
    public int power;

    // Progress data
    public int[] unlocked;
    public bool[] researchTechs;
    public int bossesDefeated;

    // Difficulty data
    public float enemyAmountMulti;
    public float enemyHealthMulti;
    public float enemyDamageMulti;
    public float enemySpeedMulti;
    public bool enemyOutposts;
    public bool enemyGroups;
    public bool enemyGuaridans;

    // World data
    public float goldSpawnAmount;
    public float essenceSpawnAmount;
    public float iridiumSpawnAmount;

    // Save data
    public string worldName;
    public string worldMode;
    public string worldSeed;
    public string worldVersion;
    public int worldPlaytime;

    public SurvivalData (Survival data_1, Technology data_2, EnemySpawner data_3, Research data_4, int worldPlaytime = 0) 
    {
        // Location data
        buildings = data_1.GetLocationData();
        enemies = data_1.GetEnemyData();
        hotbar = data_1.GetHotbarData();

        // Resource data
        gold = Resource.Get(Resource.Currency.Gold);
        essence = Resource.Get(Resource.Currency.Essence);
        iridium = Resource.Get(Resource.Currency.Iridium);
        heat = Resource.Get(Resource.Currency.Heat);
        power = Resource.Get(Resource.Currency.Power);
        
        // Progress data
        unlocked = data_2.GetSaveData();
        researchTechs = data_4.GetResearchData();
        bossesDefeated = data_3.bossesDefeated;

        // Difficulty data
        enemyAmountMulti = Difficulties.enemyAmountMulti;
        enemyHealthMulti = Difficulties.enemyHealthMulti;
        enemyDamageMulti = Difficulties.enemyDamageMulti;
        enemySpeedMulti = Difficulties.enemySpeedMulti;
        enemyOutposts = Difficulties.enemyOutposts;
        enemyGroups = Difficulties.enemyGroups;
        enemyGuaridans = Difficulties.enemyGuardians;

        // World data
        goldSpawnAmount = Difficulties.goldMulti;
        essenceSpawnAmount = Difficulties.essenceMulti;
        iridiumSpawnAmount = Difficulties.iridiumMulti;

        // Save data
        worldName = Difficulties.world;
        worldMode = Difficulties.mode;
        worldSeed = Difficulties.seed;
        worldVersion = Difficulties.version;
        this.worldPlaytime = worldPlaytime;
    }
}
