using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;

public class TriangleAI : EntityClass
{
    [SerializeField]
    private ParticleSystem effect;
    public float moveSpeed = 5f;
    private Rigidbody2D rb;
    private Vector2 movement;
    private Transform player;

    public static List<TriangleAI> Instances { get; } = new List<TriangleAI>(100);
    public Vector2 position { get { return _transform.position; } }
    Transform _transform;

    public TriangleAI()
    {
        health = 5;
        damage = 1;
        DeathEffect = effect;
    }

    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        DeathEffect = effect;
    }

    void Awake()
    {
        _transform = transform;
    }

    void OnEnable()
    {
        Instances.Add(this);
    }

    void OnDisable()
    {
        Instances.Remove(this);
    }

    void Update()
    {
        Vector3 direction = player.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        rb.rotation = angle;
        direction.Normalize();
        movement = direction;
    }

    private void FixedUpdate()
    {
        moveAI(movement);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Apply knockback force to enemy entity
        GameObject other = collision.gameObject;
        if (other.tag == "Bullet")
        {
            DamageEntity();
        }
    }

    private void moveAI(Vector2 direction)
    {
        GetComponent<Rigidbody2D>().AddForce(direction * moveSpeed);
    }

    public static TriangleAI FindNearest(Vector2 point)
    {
        TriangleAI nearest = null;
        Profiler.BeginSample("FindNearest()");
        {
            Vector2 nearestPosition = Vector2.zero;
            float nearestDistance = float.PositiveInfinity;
            {
                int instancesCount = Instances.Count;
                int i = 0;
                if (instancesCount > 0)
                {
                    nearest = Instances[0];
                    nearestPosition = nearest._transform.position;
                    nearestDistance = Vector2.Distance(point, nearestPosition);
                    i = 1;
                }
                for (; i < instancesCount; i++)
                {
                    TriangleAI next = Instances[i];
                    Vector2 nextPosition = next._transform.position;
                    float dist = Vector2.Distance(point, nextPosition);
                    if (dist < nearestDistance)
                    {
                        nearest = next;
                        nearestPosition = next._transform.position;
                        nearestDistance = dist;
                    }
                }
            }
        }
        Profiler.EndSample();

        return nearest;
    }

}
