using UnityEngine;

/*
 * A lot of comments in this document is logic from when we were toying
 * with the idea of logistics. Ignore most of it.
 */

public class CollectorAI: TileClass
{
    // Declare local object variables
    public int amount;
    // public int rotation;
    public bool enhanced;
    // public GameObject Gold;
    // private Vector3 Destination;
    //private GoldAI GoldScript;
    private Survival SRVSC;
    public LayerMask TileLayer;

    // On start, invoke repeating SendGold() method
    private void Start()
    {
        RaycastHit2D resourceCheck = Physics2D.Raycast(transform.position, Vector2.zero, Mathf.Infinity, 1 << LayerMask.NameToLayer("Resource"));
        if (resourceCheck.collider != null && resourceCheck.collider.name == "Goldtile") doubleAmount();

        // Get destination
        /*
        if (transform.localEulerAngles.z >= 270f)
        {
            Destination = new Vector3(transform.position.x - 5, transform.position.y, 0);
            rotation = 4;
        }
        else if (transform.localEulerAngles.z >= 180f)
        {
            Destination = new Vector3(transform.position.x , transform.position.y + 5, 0);
            rotation = 1;
        }
        else if (transform.localEulerAngles.z >= 90f)
        {
            Destination = new Vector3(transform.position.x + 5, transform.position.y, 0);
            rotation = 2;
        }
        else
        {
            Destination = new Vector3(transform.position.x, transform.position.y - 5, 0);
            rotation = 3;
        }
        */

        SRVSC = GameObject.Find("Survival").GetComponent<Survival>();
        //GoldScript = GameObject.Find("Manager").GetComponent<GoldAI>();
        InvokeRepeating("SendGold", 0f, 1f);
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
    }

    // Deenhance collector
    public void deenhanceCollector()
    {
        enhanced = false;
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
