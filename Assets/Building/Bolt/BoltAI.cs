using UnityEngine;

public class BoltAI : TurretDefense
{
    // Turret AI variables 
    public Transform Point;
    public Rigidbody2D Gun;
    public GameObject Bullet;

    // On start, assign weapon variables
    void Start()
    {
        fireRate = 2f;
        bulletForce = 100f;
        bulletSpread = 0f;
        bulletAmount = 1;
        range = 10000;
    }

    // Targetting system
    void Update()
    {
        // Find closest enemy 
        var target = EnemyPool.FindClosestEnemy(transform.position, range);

        // If a target exists, shoot at it
        if (target != null)
        {
            // Rotate turret towards target
            Vector2 TargetPosition = new Vector2(target.gameObject.transform.position.x, target.gameObject.transform.position.y);
            Vector2 lookDirection = (TargetPosition - Gun.position);
            float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg - 90f;

            // Smooth rotation when targetting enemies
            if (Gun.rotation >= angle && !((Gun.rotation - angle) <= 0.3 && (Gun.rotation - angle) >= -0.3))
            {
                Gun.rotation -= 0.3f;
            } 
            else if (Gun.rotation <= angle && !((Gun.rotation - angle) <= 0.3 && (Gun.rotation - angle) >= -0.3))
            {
                Gun.rotation += 0.3f;
            }
            
            // If turret is pointing at target, fire at it
            if ((Gun.rotation - angle) <= 1 && (Gun.rotation - angle) >= -1)
            {
                // Call shoot function
                Shoot(Bullet, Point);
            }
        }
    }
}
