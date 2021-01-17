using UnityEngine;
using Michsky.UI.ModernUIPack;
using System.Collections.Generic;
using TMPro;

public class WaveSpawner : MonoBehaviour
{

    public ProgressBar heatUI;
    public TextMeshProUGUI heatAmount;
    public int htrack = 0;
    public int heat = 0;
    List<GameObject> discovered = new List<GameObject>();

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
        transform.position += transform.right * 370;
        var holder = Instantiate(_enemy, transform.position, Quaternion.Euler(new Vector3(0, 0, 0)));
        holder.name = _enemy.name;
        transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
        transform.position = new Vector3(0, 0, 0);
    }

    public void increaseHeat(int a)
    {
        htrack += a;
        heat = htrack / 150;
        heatUI.currentPercent = ((float)htrack / 10000f * 100f);
        heatAmount.text = htrack.ToString();
    }

    public void decreaseHeat(int a)
    {
        htrack -= a;
        heat = htrack / 150;
        heatUI.currentPercent = ((float)htrack / 10000f * 100f);
        heatAmount.text = htrack.ToString();
    }

    public bool checkIfEnemyDiscovered(GameObject a)
    {
        for (int i=0; i<enemy.Length; i++)
        {
            if (enemy[i].enemyObject == a)
            {
                if (enemy[i].minHeat <= heat) return true;
            }
        }
        return false;
    }
}
