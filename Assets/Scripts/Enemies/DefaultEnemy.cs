using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class DefaultEnemy : BaseEntity
{
    // Class variables
    public Enemy enemy;
    public float moveSpeed;
    public float damage;
    [HideInInspector] public bool isMenu;
    [HideInInspector] public Variant variant;
    [HideInInspector] public BaseEntity target;
    public Transform rotator;
    public bool gradualRotation = false;
    [HideInInspector] public bool rotationHolder = false;
    public bool purified = false;

    // Sprite info
    public SpriteRenderer[] border;
    public SpriteRenderer[] fill;
    public TrailRenderer[] trail;

    public override void Setup()
    {
        // Check applied variant
        if (variant == null)
        {
            Debug.Log("Enemy was instantiated with a null variant!");
            Destroy(gameObject);
            return;
        }

        foreach (SpriteRenderer a in border)
            a.material = variant.border;

        foreach (SpriteRenderer a in fill)
            a.material = variant.fill;

        foreach (TrailRenderer a in trail)
            a.material = variant.trail;

        damage = enemy.damage * variant.damageModifier;
        moveSpeed = enemy.moveSpeed * variant.speedModifier;
        health = enemy.health * variant.healthModifier;
        maxHealth = health;

        rotationHolder = gradualRotation;
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
        ParticleSystemRenderer holder = Instantiate(variant.particle, transform.position,
            Quaternion.identity).GetComponent<ParticleSystemRenderer>();
        holder.material = variant.border;
        holder.trailMaterial = variant.border;

        // Spawn any enemies it's supposed to
        if (enemy.spawnsOnDeath.Length > 0)
            Events.active.EnemySpawnOnDeath(enemy, transform.position);

        // Invoke enemy death event
        Events.active.EnemyDestroyed(this);

        // Update unlockables
        Buildables.UpdateEntityUnlockables(Unlockable.UnlockType.DestroyEnemyAmount, enemy, 1);

        // Check if purified
        if (purified)
        {
            // Choose random currency
            int random = Random.Range(0, 3);

            // Determine which resource to add
            switch(random)
            {
                case 0:
                    if (Resource.active.GetStorage(Resource.CurrencyType.Gold) > 0)
                    {
                        Resource.active.Apply(Resource.CurrencyType.Gold, 1000, true);
                        PopupHandler.active.CreatePopup(transform.position, Resource.CurrencyType.Gold, "+" + 1000);
                    }
                    break;
                case 1:
                    if (Resource.active.GetStorage(Resource.CurrencyType.Essence) > 0)
                    {
                        Resource.active.Apply(Resource.CurrencyType.Essence, 250, true);
                        PopupHandler.active.CreatePopup(transform.position, Resource.CurrencyType.Essence, "+" + 250);
                    }
                    break;
                case 2:
                    if (Resource.active.GetStorage(Resource.CurrencyType.Iridium) > 0)
                    {
                        Resource.active.Apply(Resource.CurrencyType.Iridium, 100, true);
                        PopupHandler.active.CreatePopup(transform.position, Resource.CurrencyType.Iridium, "+" + 100);
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
        transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * enemy.rotationSpeed);
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
        if (isMenu)
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
        return variant.border;
    }
}
