using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Enemy : BaseEntity
{
    // Runtime variables
    public float damage { get; set; }
    public float moveSpeed { get; set; }

    // Class variables
    public EnemyData enemyData { get; set; }
    public VariantStats variantStats { get; set; }
    public BaseEntity target { get; set; }

    // Object variables
    public Transform rotator;
    public bool gradualRotation = false;

    // Runtime flags
    private bool rotationHolder = false;
    public bool purified { get; set; }

    // Sprite info
    public SpriteRenderer[] border;
    public SpriteRenderer[] fill;
    public TrailRenderer[] trail;

    public virtual void Setup(EnemyData enemyData, Variant variant)
    {
        // Set the scriptable info
        this.enemyData = enemyData;
        variantStats = enemyData.variants[variant];

        // Setup variables
        damage = variantStats.damage;
        moveSpeed = variantStats.moveSpeed;
        health = variantStats.health;
        maxHealth = health;

        // Set rotation holder
        rotationHolder = gradualRotation;

        // Setup the materials
        foreach (SpriteRenderer a in border)
            a.material = variantStats.border;
        foreach (SpriteRenderer a in fill)
            a.material = variantStats.fill;
        foreach (TrailRenderer a in trail)
            a.material = variantStats.trail;
    }

    // Damages the entity (IDamageable interface method)
    public override void DamageEntity(float dmg)
    {
        health -= dmg;
        if (health <= 0) SyncDestroy();
        else Events.active.EnemyHurt(this);
    }

    public override void DestroyEntity()
    {
        // Create particle and set material / trail material
        ParticleSystemRenderer holder = Instantiate(variantStats.particle, transform.position,
            Quaternion.identity).GetComponent<ParticleSystemRenderer>();
        holder.material = variantStats.border;
        holder.trailMaterial = variantStats.border;

        // Spawn any enemies it's supposed to
        if (enemyData.spawnsOnDeath.Length > 0)
            Events.active.EnemySpawnOnDeath(enemyData, transform.position);

        // Invoke enemy death event
        Events.active.EnemyDestroyed(this);

        // Update unlockables
        Buildables.UpdateEntityUnlockables(Unlockable.UnlockType.DestroyEnemyAmount, enemyData, 1);

        // Check if purified
        if (purified)
        {
            // Choose random currency
            int random = Random.Range(0, 3);

            // Determine which resource to add
            switch(random)
            {
                case 0:
                    if (Resource.active.GetStorage(Resource.Type.Gold) > 0)
                    {
                        Resource.active.Apply(Resource.Type.Gold, 1000, true);
                        PopupHandler.active.CreatePopup(transform.position, Resource.Type.Gold, "+" + 1000);
                    }
                    break;
                case 1:
                    if (Resource.active.GetStorage(Resource.Type.Essence) > 0)
                    {
                        Resource.active.Apply(Resource.Type.Essence, 250, true);
                        PopupHandler.active.CreatePopup(transform.position, Resource.Type.Essence, "+" + 250);
                    }
                    break;
                case 2:
                    if (Resource.active.GetStorage(Resource.Type.Iridium) > 0)
                    {
                        Resource.active.Apply(Resource.Type.Iridium, 100, true);
                        PopupHandler.active.CreatePopup(transform.position, Resource.Type.Iridium, "+" + 100);
                    }
                    break;
            }
        }

        // Destroy game object
        Destroy(gameObject);
    }

    // Move towards target
    public virtual void MoveTowards(Transform obj, Transform target)
    {
        float step = moveSpeed * Time.deltaTime;
        obj.position = Vector2.MoveTowards(obj.position, target.position, step);

        if (gradualRotation) GradualRotation();
    }

    // Rotates towards a target
    public void RotateToTarget()
    {
        Vector3 dir = transform.position - target.transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle + 90f, Vector3.forward);
    }

    // Gradual rotation
    public void GradualRotation()
    {
        Vector3 targetDir = target.transform.position - transform.position;
        float angle = Mathf.Atan2(targetDir.y, targetDir.x) * Mathf.Rad2Deg - 90f;
        Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * enemyData.rotationSpeed);
    }

    // Set target
    public void SetTarget(BaseEntity tile, bool gradual)
    {
        target = tile;
        gradualRotation = gradual || rotationHolder;

        if (!gradualRotation)
            RotateToTarget();
    }

    // If a collision is detected, destroy the other entity and apply damage to self
    public virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (EnemyHandler.isMenu)
        {
            if (other is BoxCollider2D)
            {
                Destroy(gameObject);
                return;
            }
            else
            {
                DefaultBullet bullet = other.GetComponent<DefaultBullet>();
                if (bullet != null) Destroy(gameObject);

                BaseEntity building = other.GetComponent<BaseEntity>();
                if (building != null) building.OnCircleCollision(this);
            }
        }
        else
        {
            DefaultBullet bullet = other.GetComponent<DefaultBullet>();
            if (bullet != null) bullet.OnCollision(this);

            BaseEntity building = other.GetComponent<BaseEntity>();
            if (building != null)
            {
                if (other is BoxCollider2D) building.OnBoxCollision(this);
                else building.OnCircleCollision(this);
            }
        }
    }

    // If entity leaves defense range, remove self from target list
    public virtual void OnTriggerExit2D(Collider2D other)
    {
        BaseEntity building = other.GetComponent<BaseEntity>();
        if (building != null && other is CircleCollider2D)
            building.OnCircleLeave(this);
    }

    // Get material
    public override Material GetMaterial()
    {
        return variantStats.border;
    }
}
