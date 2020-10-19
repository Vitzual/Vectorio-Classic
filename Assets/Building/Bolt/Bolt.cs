using UnityEditor.UI;
using UnityEngine;

public class Bolt : BulletClass
{

    public ParticleSystem Effect;
    private int totalHits = 3;

    public void Start()
    {
        HitEffect = Effect;
        damage = 5;
        StartCoroutine(SetLifetime(1));
    }

    public override void collide()
    {
        Instantiate(HitEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

}
