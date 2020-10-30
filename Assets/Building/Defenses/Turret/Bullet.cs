using UnityEngine;

public class Bullet : BulletClass
{
    public ParticleSystem Effect;

    public void Start()
    {
        HitEffect = Effect;
        StartCoroutine(SetLifetime(.5f));
    }

    public override void collide()
    { 
        Instantiate(HitEffect, transform.position, Quaternion.Euler(0, 0, transform.localEulerAngles.z + 180f));
        Destroy(gameObject);
    }

}
