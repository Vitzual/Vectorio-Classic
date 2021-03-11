using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BulletClass : MonoBehaviour
{
    protected ParticleSystem HitEffect;
    private bool pierced = false;
    public int damage;
    public float speed;

    public abstract void collide();

    public virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (transform.tag == "Bullet" && other.tag == "Enemy")
        {
            // Check to see if other enemy is spawner
            if (other.name == "Hive")
                other.GetComponent<SpawnerAI>().SpawnEnemy();
            else if (other.name == "Enemy Turret")
            {
                other.GetComponent<EnemyTurretAI>().DamageTile(damage);
                HitEffect = Resources.Load<ParticleSystem>("Particles/EnemyParticle");
                if(!this.name.Contains("BoltBullet") && !this.name.Contains("Beam")) collide();
                return;
            }
            else if (other.name == "Enemy Static")
            {
                other.GetComponent<EnemyWallAI>().DamageTile(damage);
                HitEffect = Resources.Load<ParticleSystem>("Particles/EnemyParticle");
                if (!this.name.Contains("BoltBullet") && !this.name.Contains("Beam")) collide();
                return;
            }
            
            other.GetComponent<EnemyClass>().DamageEntity(damage + Research.bonus_damage);
            if (!this.name.Contains("BoltBullet") && !this.name.Contains("Beam"))
            {
                // Set the particle effect when hitting enemy
                if (other.name.Contains("Dark"))
                    HitEffect = Resources.Load<ParticleSystem>("Particles/DarkParticle");
                else if (other.name.Contains("Phantom"))
                    HitEffect = Resources.Load<ParticleSystem>("Particles/PhantomParticle");
                else
                    HitEffect = Resources.Load<ParticleSystem>("Particles/EnemyParticle");

                // Check if piercing has been unlocked
                if (!pierced && Research.bonus_pierce) pierced = true;
                else collide();
            }
        }
        else if (transform.tag == "Enemy Bullet" && other.tag == "Defense")
        {
            Debug.Log("Hit " + other.name);
            other.GetComponent<TileClass>().DamageTile(damage);
            collide();
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

    public float GetSpeed()
    {
        return speed;
    }

    public void SetDamage(int a)
    {
        damage = a;
    }

    public void MultiplyDamage(float a)
    {
        damage = (int)(damage * a);
    }
}
