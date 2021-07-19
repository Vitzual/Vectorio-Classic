using UnityEngine;

public class Rocket : BulletClass
{
    // Global variables
    protected GameObject target = null;
    protected Rigidbody2D body;
    public ParticleSystem Effect;
    public int explosionRadius;
    protected Vector2 Movement;

    public void Start()
    {
        body = this.GetComponent<Rigidbody2D>();
        HitEffect = Effect;
        StartCoroutine(SetLifetime(6f));
        //target = FindNearestEnemy();
    }

    public void Update()
    {
        if (target != null)
        {
            // Rotate towards current target
            float distance = (target.transform.position - this.transform.position).sqrMagnitude;
            Vector2 TargetPosition = new Vector2(target.gameObject.transform.position.x, target.gameObject.transform.position.y);

            // Move towards defense
            Vector2 lookDirection = TargetPosition - body.position;

            float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg - 90f;
            body.rotation = angle;
            lookDirection.Normalize();
            Movement = lookDirection;
        }
    }

    public override void collide()
    {
        Instantiate(HitEffect, transform.position, Quaternion.Euler(0, 0, transform.localEulerAngles.z + 180f));
        Destroy(gameObject);
    }

    /*
    public override void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy")
        {
            var colliders = Physics2D.OverlapCircleAll(
                this.gameObject.transform.position, 
                explosionRadius, 
                1 << LayerMask.NameToLayer("Enemy"));
            foreach (var colider in colliders)
            {
                if (other.name == "Enemy Turret")
                    colider.GetComponent<EnemyTurretAI>().DamageTile(damage);
                else if (other.name == "Enemy Static")
                    colider.GetComponent<EnemyWallAI>().DamageTile(damage);
                else
                    colider.GetComponent<EnemyClass>().DamageEntity(damage);
            }
            collide();
        }
    }
    */

    protected static float AlignRotation(float r)
    {
        float temp = r;
        while (temp < 0f)
        {
            temp += 360f;
        }
        while (temp > 360f)
        {
            temp -= 360f;
        }
        return temp;
    }

}
