[System.Serializable]
public class SaveData
{
    //public int[][] Buildings;
    public int[,] Locations;
    public float[,] Enemies;
    public int[] hotbar;
    public int PowerUsage;
    public int PowerAvailable;
    public int Gold;
    public int Essence;
    public int Iridium;
    public int HeatUsage;
    public int UnlockLevel;
    public bool UnlocksLeft;
    public int RLevel;
    public bool[] ResearchedTiers;
    public int Difficulty;
    public int startingGold;
    public int startingPower;
    public float EnemyAmountMulti;
    public float EnemyHealthMulti;
    public float EnemyDamageMulti;
    public float EnemySpeedMulti;
    public float GoldAmount;
    public float EssenceAmount;
    public float IridiumAmount;
    public bool EnemyOutposts;
    public bool EnemyGroups;
    public bool EnemyGuaridans;
    public string WorldName;
    public string WorldMode;
    public string WorldSeed;
    public string WorldVersion;
    public int time;
    public int heatt;
    public int bossesDefeated;

    public SaveData (Survival data, Technology unlock, WaveSpawner heat, Research research, int time = 0, int heatt = 0) 
    {
        //Buildings = data.GetSaveData();
        hotbar = data.GetHotbarData();
        Locations = data.GetLocationData();
        Enemies = data.GetEnemyData();
        PowerUsage = data.PowerConsumption;
        PowerAvailable = data.AvailablePower;
        Gold = data.gold;
        Essence = data.essence;
        Iridium = data.iridium; 
        HeatUsage = heat.htrack;
        UnlockLevel = unlock.UnlockAmount;
        UnlocksLeft = unlock.UnlocksLeft;
        ResearchedTiers = research.GetResearchData();
        EnemyAmountMulti = Difficulties.enemyAmountMulti;
        EnemyHealthMulti = Difficulties.enemyHealthMulti;
        EnemyDamageMulti = Difficulties.enemyDamageMulti;
        EnemySpeedMulti = Difficulties.enemySpeedMulti;
        GoldAmount = Difficulties.goldMulti;
        EssenceAmount = Difficulties.essenceMulti;
        IridiumAmount = Difficulties.iridiumMulti;
        EnemyOutposts = Difficulties.enemyOutposts;
        EnemyOutposts = Difficulties.enemyOutposts;
        EnemyOutposts = Difficulties.enemyOutposts;
        WorldName = Difficulties.world;
        WorldMode = Difficulties.mode;
        WorldSeed = Difficulties.seed;
        WorldVersion = Difficulties.version;
        this.time = time;
        this.heatt = heatt;
        this.bossesDefeated = heat.bossesDefeated;
    }
}
