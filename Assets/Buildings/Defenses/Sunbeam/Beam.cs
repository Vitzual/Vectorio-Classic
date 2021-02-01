using UnityEngine;

public class Beam : BulletClass
{
    public ParticleSystem Effect;

    public void Start()
    {
        HitEffect = Effect;
        StartCoroutine(SetLifetime(Random.Range(1f, 2f)));
    }

    public override void collide()
    {
        Instantiate(HitEffect, transform.position, Quaternion.Euler(0, 0, transform.localEulerAngles.z + 180f));
        Destroy(gameObject);
    }

}
