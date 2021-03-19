using UnityEngine;

public class EssenceAI: TileClass
{
    // Declare local object variables
    public int amount;
    public bool enhanced;
    private GameObject SRVSC;

    // On start, invoke repeating SendGold() method
    private void Start()
    {
        SRVSC = GameObject.Find("Survival");
        InvokeRepeating("SendEssence", 0f, 1f);
    }

    // Send gold
    private void SendEssence()
    {
        if (enhanced) SRVSC.GetComponent<Survival>().AddEssence(amount * 4);
        else SRVSC.GetComponent<Survival>().AddEssence(amount);
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
        GameObject.Find("Spawner").GetComponent<WaveSpawner>().decreaseHeat(heat);
        Instantiate(Effect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
