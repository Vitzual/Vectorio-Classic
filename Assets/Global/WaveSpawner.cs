using UnityEngine;
using Michsky.UI.ModernUIPack;
using TMPro;

public class WaveSpawner : MonoBehaviour
{
    private Difficulties difficulties;
    public Survival survival;
    public Technology technology;
    public ProgressBar heatUI;
    public TextMeshProUGUI heatAmount;
    public int SpawnRegion = 1000;
    public int htrack = 0;
    public int totalSpawned = 0;

    private void Start()
    {
        difficulties = GameObject.Find("Difficulty").GetComponent<Difficulties>();
        InvokeRepeating("SpawnEnemies", 0f, 1f);
    }

    [System.Serializable]
    public class Enemies
    {
        public string name;
        public Transform enemyObject;
        public int minHeat;
        public int maxHeat;
        public int procChance;
    }

    public Enemies[] enemy;

    private void SpawnEnemies()
    {
        for (int a=0; a<enemy.Length; a++)
        {
            if ((htrack >= enemy[a].minHeat && htrack <= enemy[a].maxHeat))
            {
                int chance = ((htrack - enemy[a].minHeat) / 1000) + 1;
                if (chance >= 10) chance = 10;
                var proc = Random.Range(0, enemy[a].procChance);
                if (chance >= proc)
                {
                    SpawnEnemy(enemy[a].enemyObject);
                }
            }
        }
    }

    void SpawnEnemy(Transform _enemy)
    {
        totalSpawned += 1;
        Transform holder;
        switch(Random.Range(0, 4))
        {
            case 0:
                holder = Instantiate(_enemy, new Vector2(SpawnRegion, Random.Range(-SpawnRegion, SpawnRegion)), Quaternion.Euler(new Vector3(0, 0, 0)));
                break;
            case 1:
                holder = Instantiate(_enemy, new Vector2(-SpawnRegion, Random.Range(-SpawnRegion, SpawnRegion)), Quaternion.Euler(new Vector3(0, 0, 0)));
                break;
            case 2:
                holder = Instantiate(_enemy, new Vector2(Random.Range(-SpawnRegion, SpawnRegion), SpawnRegion), Quaternion.Euler(new Vector3(0, 0, 0))); ;
                break;
            case 3:
                holder = Instantiate(_enemy, new Vector2(Random.Range(-SpawnRegion, SpawnRegion), -SpawnRegion), Quaternion.Euler(new Vector3(0, 0, 0)));
                break;
            default:
                holder = Instantiate(_enemy, new Vector2(SpawnRegion, Random.Range(-SpawnRegion, SpawnRegion)), Quaternion.Euler(new Vector3(0, 0, 0)));
                break;
        }
        holder.name = _enemy.name;

        EnemyClass holderScript;
        holderScript = holder.GetComponent<EnemyClass>();
        holderScript.SetHealth((int)(holderScript.GetHealth() * difficulties.GetEnemyHP()));
        holderScript.SetDamage((int)(holderScript.GetDamage() * difficulties.GetEnemyDMG()));
    }

    public void increaseHeat(int a)
    {
        htrack += a;
        heatUI.currentPercent = ((float)htrack / 10000f * 100f);
        heatAmount.text = htrack.ToString();
        technology.UpdateUnlock(htrack);
    }

    public void decreaseHeat(int a)
    {
        htrack -= a;
        heatUI.currentPercent = ((float)htrack / 10000f * 100f);
        heatAmount.text = htrack.ToString();
    }
}
