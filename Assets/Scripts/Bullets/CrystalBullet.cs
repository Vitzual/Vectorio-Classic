using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalBullet : DefaultBullet
{
    // Split bullet
    [SerializeField] public DefaultBullet crystalBullet;
    [SerializeField] public int bulletSplits;

    // Setup bullet
    public override void Setup(Turret turret, Cosmetic.Bullet bullet = null)
    {
        this.turret = turret;
        this.model = GetComponent<SpriteRenderer>();

        damage = turret.damage * Research.damageBoost;
        speed = Random.Range(turret.bulletSpeed - 2, turret.bulletSpeed + 2);
        pierces = turret.bulletPierces + Research.pierceBoost;

        time = turret.bulletTime;
    }

    public override void DestroyBullet(Material material, BaseEntity entity)
    {
        // Create bullets
        for (int i = 0; i < bulletSplits; i++)
        {
            DefaultBullet holder = Instantiate(crystalBullet, transform.position, transform.rotation).GetComponent<DefaultBullet>();
            holder.transform.Rotate(0f, 0f, Random.Range(0, 360));
            holder.Setup(turret);
            if (entity != null) holder.ignoreList.Add(entity);
            Events.active.BulletFired(holder);
        }

        base.DestroyBullet(material);
    }
}
