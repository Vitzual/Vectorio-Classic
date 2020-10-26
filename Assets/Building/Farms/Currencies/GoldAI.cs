using System.Collections;
using UnityEngine;

public class GoldAI : MonoBehaviour
{
    public int value = 1;

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name == "Hub")
        {
            Survival player = GameObject.Find("Survival").GetComponent<Survival>();
            player.AddGold(value);
            StartCoroutine(DestroyAfter(1));
        }
    }

    protected IEnumerator DestroyAfter(float a)
    {
        yield return new WaitForSeconds(a);
        Destroy(gameObject);
    }
}
