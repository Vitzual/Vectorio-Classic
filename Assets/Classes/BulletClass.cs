using System.Collections;
using UnityEngine;

public abstract class BulletClass : MonoBehaviour
{
    public ParticleSystem EnemyEffect;
    protected ParticleSystem HitEffect;
    public int damage;

    public abstract void collide();

    public virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy")
        {
            other.GetComponent<EnemyClass>().DamageEntity(damage);
            if (!this.name.Contains("BoltBullet"))
            {
                HitEffect = this.EnemyEffect;
                collide();
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
