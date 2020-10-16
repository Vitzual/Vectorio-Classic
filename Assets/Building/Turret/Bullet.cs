using UnityEngine;

public class Bullet : MonoBehaviour
{
    public ParticleSystem hitEffect;
    public Transform bullet;

    private void Start()
    {
        Physics2D.IgnoreCollision(bullet.GetComponent<BoxCollider2D>(), GetComponent<BoxCollider2D>());
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Apply knockback force to enemy entity
        GameObject other = collision.gameObject;
        if (other.tag == "Enemy")
        {
            other.GetComponent<Rigidbody2D>().AddForce(other.transform.position * -3f);
        }

        Debug.Log(collision.gameObject.name);

        Instantiate(hitEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);

    }


}
