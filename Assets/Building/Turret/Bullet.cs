using UnityEngine;

public class Bullet : MonoBehaviour
{
    public ParticleSystem hitEffect;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag != "Defense" && other.tag != "Bullet")
        {
            Instantiate(hitEffect, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }


}
