using UnityEngine;

public class CollectorAI: TileClass
{
    // Declare local object variables
    [SerializeField] private LayerMask ResourceLayer;
    public int amount;
    public bool enhanced;
    private GameObject SRVSC;

    // On start, invoke repeating SendGold() method
    private void Start()
    {
        RaycastHit2D resourceCheck = Physics2D.Raycast(transform.position, Vector2.zero, Mathf.Infinity, ResourceLayer);
        if (resourceCheck.collider != null && resourceCheck.collider.name == "Goldtile") doubleAmount();

        var colliders = Physics2D.OverlapBoxAll(this.gameObject.transform.position, new Vector2(7, 7), 1 << LayerMask.NameToLayer("Defense"));
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].name == "Enhancer")
            {
                doubleAmount();
            }
            else if (colliders[i].name == "Enhancer MK2")
            {
                doubleAmount();
            }
        }

        SRVSC = GameObject.Find("Survival");
        InvokeRepeating("SendGold", 0f, 1f);
    }

    // Send gold
    private void SendGold()
    {
        SRVSC.GetComponent<Survival>().AddGold(amount);
        SRVSC.GetComponent<Survival>().UpdateGui();
    }

    // Increase gold
    public void increaseAmount(int a)
    {
        amount += a;
    }

    // Increase gold
    public void doubleAmount()
    {
        amount = amount * 2;
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
