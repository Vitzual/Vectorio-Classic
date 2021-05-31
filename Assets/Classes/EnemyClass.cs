using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public abstract class EnemyClass : MonoBehaviour
{
    // Enemy handler script
    public EnemyHandler enemyScript;
    
    // Enemy ID
    public int ID;
    
    // Effect variables
    private Transform burning_effect;
    private Transform freezing_effect;
    private Transform poison_effect;
    public bool is_burning = false;
    public bool is_frozen = false;
    public bool is_poisoned = false;

    // Enemy stats
    public int health;
    public int damage;
    public float moveSpeed;
    public int attackSpeed;
    public int explosiveRadius;
    public int explosiveDamage;
    public float rayLength;
    public bool effectImmunity = false;
    public GameObject[] spawnOnDeath;
    public int[] amountToSpawn;
    public ParticleSystem Effect;

    // Enemy vars
    public Transform PreferredTarget = null;
    public Transform target;
    protected int attackTimeout;

    private EnemyLocater scanner;

    // Start method
    protected void Start()
    {
        enemyScript = GameObject.Find("Enemy Handler").GetComponent<EnemyHandler>();
        enemyScript.RegisterEnemy(transform, moveSpeed, damage);
        scanner = GameObject.Find("Enemy Scanner").GetComponent<EnemyLocater>();
    }

    // Gets called when entering another defenses range or hitting the defense all together
    public void OnTriggerEnter2D(Collider2D collider)
    {
        if (Vector3.Distance(collider.transform.position, transform.position) >= 10 && collider.tag == "Defense")
        {
            if (collider.name != "Wall" && collider.name != "Drone Port")
            {
                TurretClass holder = collider.GetComponent<TurretClass>();
                if (!holder.enabled)
                {
                    holder.enabled = true;
                    holder.forceTarget(transform);
                }
            }
        }

        else if (collider.tag == "Defense" || collider.tag == "Building" || collider.tag == "Hub" || collider.tag == "Production")
        {
            int ID = enemyScript.RequestID(transform);
            if (ID != -1) enemyScript.OnHit(ID, collider.transform);
        }
    }

    // Gets called when entering another defenses range or hitting the defense all together
    public void OnTriggerLeave2D(Collider2D collider)
    {
        TurretClass holder = collider.GetComponent<TurretClass>();
        if (holder.enabled) holder.removeTarget(transform);
    }

    // Kill entity
    public virtual void KillEntity()
    {
        // If menu scene, re instantiate the object
        if (SceneManager.GetActiveScene().name == "Menu") {
            Instantiate(Effect, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
        else
        {
            // If has explosive damage, apply it to surrounding tiles
            if (explosiveRadius > 0 && explosiveDamage > 0)
            {
                var colliders = Physics2D.OverlapCircleAll(transform.position, explosiveRadius, 1 << LayerMask.NameToLayer("Building"));
                for (int i = 0; i < colliders.Length; i++)
                    if (Vector3.Distance(colliders[i].transform.position, transform.position) < 10)
                        colliders[i].GetComponent<TileClass>().DamageTile(explosiveDamage);
            }

            // If spawns on death, itterate through and spawn enemies
            if (spawnOnDeath.Length > 0)
            {
                if (amountToSpawn.Length != spawnOnDeath.Length)
                {
                    Debug.LogError("Custom error #001\n- Mismatched array size!");
                    return;
                }
                for (int a = 0; a < spawnOnDeath.Length; a++)
                {
                    for (int b = 0; b < amountToSpawn[a]; b++)
                    {
                        if (spawnOnDeath[a] == gameObject)
                        {
                            Debug.LogError("Custom error #002\n- Enemies cannot spawn themselves on death");
                        }
                        else
                        {
                            Instantiate(spawnOnDeath[a], new Vector2(transform.position.x + Random.Range(-6, 6), transform.position.y + Random.Range(-6, 6)), Quaternion.identity);
                        }
                    }
                }
            }

            // Check if the entity is a boss unit
            if (transform.name == "The Revenant") GameObject.Find("Spawner").GetComponent<WaveSpawner>().defeatBoss(0);

            // Instantiate death effect and destroy self
            Instantiate(Effect, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

    // Apply damage to entity
    public virtual void DamageEntity(int dmgRecieved)
    {
        // Apply damage and double if entity is poisoned
        if (is_poisoned) health -= dmgRecieved * 2;
        else health -= dmgRecieved;

        // Check health, if less then 0 kill entity
        if (health <= 0)
        {
             KillEntity();
             return;
        } 
        else if (!effectImmunity)
        {
            // Check to see if burning has been unlocked and if it should be applied
            if (!is_burning && Research.research_burning > 0)
            {
                if (Random.Range(0, 20) == 10)
                {
                    is_burning = true;
                    burning_effect = Instantiate(Resources.Load<Transform>("Effects/Fire"), transform.position, transform.rotation);
                    burning_effect.parent = transform;
                    StartCoroutine(ApplyBurning(Research.research_burning));
                }
            }

            // Check to see if freezing has been unlocked and if it should be applied
            if (!is_frozen && Research.research_freezing > 0)
            {
                if (Random.Range(0, 20) == 10)
                {
                    is_frozen = true;
                    freezing_effect = Instantiate(Resources.Load<Transform>("Effects/Ice"), transform.position, transform.rotation);
                    freezing_effect.parent = transform;
                    StartCoroutine(ApplyFreezing(Research.research_freezing));
                }
            }

            // Check to see if poisoning has been unlocked and if it should be applied
            if (!is_poisoned && Research.research_poisoning > 0)
            {
                if (Random.Range(0, 20) == 10)
                {
                    is_poisoned = true;
                    poison_effect = Instantiate(Resources.Load<Transform>("Effects/Poison"), transform.position, transform.rotation);
                    poison_effect.parent = transform;
                    StartCoroutine(ApplyPoisoning(Research.research_poisoning));
                }
            }
        }
    }

    // Apply burning effect
    public IEnumerator ApplyBurning(int amount)
    {
        // Wait for 1 second before applying burn
        yield return new WaitForSeconds(1);

        // Apply damage
        health -= 1;

        // Check health, if less then 0 kill entity
        if (health <= 0)
        {
            KillEntity();
        }

        // If entity still alive, check if still burning
        if (amount <= 0)
        {
            is_burning = false;
            Destroy(burning_effect.gameObject);
        }
        else StartCoroutine(ApplyBurning(amount - 1));
    }

    // Apply freezing effect
    public IEnumerator ApplyFreezing(int amount)
    {
        float speed_holder = moveSpeed;
        moveSpeed = moveSpeed / 2;
        yield return new WaitForSeconds(amount);
        Destroy(freezing_effect.gameObject);
        moveSpeed = speed_holder;
        is_frozen = false;
    }

    // Apply poisoning effect
    public IEnumerator ApplyPoisoning(int amount)
    {
        yield return new WaitForSeconds(amount);
        Destroy(poison_effect.gameObject);
        is_poisoned = false;
    }

    public void FindNearestDefence()
    {
        // Request a target
        target = scanner.requestTarget(transform.position, PreferredTarget);
        
        // Get target position relative to this entity
        Vector2 TargetPosition = new Vector2(target.position.x, target.position.y);

        // Get the direction towards that unit from this entity
        Vector2 lookDirection = TargetPosition - new Vector2(transform.position.x, transform.position.y);

        // Get the angle between the target and this transform
        transform.eulerAngles = new Vector3(0, 0, Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg - 90f);
    }

    public int GetID() { return ID; }
    public int GetHealth() { return health; }
    public void SetHealth(int a) { health = a; }
    public int GetDamage() { return damage; }
    public void SetDamage(int a) { damage = a; }
    public float GetSpeed() { return moveSpeed; }
    public void SetSpeed(float a) { moveSpeed = a; }
}
