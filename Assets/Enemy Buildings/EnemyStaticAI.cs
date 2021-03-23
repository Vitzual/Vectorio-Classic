using UnityEngine;

public class EnemyStaticAI : TileClass
{
    public int amount = 0;

    // Kill defense
    public override void DestroyTile()
    {
        if (amount != 0)
        {
            GameObject survival = GameObject.Find("Survival");
            survival.GetComponent<Survival>().AddGold(amount);
            survival.GetComponent<Interface>().CreateResourcePopup("+ " + amount, "Gold", transform.position);
        }
        Instantiate(Effect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
