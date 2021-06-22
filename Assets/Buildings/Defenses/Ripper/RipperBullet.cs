using UnityEngine;

public class RipperBullet : BulletClass
{
    public ParticleSystem Effect;

    public void Start()
    {
        HitEffect = Effect;
        StartCoroutine(SetLifetime(Random.Range(1f, 1.5f)));
    }

    public override void collide()
    {
        Instantiate(HitEffect, transform.position, Quaternion.Euler(0, 0, transform.localEulerAngles.z + 180f));
        Destroy(gameObject);
    }

}
