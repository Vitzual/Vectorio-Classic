using UnityEngine;

public class Bullet : MonoBehaviour
{
    public ParticleSystem hitEffect;

    // hit.collider.gameObject.GetComponent<SpaceThing>().Damage(10,0);

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag != "Defense" && other.tag != "Bullet")
        {
            if (other.name == "Triangle(Clone)")
            {
                other.GetComponent<TriangleAI>().TakeDamage(1);
            }
            Instantiate(hitEffect, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }


}
