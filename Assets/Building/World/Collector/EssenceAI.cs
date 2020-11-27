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
        var colliders = Physics2D.OverlapBoxAll(this.gameObject.transform.position, new Vector2(7, 7), 1 << LayerMask.NameToLayer("Defense"));
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].name.Contains("Enhancer"))
            {
                increaseAmount(4);
            }
        }

        SRVSC = GameObject.Find("Survival");
        InvokeRepeating("SendEssence", 0f, 2f);
    }

    // Send gold
    private void SendEssence()
    {
        SRVSC.GetComponent<Survival>().AddEssence(amount);
        SRVSC.GetComponent<Survival>().UpdateGui();
    }

    // Increase gold
    public void increaseAmount(int a)
    {
        amount += a;
    }

    // Decrease gold
    public void decreaseAmount(int a)
    {
        amount -= a;
    }

    // Kill defense
    public override void DestroyTile()
    {
        GameObject.Find("Survival").GetComponent<Survival>().decreasePowerConsumption(power);
        Instantiate(Effect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
