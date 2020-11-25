using UnityEngine;
using Michsky.UI.ModernUIPack;
using TMPro;

public class WaveSpawner : MonoBehaviour
{

    public GameObject survival;
    public ProgressBar heatUI;
    private int htrack;
    private int heat;

    private void Start()
    {
        htrack = 1;
        heat = 0;
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
        transform.position += transform.right * 350;
        Instantiate(_enemy, transform.position, Quaternion.Euler(new Vector3(0, 0, 0)));
        transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
        transform.position = new Vector3(0, 0, 0);
    }

    public void increaseHeat(int a)
    {
        htrack += a;
        heat = htrack / 100;
        heatUI.currentPercent = heat;
    }

    public void decreaseHeat(int a)
    {
        htrack -= a;
        heat = htrack / 100;
        heatUI.currentPercent = heat;
    }
}
