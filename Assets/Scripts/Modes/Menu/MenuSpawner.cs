using UnityEngine;
using Michsky.UI.ModernUIPack;
using TMPro;

public class MenuSpawner : MonoBehaviour
{
    public Variant variant;

    private void Start() { InvokeRepeating("SpawnEnemies", 0f, 0.5f); }

    [System.Serializable]
    public class Enemies
    {
        public EnemyData enemy;
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
                SpawnEnemy(enemy[a].enemy);
            }
        }
    }

    void SpawnEnemy(EnemyData enemy)
    {
        Vector3 OGP = transform.position;
        transform.localRotation = Quaternion.Euler(new Vector3(0, 0, Random.Range(0f, 360f)));
        transform.position += transform.right * 15;
        var holder = Instantiate(enemy.obj, transform.position, Quaternion.Euler(new Vector3(0, 0, 0)));
        holder.name = enemy.name;
        transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
        transform.position = OGP;

        Enemy newEnemy = holder.GetComponent<Enemy>();
        newEnemy.Setup(enemy, variant);

        EnemyHandler.active.enemies.Add(newEnemy);
    }
}
