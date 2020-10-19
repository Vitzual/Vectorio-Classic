using UnityEngine;
using System.Collections;

public class TriangleScene : EnemyClass
{
    // Model components
    [SerializeField]
    private ParticleSystem Effect;
    [SerializeField]
    private ParticleSystem ChargeEffect;
    private Vector3 SpawnPosition;
    private Rigidbody2D Triangle;

    // Movement variables
    private Vector2 Movement;

    // On start, get rigidbody and assign death effect
    void Start()
    {
        Triangle = this.GetComponent<Rigidbody2D>();
        SpawnPosition = transform.position;
        health = 3;
        damage = 0;
        moveSpeed = 20f;
    }

    // Targetting system
    void Update()
    {
        // Find closest enemy 
        var target = DefensePool.FindClosestDefense(transform.position);
        float distance = DefensePool.FindClosestPosition(transform.position);
        if (target != null)
        {
            // Rotate towards current target
            Vector2 TargetPosition = new Vector2(target.gameObject.transform.position.x, target.gameObject.transform.position.y);

            // Move towards defense
            Vector2 lookDirection = TargetPosition - Triangle.position;

            float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg - 90f;
            Triangle.rotation = angle;
            lookDirection.Normalize();
            Movement = lookDirection;
        } 
        else
        {
            Movement = new Vector2(0, 0);
        }
    }

    // Move entity towards target every frame
    private void FixedUpdate()
    {
        Triangle.AddForce(Movement * moveSpeed);
    }

    // Kill entity
    public override void KillEntity()
    {
        Instantiate(Effect, transform.position, Quaternion.identity);
        var clone = Instantiate(this, SpawnPosition, Quaternion.identity);
        clone.name = "Triangle";
        Destroy(gameObject);
    }

    // Wait x amount of time
    IEnumerator SetChargeup(int a)
    {
        yield return new WaitForSeconds(a);
        moveSpeed = 100f;
    }
}
