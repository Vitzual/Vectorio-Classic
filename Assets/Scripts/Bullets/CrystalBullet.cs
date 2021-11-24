using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalBullet : DefaultBullet
{
    // Split bullet
    [SerializeField] public DefaultBullet bullet;
    [SerializeField] public int bulletSplits;

    // Setup bullet
    public override void Setup(Turret turret, Sprite model = null)
    {
        this.turret = turret;
        this.model = GetComponent<SpriteRenderer>();

        if (model != null)
        {
            this.model.sprite = model;
            this.model.material = turret.material;
        }

        damage = turret.damage + Research.damage;
        speed = Random.Range(turret.bulletSpeed - 2, turret.bulletSpeed + 2);
        pierces = turret.bulletPierces + Research.pierce;

        time = turret.bulletTime;
    }

    public override void DestroyBullet(Material material, BaseEntity entity)
    {
        // Create bullets
        for (int i = 0; i < bulletSplits; i++)
        {
            DefaultBullet holder = Instantiate(bullet, transform.position, transform.rotation).GetComponent<DefaultBullet>();
            holder.transform.Rotate(0f, 0f, Random.Range(0, 360));
            holder.Setup(turret, model.sprite);
            if (entity != null) holder.ignoreList.Add(entity);
            Events.active.BulletFired(holder, null);
        }

        base.DestroyBullet(material);
    }
}
