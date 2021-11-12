using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "New Turret", menuName = "Building/Turret")]
public class Turret : Building
{
    // Base turret stat variables
    [FoldoutGroup("Turret Variables")]
    public float damage, range, rotationSpeed, cooldown;
    [FoldoutGroup("Turret Variables")]
    public AudioClip sound;
    
    // Bullet variables
    [FoldoutGroup("Bullet Variables")]
    public DefaultBullet bullet;
    [FoldoutGroup("Bullet Variables")]
    public int bulletPierces, bulletAmount;
    [FoldoutGroup("Bullet Variables")]
    public float bulletSpeed, bulletSpread, bulletTime;
    [FoldoutGroup("Bullet Variables")]
    public bool bulletLock;
    [FoldoutGroup("Bullet Variables")]
    public ParticleSystem bulletParticle;

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
