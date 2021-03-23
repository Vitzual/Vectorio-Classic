using UnityEngine;

/*
 * A lot of comments in this document is logic from when we were toying
 * with the idea of logistics. Ignore most of it.
 */

public class CollectorAI: TileClass
{
    // Declare local object variables
    public int amount;
    public bool enhanced;
    private Survival SRVSC;
    public LayerMask TileLayer;

    public float sizeTracker = 1f;
    public bool sizeGrowing = false;

    // On start, invoke repeating SendGold() method
    private void Start()
    {
        RaycastHit2D resourceCheck = Physics2D.Raycast(transform.position, Vector2.zero, Mathf.Infinity, 1 << LayerMask.NameToLayer("Resource"));
        if (resourceCheck.collider != null && resourceCheck.collider.name == "Goldtile") doubleAmount();

        SRVSC = GameObject.Find("Survival").GetComponent<Survival>();
        InvokeRepeating("SendGold", 0f, 1f);
    }

    private void Update()
    {
        if (enhanced)
        {
            transform.localScale = new Vector2(sizeTracker, sizeTracker);
            if (sizeGrowing)
            {
                sizeTracker += 0.0008f;
                if (sizeTracker >= 1.04f)
                    sizeGrowing = false;
            }
            else
            {
                sizeTracker -= 0.0008f;
                if (sizeTracker <= 1.0f)
                    sizeGrowing = true;
            }
        }
    }

    // Send gold
    private void SendGold()
    {
        // ConveyorScript.SetEntranceStatus(true);
        // GameObject Object = Instantiate(Gold, transform.position, Quaternion.Euler(new Vector3(0, 0, 0)));
        if (enhanced) SRVSC.AddGold(amount * 2);
        else SRVSC.AddGold(amount);
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
        gameObject.GetComponent<AnimateThenStop>().enabled = true;
    }

    // Deenhance collector
    public void deenhanceCollector()
    {
        enhanced = false;
        sizeTracker = 1f;
        transform.localScale = new Vector2(1, 1);
        sizeGrowing = false;
    }

    // Kill defense
    public override void DestroyTile()
    {
        SRVSC.decreasePowerConsumption(power);
        GameObject.Find("Spawner").GetComponent<WaveSpawner>().decreaseHeat(heat);
        Instantiate(Effect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
