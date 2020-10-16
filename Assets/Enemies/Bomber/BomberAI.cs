using UnityEngine;

public class BomberAI : EntityClass
{

    [SerializeField]
    private ParticleSystem Effect;
    private Rigidbody2D Bomber;
    private Vector2 Movement;
    private Vector2 TargetPosition;
    public float MoveSpeed = 5f;

    // Start is called before the first frame update
    public BomberAI()
    {
        health = 5;
        damage = 1;
        DeathEffect = Effect;
    }

    // On start, get rigidbody and assign death effect
    void Start()
    {
        Bomber = this.GetComponent<Rigidbody2D>();
        DeathEffect = Effect;
    }

    void Update()
    {
        // Spins bomber
        transform.Rotate(0, 0, 100 * Time.deltaTime);

        // Find closest enemy 
        var target = DefensePool.FindClosestDefense(transform.position);

        // Rotate towards current target
        TargetPosition = new Vector2(target.gameObject.transform.position.x, target.gameObject.transform.position.y);

        // Move towards defense
        Vector2 lookDirection = TargetPosition - Bomber.position;
        float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg - 90f;
        lookDirection.Normalize();
        Movement = lookDirection;
    }

    // Move entity towards target every frame
    private void FixedUpdate()
    {
        Bomber.AddForce(Movement * MoveSpeed);
    }
}
