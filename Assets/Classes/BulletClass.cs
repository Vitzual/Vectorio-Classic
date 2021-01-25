using System.Collections;
using UnityEngine;

public abstract class BulletClass : MonoBehaviour
{
    public ParticleSystem EnemyEffect;
    protected ParticleSystem HitEffect;
    private bool pierced = false;
    public int damage;

    public abstract void collide();

    public virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy")
        {
            // Check to see if other enemy is spawner
            if (other.name == "Hive")
                other.GetComponent<SpawnerAI>().SpawnEnemy();

            other.GetComponent<EnemyClass>().DamageEntity(damage + Research.bonus_damage);
            if (!this.name.Contains("BoltBullet"))
            {
                HitEffect = this.EnemyEffect;

                // Check if piercing has been unlocked
                if (!pierced && Research.bonus_pierce) pierced = true;
                else collide();
            }
        }
    }

    protected GameObject FindNearestEnemy()
    {
        var colliders = Physics2D.OverlapCircleAll(
            this.gameObject.transform.position,
            500,
            1 << LayerMask.NameToLayer("Enemy"));
        GameObject result = null;
        float closest = float.PositiveInfinity;

        foreach (Collider2D collider in colliders)
        {
            float distance = (collider.transform.position - this.transform.position).sqrMagnitude;
            if (distance < closest)
            {
                result = collider.gameObject;
                closest = distance;
            }
        }
        return result;
    }

    public IEnumerator SetLifetime(float a)
    {
        yield return new WaitForSeconds(a);
        if (this != null)
        {
            collide();
        }
    }
}
