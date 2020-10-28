using System.Collections;
using UnityEngine;

public abstract class BulletClass : MonoBehaviour
{
    protected static ParticleSystem HitEffect;
    protected int damage;

    public abstract void collide();

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy")
        {
            other.GetComponent<EnemyClass>().DamageEntity(damage);
            if (!this.name.Contains("BoltBullet"))
            {
                collide();
            }
        }
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
