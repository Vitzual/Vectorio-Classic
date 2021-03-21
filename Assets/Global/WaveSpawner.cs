using UnityEngine;
using Michsky.UI.ModernUIPack;
using TMPro;

public class WaveSpawner : MonoBehaviour
{
    // Spawner elements
    private Difficulties difficulties;
    public Survival survival;
    public Technology technology;
    public ProgressBar heatUI;
    public TextMeshProUGUI heatAmount;
    public TextMeshProUGUI maxHeatAmount;
    public int SpawnRegion = 1000;
    public int htrack = 0;
    public int maxHeat = 10000;
    public int totalSpawned = 0;
    public bool groupSpawned = false;
    public GameObject[] borders;
    public bool loadingSave = true;
    public bool bossSpawned = false;
    public bool firstDisplay = true;
    public int bossesDefeated = 0;

    // UI Elements
    public GameObject bossWarning;
    public ModalWindowManager bossInfo;
    public ModalWindowManager bossDestroyed;

    [System.Serializable]
    public class Bosses
    {
        public Transform bossObject;
        public string name;
        [TextArea] public string description;
        [TextArea] public string destroyedInfo;
        public bool isDefeated;
    }
    public Bosses[] bosses;

    [System.Serializable]
    public class Enemies
    {
        public string name;
        public Transform enemyObject;
        public int minHeat;
        public int maxHeat;
        public int procChance;
        public int groupSpawnEvery;
        public int groupSpawnTracker;
    }

    public Enemies[] enemy;

    private void Start()
    {
        difficulties = GameObject.Find("Difficulty").GetComponent<Difficulties>();
        InvokeRepeating("SpawnEnemies", 0f, 1f);
    }

    private void SpawnEnemies()
    {
        if (bossSpawned) return;
        groupSpawned = false;
        for (int a=0; a<enemy.Length; a++)
        {
            if ((htrack >= enemy[a].minHeat && htrack <= enemy[a].maxHeat))
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
    }

    void SpawnEnemy(int index)
    {
        totalSpawned += 1;
        Transform holder;
        switch(Random.Range(0, 4))
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
        holderScript.SetHealth((int)(holderScript.GetHealth() * difficulties.GetEnemyHP()));
        holderScript.SetDamage((int)(holderScript.GetDamage() * difficulties.GetEnemyDMG()));

        // If group spawning is enabled, check if it's time to spawn a group
        if (enemy[index].groupSpawnEvery != 0)
        {
            if (enemy[index].groupSpawnEvery == 1)
            {
                if (!groupSpawned)
                {
                    groupSpawned = true;
                    enemy[index].groupSpawnEvery = enemy[index].groupSpawnTracker;
                    Transform groupHolder;
                    for (int i = 0; i < 20; i++)
                    {
                        groupHolder = Instantiate(enemy[index].enemyObject, new Vector2(Random.Range(-15, 15) + holder.transform.position.x, Random.Range(-15, 15) + holder.transform.position.y), Quaternion.Euler(new Vector3(0, 0, 0)));
                        groupHolder.name = enemy[index].enemyObject.name;
                    }
                }
            } else enemy[index].groupSpawnEvery--;
        }
    }

    public void increaseHeat(int a)
    {
        htrack += a;
        heatUI.currentPercent = ((float)htrack / maxHeat) * 100f;
        heatAmount.text = htrack.ToString();
        technology.UpdateUnlock(htrack);

        if (bossesDefeated == 0 && htrack >= 10000 && !bosses[0].isDefeated && !bossSpawned && !loadingSave)
        {
            Transform holder = Instantiate(bosses[0].bossObject, new Vector2(0, SpawnRegion), Quaternion.Euler(new Vector3(0, 0, 0)));
            holder.name = bosses[0].bossObject.name;
            bossSpawned = true;
        }

        // Check if the warning should be displayed or not
        if (htrack <= 10000 && htrack >= 9000 && bossesDefeated == 0) {
            if (!bossWarning.activeInHierarchy) { bossWarning.SetActive(true); }
        } else if (bossWarning.activeInHierarchy) { bossWarning.SetActive(false); }

        // Display end screen
        if (htrack >= 20000 && firstDisplay)
        {
            GameObject.Find("Survival").GetComponent<Interface>().OpenEndWindow();
            maxHeat = 30000;
            firstDisplay = false;
        }

        if (!bossSpawned)
        {
            if (htrack >= 20000)
            {
                borders[2].SetActive(true);
                borders[1].SetActive(false);
            }
            else if (htrack >= 10000)
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
    }

    public void updateBosses()
    {
        for (int i=1; i <= bosses.Length; i++)
            if (htrack >= i * 10000)
                defeatBoss(i-1);
        loadingSave = false;
    }

    public void defeatBoss(int a)
    {
        // Show info
        if (!loadingSave) displayBossDestroyed();

        // Set new values
        bosses[a].isDefeated = true;
        bossesDefeated += 1;
        bossSpawned = false;
        if (maxHeat + 10000 > 30000) maxHeat = 30000;
        else maxHeat += 10000;
        maxHeatAmount.text = maxHeat + " MAX";

        // Updates the border
        increaseHeat(0);
    }

    public void decreaseHeat(int a)
    {
        htrack -= a;
        heatUI.currentPercent = ((float)htrack / maxHeat) * 100f;
        heatAmount.text = htrack.ToString();
    }

    public void displayBossInfo()
    {
        bossInfo.titleText = bosses[bossesDefeated].name;
        bossInfo.descriptionText = bosses[bossesDefeated].description;
        bossInfo.UpdateUI();
        bossInfo.OpenWindow();
        survival.UI.BossInfoOpen = true;
        Time.timeScale = 0f;
    }

    public void displayBossDestroyed()
    {
        bossDestroyed.titleText = bosses[bossesDefeated].name;
        bossDestroyed.descriptionText = bosses[bossesDefeated].destroyedInfo;
        bossDestroyed.UpdateUI();
        bossDestroyed.OpenWindow();
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
