using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricBullet : DefaultBullet
{
    // Particle system
    public ParticleSystemRenderer particle;

    // Setup bullet
    public override void Setup(Turret turret, Sprite model = null)
    {
        this.turret = turret;

        particle = GetComponent<ParticleSystemRenderer>();
        particle.material = turret.material;
        particle.trailMaterial = turret.material;

        damage = turret.damage + Research.damageBoost;
        speed = Random.Range(turret.bulletSpeed - 2, turret.bulletSpeed + 2);
        pierces = turret.bulletPierces + Research.pierceBoost;

        time = turret.bulletTime;
    }
}
