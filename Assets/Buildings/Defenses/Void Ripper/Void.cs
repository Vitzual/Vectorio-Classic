using UnityEngine;

public class Void : BulletClass
{
    public ParticleSystem Effect;

    public void Start()
    {
        HitEffect = Effect;
        StartCoroutine(SetLifetime(Random.Range(4f, 6f)));
    }

    public override void collide()
    { 
        Instantiate(HitEffect, transform.position, Quaternion.Euler(0, 0, transform.localEulerAngles.z + 180f));
        Destroy(gameObject);
    }

}
