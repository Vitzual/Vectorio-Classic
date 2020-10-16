using UnityEngine;

public class TriangleAI : EntityClass
{
    [SerializeField]
    private ParticleSystem Effect;
    private Rigidbody2D Triangle;
    private Vector2 Movement;
    private Vector2 TargetPosition;
    public float MoveSpeed = 5f;

    // Constructor method
    public TriangleAI()
    {
        health = 5;
        damage = 1;
        DeathEffect = Effect;
    }

    // On start, get rigidbody and assign death effect
    void Start()
    {
        Triangle = this.GetComponent<Rigidbody2D>();
        DeathEffect = Effect;
    }

    void Update()
    {
        // Find closest enemy 
        var target = DefensePool.FindClosestDefense(transform.position);

        // Rotate towards current target
        TargetPosition = new Vector2(target.gameObject.transform.position.x, target.gameObject.transform.position.y);

        // Move towards defense
        Vector2 lookDirection = TargetPosition - Triangle.position;
        float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg - 90f;
        Triangle.rotation = angle;
        lookDirection.Normalize();
        Movement = lookDirection;
    }

    // Move entity towards target every frame
    private void FixedUpdate()
    {
        Triangle.AddForce(Movement * MoveSpeed);
    }
}
