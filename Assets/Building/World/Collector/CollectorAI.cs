using UnityEngine;

public class CollectorAI: TileClass
{
    // Declare local object variables
    public int amount;
    private GameObject SRVSC;

    // On start, invoke repeating SendGold() method
    private void Start()
    {
        SRVSC = GameObject.Find("Survival");
        InvokeRepeating("SendGold", 0f, 1f);
    }

    // Send gold
    private void SendGold()
    {
        SRVSC.GetComponent<Survival>().AddGold(amount);
        SRVSC.GetComponent<Survival>().UpdateGui();
    }

    // Kill defense
    public override void DestroyTile()
    {
        Instantiate(Effect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
