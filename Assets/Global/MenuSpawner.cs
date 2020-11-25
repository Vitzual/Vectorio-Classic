using UnityEngine;
using Michsky.UI.ModernUIPack;
using TMPro;

public class MenuSpawner : MonoBehaviour
{
    private void Start() { InvokeRepeating("SpawnEnemies", 0f, 0.5f); }

    [System.Serializable]
    public class Enemies
    {
        public Transform enemyObject;
        public float chance;
    }

    public Enemies[] enemy;

    private void SpawnEnemies()
    {
        for (int a=0; a<enemy.Length; a++)
        {
            var proc = Random.Range(0, 100);
            if (proc <= enemy[a].chance)
            {
                SpawnEnemy(enemy[a].enemyObject);
            }
        }
    }

    void SpawnEnemy(Transform _enemy)
    {
        transform.localRotation = Quaternion.Euler(new Vector3(0, 0, Random.Range(0f, 360f)));
        transform.position += transform.right * 15;
        var holder = Instantiate(_enemy, transform.position, Quaternion.Euler(new Vector3(0, 0, 0)));
        holder.name = _enemy.name;
        transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
        transform.position = new Vector3(-2, -40, 0);
    }
}
