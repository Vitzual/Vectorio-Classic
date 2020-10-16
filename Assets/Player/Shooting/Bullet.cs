using UnityEngine;

public class Bullet : MonoBehaviour
{
    public ParticleSystem hitEffect;

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


}
