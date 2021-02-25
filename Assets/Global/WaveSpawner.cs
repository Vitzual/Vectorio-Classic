using UnityEngine;
using Michsky.UI.ModernUIPack;
using TMPro;

public class WaveSpawner : MonoBehaviour
{
    public Survival survival;
    public Technology technology;
    public ProgressBar heatUI;
    public TextMeshProUGUI heatAmount;
    public int SpawnRegion = 1000;
    public int htrack = 0;

    private void Start()
    {
        InvokeRepeating("SpawnEnemies", 0f, 1f);
    }

    [System.Serializable]
    public class Enemies
    {
        public string name;
        public Transform enemyObject;
        public float chance;
        public float minHeat;
        public float maxHeat;
        public int amount;
    }

    public Enemies[] enemy;

    private void SpawnEnemies()
    {
        for (int a=0; a<enemy.Length; a++)
        {
            if ((htrack >= enemy[a].minHeat && htrack <= enemy[a].maxHeat) || (htrack >= enemy[a].minHeat && enemy[a].maxHeat == 10000))
            {
                for(int b=0; b<enemy[a].amount; b++)
                {
                    var proc = Random.Range(0, 100);
                    if (proc <= enemy[a].chance)
                    {
                        SpawnEnemy(enemy[a].enemyObject);
                    }
                }
            }
        }
    }

    void SpawnEnemy(Transform _enemy)
    {
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
