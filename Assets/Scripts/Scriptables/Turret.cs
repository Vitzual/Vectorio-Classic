using UnityEngine;

[CreateAssetMenu(fileName = "New Turret", menuName = "Building/Turret")]
public class Turret : Building
{
    // Base turret stat variables
    [Header("Turret Variables")]
    public float damage;
    public float range;
    public float rotationSpeed;
    public float cooldown;

    [Header("Bullet Variables")]
    public DefaultBullet bullet;
    public int bulletPierces;
    public int bulletAmount;
    public float bulletSpeed;
    public float bulletSpread;
    public float bulletTime;
    public bool bulletLock;
    public ParticleSystem bulletParticle;
    public AudioClip sound;

    // Set panel stats
    // This gets used to set the stats on the building menu panel
    public override void CreateStats(Panel panel)
    {
        panel.CreateStat(new Stat("Health", health, 0, Sprites.GetSprite("Health")));
        panel.CreateStat(new Stat("Damage", damage, 0, Sprites.GetSprite("Damage")));
        panel.CreateStat(new Stat("Range", range, 0, Sprites.GetSprite("Range")));
        panel.CreateStat(new Stat("Firerate", cooldown, 0, Sprites.GetSprite("Firerate")));
        panel.CreateStat(new Stat("Pierces", bulletPierces, 0, Sprites.GetSprite("Pierces")));
        panel.CreateStat(new Stat("Bullets", bulletAmount, 0, Sprites.GetSprite("Bullets")));
        panel.CreateStat(new Stat("Spread", bulletSpread, 0, Sprites.GetSprite("Spread")));

        // Base method
        base.CreateStats(panel);
    }
}
