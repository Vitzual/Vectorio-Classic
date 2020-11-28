using UnityEngine;
using Michsky.UI.ModernUIPack;
using TMPro;

public class WaveSpawner : MonoBehaviour
{

    public GameObject survival;
    public ProgressBar heatUI;
    public int htrack = 0;
    public int heat = 0;

    private void Start()
    {
        InvokeRepeating("SpawnEnemies", 0f, 1f);
    }

    [System.Serializable]
    public class Enemies
    {
        public Transform enemyObject;
        public float chance;
        public float minHeat;
        public float maxHeat;
    }

    public Enemies[] enemy;

    private void SpawnEnemies()
    {
        for (int a=0; a<enemy.Length; a++)
        {
            if (heat >= enemy[a].minHeat && heat <= enemy[a].maxHeat)
            {
                var proc = Random.Range(0, 100);
                if (proc <= enemy[a].chance)
                {
                    SpawnEnemy(enemy[a].enemyObject);
                }
            }
        }
    }

    void SpawnEnemy(Transform _enemy)
    {
        transform.localRotation = Quaternion.Euler(new Vector3(0, 0, Random.Range(0f, 360f)));
        transform.position += transform.right * 300;
        var holder = Instantiate(_enemy, transform.position, Quaternion.Euler(new Vector3(0, 0, 0)));
        holder.name = _enemy.name;
        transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
        transform.position = new Vector3(0, 0, 0);
    }

    public void increaseHeat(int a)
    {
        htrack += a;
        heat = htrack / 120;
        heatUI.currentPercent = heat;
    }

    public void decreaseHeat(int a)
    {
        htrack -= a;
        heat = htrack / 120;
        if (heat < 0) heat = 0;
        heatUI.currentPercent = heat;
    }
}
