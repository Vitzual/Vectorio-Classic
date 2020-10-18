using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    public ParticleSystem hitEffect;
    public float MoveSpeed = 3f;
    private Vector2 TargetPosition;
    private Vector2 Movement;
    private Rigidbody2D rocket;
    protected int range = 20;

    void Start()
    {
        rocket = this.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void OnCollisionEnter2D(Collision2D collision)
    {
        // Apply knockback force to enemy entity
        GameObject other = collision.gameObject;
        if (other.tag == "Enemy")
        {
            other.GetComponent<Rigidbody2D>().AddForce(other.transform.position * -3f);
        }

        Instantiate(hitEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    void Update()
    {
       
        // Find closest enemy 
         var target = EnemyPool.FindClosestEnemy(transform.position, range);

        // Rotate towards current target
        TargetPosition = new Vector2(target.gameObject.transform.position.x, target.gameObject.transform.position.y);

        // Move towards defense
        Vector2 lookDirection = TargetPosition - rocket.position;
        float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg - 90f;
        lookDirection.Normalize();
        Movement = lookDirection;
    }

    // Move entity towards target every frame
    private void FixedUpdate()
    {
        rocket.AddForce(Movement * MoveSpeed);
    }
}
