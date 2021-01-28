using UnityEngine;

public class CollectorAI: TileClass
{
    // Declare local object variables
    public int amount;
    public int rotation;
    public bool enhanced;
    public GameObject Gold;
    private Vector3 Destination;
    private GoldAI GoldScript;
    private Survival SRVSC;
    public LayerMask TileLayer;

    // On start, invoke repeating SendGold() method
    private void Start()
    {
        //RaycastHit2D resourceCheck = Physics2D.Raycast(transform.position, Vector2.zero, Mathf.Infinity, 1 << LayerMask.NameToLayer("Resource"));
        //if (resourceCheck.collider != null && resourceCheck.collider.name == "Goldtile") doubleAmount();

        // Get destination
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

        SRVSC = GameObject.Find("Survival").GetComponent<Survival>();
        GoldScript = GameObject.Find("Manager").GetComponent<GoldAI>();
        InvokeRepeating("SendGold", 0f, 0.3f);
    }

    // Send gold
    private void SendGold()
    {
        // Create gold
        RaycastHit2D Target = Physics2D.Raycast(Destination, Vector2.zero, Mathf.Infinity, TileLayer);
        if (Target.transform == null || Target.transform.name != "Conveyor")
        {
            return;
        }
        ConveyorAI ConveyorScript = Target.transform.GetComponent<ConveyorAI>();
        if (!ConveyorScript.EntranceOccupied && ConveyorScript.ValidRotation(rotation))
        {
            ConveyorScript.SetEntranceStatus(true);
            GameObject Object = Instantiate(Gold, transform.position, Quaternion.Euler(new Vector3(0, 0, 0)));
            if (enhanced) GoldScript.RegisterNewCoin(Object.transform, ConveyorScript, ConveyorScript.GetEntranceLocation(), amount * 2);
            else GoldScript.RegisterNewCoin(Object.transform, ConveyorScript, ConveyorScript.GetEntranceLocation(), amount);
        }
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
        Instantiate(Effect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
