using UnityEngine;
using System.Collections;
using UnityEditor.UI;

public class TriangleAI : EntityClass
{
    [SerializeField]
    private ParticleSystem Effect;
    [SerializeField]
    private ParticleSystem ChargeEffect;
    private Rigidbody2D Triangle;

    // Behavioural variables
    private Vector2 Movement;
    private Vector2 TargetPosition;
    private bool InRange = false;
    private float MoveSpeed = 10f;
    private int ProcRange = 500;
    private int ChargeTime = 3;

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
        float distance = DefensePool.FindClosestPosition(transform.position);

        // Rotate towards current target
        TargetPosition = new Vector2(target.gameObject.transform.position.x, target.gameObject.transform.position.y);

        // Move towards defense
        Vector2 lookDirection = TargetPosition - Triangle.position;

        float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg - 90f;
        Triangle.rotation = angle;
        lookDirection.Normalize();
        Movement = lookDirection;

        if (distance <= ProcRange && InRange == false)
        {
            InRange = true;
            MoveSpeed = 0;
            //ParticleSystem Charge = Instantiate(ChargeEffect, Triangle.position, Quaternion.Euler(-90f, 0, 0f), Triangle.transform);
            StartCoroutine(SetChargeup(ChargeTime));
        }

    }

    // If hit by bullet, take damage
    public void TakeDamage(int a)
    {
        DamageEntity(a);
    }

    // Move entity towards target every frame
    private void FixedUpdate()
    {
        Triangle.AddForce(Movement * MoveSpeed);
    }

    // Wait x amount of time
    IEnumerator SetChargeup(int a)
    {
        yield return new WaitForSeconds(a);
        MoveSpeed = 100f;
    }
}
