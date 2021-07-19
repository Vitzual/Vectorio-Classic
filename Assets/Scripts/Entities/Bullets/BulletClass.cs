using System.Collections;
using UnityEngine;

public abstract class BulletClass : MonoBehaviour
{
    protected ParticleSystem HitEffect;
    public bool isParticleChangeable = true;

    public abstract void collide();

    public void SetHitEffect(string name)
    {
        HitEffect = Resources.Load<ParticleSystem>("Particles/" + name);
    }

    public IEnumerator SetLifetime(float a)
    {
        yield return new WaitForSeconds(a);
        if (this != null)
        {
            collide();
        }
    }

    public bool IsParticleChangeable()
    {
        return isParticleChangeable;
    }
}
