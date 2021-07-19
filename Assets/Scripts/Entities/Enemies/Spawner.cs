using UnityEngine;
using Michsky.UI.ModernUIPack;
using TMPro;

public class WaveSpawner : MonoBehaviour
{
    // Spawner elements
    public Survival survival;
    public Technology technology;
    public ProgressBar heatUI;
    public TextMeshProUGUI heatAmount;
    public TextMeshProUGUI maxHeatAmount;
    public int SpawnRegion = 1000;
    public int htrack = 0;
    public int maxHeat = 10000;
    public GameObject[] borders;
    public bool loadingSave = true;
    public bool bossSpawned = false;
    public bool firstDisplay = true;
    public int bossesDefeated = 0;

    // Big attack thing
    public int attackEvery = 300;
    public int attackTracker = 50;

    // Scan through tracker
    public int startingIndex = 0;
    public int lastIndex = 9;

    [System.Serializable]
    public class Attacks
    {
        public string name;
        public Transform groupObject;
        public int minHeat;
        public int maxHeat;
    }
    public Attacks[] attacks;

    // UI Elements
    public ModalWindowManager bossInfo;
    public ModalWindowManager bossDestroyed;

    [System.Serializable]
    public class Bosses
    {
        public string name;
        public Transform bossObject;
        public bool isDefeated;
        public bool BossScreenShown = false;
        public ModalWindowManager BossScreen;
        public ModalWindowManager BossDefeatedScreen;
    }
    public Bosses[] bosses;

    [System.Serializable]
    public class Enemies
    {
        public string name;
        public Transform enemyObject;
        public int minHeat;
        public int procChance;
    }

    public Enemies[] enemy;

    public float amountMulti = 1;
    public float healthMulti = 1;
    public float damageMulti = 1;
    public float speedMulti = 1;
    

    private void Start()
    {
        InvokeRepeating("SpawnEnemies", 0f, 1f);

        if (Difficulties.enemyAmountMulti > 500) Difficulties.enemyAmountMulti = 500;
        if (Difficulties.enemyHealthMulti > 250) Difficulties.enemyHealthMulti = 250;
        if (Difficulties.enemyDamageMulti > 250) Difficulties.enemyDamageMulti = 250;
        if (Difficulties.enemySpeedMulti > 250) Difficulties.enemySpeedMulti = 250;

        amountMulti = Difficulties.enemyAmountMulti / 100;
        healthMulti = Difficulties.enemyHealthMulti / 100;
        damageMulti = Difficulties.enemyDamageMulti / 100;
        speedMulti = Difficulties.enemySpeedMulti / 100;

        if (amountMulti <= 0) amountMulti = 1;
        if (healthMulti <= 0) healthMulti = 1;
        if (damageMulti <= 0) damageMulti = 1;
        if (speedMulti <= 0) speedMulti = 1;

        attackEvery = attackEvery - (int)(Difficulties.enemyAmountMulti / 3);

        setMaxHeatUI();
    }

    // Returns a buildings ID if unlocked
    public Transform GetEnemyWithID(int a)
    {
        for (int i = 0; i < enemy.Length; i++)
        {
            if (enemy[i].enemyObject.GetComponent<EnemyClass>().GetID() == a)
            {
                return enemy[i].enemyObject;
            }
        }
        return null;
    }

    public void SetSpawnAmount(int a)
    {
        attackEvery = a;
    }

    private void SpawnEnemies()
    {
        // Check if it's time for a group spawn
        bool groupSpawn = false;
        if (Difficulties.enemyWaves)
        {
            if (htrack >= 1000)
            {
                if (attackTracker >= attackEvery)
                {
                    groupSpawn = true;
                    attackTracker = 0;
                }
                else attackTracker++;
            }
        }

        // Iterate through and spawn all enemies
        for (int a=startingIndex; a<lastIndex; a++)
        {
            if (htrack >= enemy[a].minHeat)
            {
                int chance = ((htrack - enemy[a].minHeat) / 1000) + 1;
                if (chance >= 10) chance = 10;
                var proc = Random.Range(0, enemy[a].procChance);
                if (chance >= proc)
                {
                    SpawnEnemy(a);
                }
            }
        }

        // Large spawns
        if (groupSpawn)
        {
            Vector2 spawnPos;
            float rotation;
            string spawnInfo;
            switch(Random.Range(1, 5))
            {
                case 1:
                    spawnPos = new Vector2(SpawnRegion, Random.Range(-SpawnRegion, SpawnRegion));
                    rotation = 90f;
                    spawnInfo = "A large attack is coming from the East!";
                    break;
                case 2:
                    spawnPos = new Vector2(-SpawnRegion, Random.Range(-SpawnRegion, SpawnRegion));
                    spawnInfo = "A large attack is coming from the West!";
                    rotation = 270f;
                    break;
                case 3:
                    spawnPos = new Vector2(Random.Range(-SpawnRegion, SpawnRegion), SpawnRegion);
                    spawnInfo = "A large attack is coming from the North!";
                    rotation = 0f;
                    break;
                case 4:
                    spawnPos = new Vector2(Random.Range(-SpawnRegion, SpawnRegion), -SpawnRegion);
                    spawnInfo = "A large attack is coming from the South!";
                    rotation = 180f;
                    break;
                default:
                    spawnPos = new Vector2(SpawnRegion, Random.Range(-SpawnRegion, SpawnRegion));
                    spawnInfo = "A large attack is coming from the East!";
                    rotation = 90f;
                    break;
            }

            // Iterate through and spawn all enemies
            for (int a = 0; a < attacks.Length; a++)
            {
                if (htrack >= attacks[a].minHeat && htrack <= attacks[a].maxHeat)
                {
                    Instantiate(attacks[a].groupObject, spawnPos, Quaternion.Euler(new Vector3(0, 0, rotation)));
                    survival.UI.DisplayGroupComing(spawnInfo);
                    return;
                }
            }
        }
    }

    void SpawnEnemy(int index)
    {
        for (int i = 0; i < amountMulti; i++)
        {
            Transform holder;
            switch (Random.Range(0, 4))
            {
                case 0:
                    holder = Instantiate(enemy[index].enemyObject, new Vector2(SpawnRegion, Random.Range(-SpawnRegion, SpawnRegion)), Quaternion.Euler(new Vector3(0, 0, 0)));
                    break;
                case 1:
                    holder = Instantiate(enemy[index].enemyObject, new Vector2(-SpawnRegion, Random.Range(-SpawnRegion, SpawnRegion)), Quaternion.Euler(new Vector3(0, 0, 0)));
                    break;
                case 2:
                    holder = Instantiate(enemy[index].enemyObject, new Vector2(Random.Range(-SpawnRegion, SpawnRegion), SpawnRegion), Quaternion.Euler(new Vector3(0, 0, 0)));
                    break;
                case 3:
                    holder = Instantiate(enemy[index].enemyObject, new Vector2(Random.Range(-SpawnRegion, SpawnRegion), -SpawnRegion), Quaternion.Euler(new Vector3(0, 0, 0)));
                    break;
                default:
                    holder = Instantiate(enemy[index].enemyObject, new Vector2(SpawnRegion, Random.Range(-SpawnRegion, SpawnRegion)), Quaternion.Euler(new Vector3(0, 0, 0)));
                    break;
            }
            holder.name = enemy[index].enemyObject.name;

            EnemyClass holderScript;
            holderScript = holder.GetComponent<EnemyClass>();

            holderScript.SetHealth((int)(holderScript.GetHealth() * healthMulti));
            holderScript.SetDamage((int)(holderScript.GetDamage() * damageMulti));
            //holderScript.SetSpeed(holderScript.GetSpeed() * speedMulti);
        }
    }

    public void checkHeat()
    {
        if (htrack >= 50000 && bosses[2].isDefeated) updateBorders(3);
        else if (htrack >= 25000 && bosses[1].isDefeated) updateBorders(2);
        else if (htrack >= 10000 && bosses[0].isDefeated) updateBorders(1);
        else updateBorders(0);
    }

    public void setMaxHeatUI()
    {
        if (maxHeat >= 1000000) maxHeatAmount.text = string.Concat(maxHeat / 100000, "M") + " BOSS";
        else if (maxHeat >= 10000) maxHeatAmount.text = string.Concat(maxHeat / 1000, "K") + " BOSS";
        else maxHeatAmount.text = maxHeatAmount.ToString() + " BOSS";
    }

    public void increaseHeat(int a)
    {
        htrack += a;
        checkHeat();
        if (htrack > maxHeat)
        {
            heatUI.currentPercent = 100f;
            heatAmount.text = maxHeat.ToString();
        }
        else
        {
            heatUI.currentPercent = ((float)htrack / maxHeat) * 100f;
            heatAmount.text = htrack.ToString();
        }
        technology.UpdateUnlock("Heat");

        // Display alpha screen
        if (htrack >= 80000 && firstDisplay)
        {
            GameObject.Find("Survival").GetComponent<Interface>().OpenEndWindow();
            firstDisplay = false;
        }

        // Check for warning 
        if (!bosses[1].isDefeated && !bosses[1].BossScreenShown && htrack >= 24000 && !loadingSave)
        {
            bosses[1].BossScreenShown = true;
            bosses[1].BossScreen.OpenWindow();
            Time.timeScale = 0f;
        }
        else if (!bosses[0].isDefeated && !bosses[0].BossScreenShown && htrack >= 9000 && !loadingSave)
        {
            bosses[0].BossScreenShown = true;
            bosses[0].BossScreen.OpenWindow();
            Time.timeScale = 0f;
        }

        // Check for guardian spawning
        if (!bosses[2].isDefeated && htrack >= 50000) 
        { 
            maxHeat = 80000;
            startingIndex = 27;
            lastIndex = 36;
            bosses[2].isDefeated = true;
            bossesDefeated = 3;
            setMaxHeatUI();
        }
        else if (!bosses[1].isDefeated && htrack >= 25000 && !bossSpawned && !loadingSave)
        {
            Transform holder = Instantiate(bosses[1].bossObject, new Vector2(0, -SpawnRegion), Quaternion.Euler(new Vector3(0, 0, 0)));
            holder.name = bosses[1].bossObject.name;
            bossSpawned = true;
        }
        else if (!bosses[0].isDefeated && htrack >= 10000 && !bossSpawned && !loadingSave)
        {
            Transform holder = Instantiate(bosses[0].bossObject, new Vector2(0, SpawnRegion), Quaternion.Euler(new Vector3(0, 0, 0)));
            holder.name = bosses[0].bossObject.name;
            bossSpawned = true;
        }
    }

    public void updateBorders(int a)
    {
        setIndex(a);
        if (a == 3)
        {
            borders[3].SetActive(true);
            borders[2].SetActive(false);
        }
        else if (a == 2)
        {
            borders[3].SetActive(false);
            borders[2].SetActive(true);
            borders[1].SetActive(false);
        }
        else if (a == 1)
        {
            borders[2].SetActive(false);
            borders[1].SetActive(true);
            borders[0].SetActive(false);
        }
        else
        {
            borders[0].SetActive(true);
            borders[1].SetActive(false);
        }
    }

    public void setIndex(int type)
    {
        switch(type)
        {
            case 0:
                startingIndex = 0;
                lastIndex = 9;
                return;
            case 1:
                startingIndex = 9;
                lastIndex = 18;
                return;
            case 2:
                startingIndex = 18;
                lastIndex = 27;
                return;
            case 3:
                startingIndex = 27;
                lastIndex = 36;
                return;
        }
    }

    public void updateBosses(int a)
    {
        try
        {
            Debug.Log(a + " bosse(s) defeated. Setting to save.");
            for (int i = 0; i < a; i++) defeatBoss(i);
        }
        catch
        {
            Debug.Log("Save does not contain boss tracking data");
        }

        // Backup for older saves (temp)
        if (!bosses[0].isDefeated && htrack >= 10000) defeatBoss(0);
        if (!bosses[1].isDefeated && htrack >= 25000) defeatBoss(1);
        if (!bosses[2].isDefeated && htrack >= 50000) defeatBoss(2);
        increaseHeat(0);
        loadingSave = false;
    }

    public void defeatBoss(int a)
    {
        // Show info
        if (!loadingSave) { displayBossDestroyed(a); }

        // Update tech tree
        technology.UpdateUnlock("Boss", a);

        // Set new values
        bosses[a].isDefeated = true;
        bossesDefeated = a+1;
        bossSpawned = false;

        // Set new max heat
        if (a == 0 && maxHeat < 25000) maxHeat = 25000;
        else if (a == 1 && maxHeat < 50000) maxHeat = 50000;
        else if (a == 2 && maxHeat < 80000) maxHeat = 80000;
        setMaxHeatUI();
        heatUI.currentPercent = ((float)htrack / maxHeat) * 100f;
        heatAmount.text = htrack.ToString();

        // Set border
        updateBorders(a+1);
    }

    public void decreaseHeat(int a)
    {
        if (a < 0)
            increaseHeat(-a);
        else
        {
            checkHeat();

            htrack -= a;
            heatUI.currentPercent = ((float)htrack / maxHeat) * 100f;
            heatAmount.text = htrack.ToString();
        }
    }

    public void displayBossDestroyed(int a)
    {
        bosses[a].BossDefeatedScreen.OpenWindow();
        survival.UI.BossInfoOpen = true;
        Time.timeScale = 0f;
    }

    public void closeBossInfo()
    {
        survival.UI.BossInfoOpen = false;
        bossInfo.CloseWindow();
        bossDestroyed.CloseWindow();
        Time.timeScale = 1f;
    }
}
