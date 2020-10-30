using UnityEngine;

public class ShotgunBullet : BulletClass
{

    public ParticleSystem Effect;

    public void Start()
    {
        HitEffect = Effect;
        StartCoroutine(SetLifetime(Random.Range(0.01f, 0.6f)));
    }

    public override void collide()
    {
        Instantiate(HitEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

}
