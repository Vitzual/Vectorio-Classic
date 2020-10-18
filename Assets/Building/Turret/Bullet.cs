using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour
{
    public ParticleSystem HitEffect;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag != "Defense" && other.tag != "Bullet")
        {
            if (other.name == "Triangle(Clone)")
            {
                other.GetComponent<TriangleAI>().TakeDamage(1);
            }
            Instantiate(HitEffect, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

    public IEnumerator SetLifetime(int a)
    {
        yield return new WaitForSeconds(a);
        if (this != null)
        {
            Instantiate(hitEffect, this.gameObject.transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }
    }

}
