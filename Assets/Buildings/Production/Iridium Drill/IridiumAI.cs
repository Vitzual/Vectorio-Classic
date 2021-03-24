using UnityEngine;

public class IridiumAI: TileClass
{
    // Declare local object variables
    public int amount;
    public bool enhanced;
    private GameObject SRVSC;

    public float sizeTracker = 1f;
    public bool sizeGrowing = false;

    // On start, invoke repeating SendGold() method
    private void Start()
    {
        SRVSC = GameObject.Find("Survival");
        InvokeRepeating("SendIridium", 0f, 10f);
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
    private void SendIridium()
    {
        if (enhanced) SRVSC.GetComponent<Survival>().AddIridium(amount * 4);
        else SRVSC.GetComponent<Survival>().AddIridium(amount);
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
        sizeTracker = 1f;
        transform.localScale = new Vector2(1, 1);
        sizeGrowing = false;
    }

    // Kill defense
    public override void DestroyTile()
    {
        GameObject.Find("Survival").GetComponent<Survival>().decreasePowerConsumption(power);
        GameObject.Find("Spawner").GetComponent<WaveSpawner>().decreaseHeat(heat);
        Instantiate(Effect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
