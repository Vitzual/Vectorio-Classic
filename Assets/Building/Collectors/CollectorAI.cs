using UnityEngine;

public class CollectorAI: TileClass
{
    // Declare local object variables
    public int amount;
    public bool enhanced;
    private GameObject SRVSC;

    // On start, invoke repeating SendGold() method
    private void Start()
    {
        RaycastHit2D resourceCheck = Physics2D.Raycast(transform.position, Vector2.zero, Mathf.Infinity, 1 << LayerMask.NameToLayer("Resource"));
        if (resourceCheck.collider != null && resourceCheck.collider.name == "Goldtile") doubleAmount();

        SRVSC = GameObject.Find("Survival");
        InvokeRepeating("SendGold", 0f, 1f);
    }

    // Send gold
    private void SendGold()
    {
        if (enhanced) SRVSC.GetComponent<Survival>().AddGold(amount*4);
        else SRVSC.GetComponent<Survival>().AddGold(amount);
        SRVSC.GetComponent<Survival>().UpdateGui();
    }

    // Increase gold
    public void doubleAmount()
    {
        amount = amount * 2;
    }

    // Enhance collector
    public void enhanceCollector()
    {
        enhanced = true;
    }

    // Deenhance collector
    public void deenhanceCollector()
    {
        enhanced = false;
    }

    // Kill defense
    public override void DestroyTile()
    {
        GameObject.Find("Survival").GetComponent<Survival>().decreasePowerConsumption(power);
        Instantiate(Effect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
