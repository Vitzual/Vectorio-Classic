using UnityEngine;

public class Rocket : BulletClass
{
    public ParticleSystem Effect;
    public int explosionRadius;

    public void Start()
    {
        HitEffect = Effect;
        StartCoroutine(SetLifetime(.5f));
    }

    public override void collide()
    {
        Instantiate(HitEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    public override void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy")
        {
            var colliders = Physics2D.OverlapCircleAll(
                this.gameObject.transform.position, 
                explosionRadius, 
                1 << LayerMask.NameToLayer("Enemy"));
            foreach (var colider in colliders)
            {
                colider.GetComponent<EnemyClass>().DamageEntity(damage);
            }
            collide();
        }
    }

}
