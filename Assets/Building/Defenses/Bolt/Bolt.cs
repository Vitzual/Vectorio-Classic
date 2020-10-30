using UnityEngine;

public class Bolt : BulletClass
{

    public ParticleSystem Effect;

    public void Start()
    {
        HitEffect = Effect;
        StartCoroutine(SetLifetime(1));
    }

    public override void collide()
    {
        Instantiate(HitEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

}
