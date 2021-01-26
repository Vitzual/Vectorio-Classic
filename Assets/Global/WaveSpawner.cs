using UnityEngine;
using Michsky.UI.ModernUIPack;
using TMPro;

public class WaveSpawner : MonoBehaviour
{
    public Survival survival;
    public Technology technology;
    public ProgressBar heatUI;
    public TextMeshProUGUI heatAmount;
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
        transform.localRotation = Quaternion.Euler(new Vector3(0, 0, Random.Range(0f, 360f)));
        transform.position += transform.right * (survival.AOC_Size + 100);
        var holder = Instantiate(_enemy, transform.position, Quaternion.Euler(new Vector3(0, 0, 0)));
        holder.name = _enemy.name;
        transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
        transform.position = new Vector3(0, 0, 0);
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
