using UnityEngine;

public class ArtilleryBullet : BulletClass
{
    public LayerMask enemyLayer;
    public LayerMask enemyBuildingLayer;
    public float splashSize = 5;
    public int splashDamage = 100;
    public ParticleSystem Effect;

    public void Start()
    {
        HitEffect = Effect;
        StartCoroutine(SetLifetime(5));
    }

    public override void collide()
    {
        // Apply splash damage
        var colliders = Physics2D.OverlapCircleAll(this.gameObject.transform.position, splashSize + Research.research_range, enemyLayer | enemyBuildingLayer);
        foreach (Collider2D collider in colliders)
            switch (collider.name)
            {
                case "Hive":
                    collider.GetComponent<SpawnerAI>().SpawnEnemy();
                    break;
                case "Enemy Turret":
                    collider.GetComponent<EnemyTurretAI>().DamageTile(splashDamage);
                    break;
                case "Enemy Wall":
                    collider.GetComponent<EnemyWallAI>().DamageTile(splashDamage);
                    break;
                case "Enemy Mine":
                    collider.GetComponent<EnemyStaticAI>().DamageTile(splashDamage);
                    break;
                default:
                    collider.GetComponent<EnemyClass>().DamageEntity(splashDamage);
                    break;
            }

        Instantiate(HitEffect, transform.position, Quaternion.Euler(0, 0, transform.localEulerAngles.z + 180f));
        Destroy(gameObject);
    }

}
