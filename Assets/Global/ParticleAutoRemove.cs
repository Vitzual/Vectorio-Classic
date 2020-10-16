using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleAutoRemove : MonoBehaviour
{
    private ParticleSystem particle;

    public void Start()
    {
        particle = GetComponent<ParticleSystem>();
    }

    public void Update()
    {
        if (particle)
        {
            if (!particle.IsAlive())
            {
                Destroy(gameObject);
            }
        }
    }

}

