using UnityEngine;

public class ShotgunBullet : BulletClass
{

    public ParticleSystem Effect;

    public void Start()
    {
        HitEffect = Effect;
        damage = 3;
        StartCoroutine(SetLifetime(.45f));
    }

    public override void collide()
    {
        Instantiate(HitEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

}
