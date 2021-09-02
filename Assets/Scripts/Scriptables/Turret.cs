using UnityEngine;

[CreateAssetMenu(fileName = "New Turret", menuName = "Buildings/Turret")]
public class Turret : Building
{
    // IAudible interface variables
    public AudioClip sound;

    // Base turret stat variables
    public float damage;
    public float range;
    public float rotationSpeed;
    public float fireRate;
    public int bulletPierces;
    public int bulletAmount;
    public float bulletSpeed;
    public float bulletSpread;

    // Base turret modifiers
    [HideInInspector] public float damageModifier;
    [HideInInspector] public float rangeModifier;
    [HideInInspector] public float rotationSpeedModifier;
    [HideInInspector] public float fireRateModifier;
    [HideInInspector] public int bulletPiercesModifier;
    [HideInInspector] public int bulletAmountModifier;
    [HideInInspector] public float bulletSpeedModifier;
    [HideInInspector] public float bulletSpreadModifier;

    // Set panel stats
    // This gets used to set the stats on the building menu panel
    public override void CreateStat()
    {
        // Base method
        base.CreateStat();

        UIEvents.active.CreateStat(new Stat("Damage", damage, damageModifier, Sprites.active.damage));
        UIEvents.active.CreateStat(new Stat("Range", range, damageModifier, Sprites.active.damage));
        UIEvents.active.CreateStat(new Stat("Firerate", fireRate, damageModifier, Sprites.active.damage));
        UIEvents.active.CreateStat(new Stat("Bullet Pierces", bulletPierces, damageModifier, Sprites.active.damage));
        UIEvents.active.CreateStat(new Stat("Bullet Amount", bulletAmount, damageModifier, Sprites.active.damage));
        UIEvents.active.CreateStat(new Stat("Bullet Speed", bulletSpeed, damageModifier, Sprites.active.damage));
        UIEvents.active.CreateStat(new Stat("Bullet Spread", bulletSpread, damageModifier, Sprites.active.damage));
    }
}
