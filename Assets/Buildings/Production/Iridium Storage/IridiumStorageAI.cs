using UnityEngine;

public class IridiumStorageAI: TileClass
{
    // Declare local object variables
    public int amount;
    public float grow;
    public bool growEnd;
    public Transform rotator;
    public Transform symbol;
    private Survival SRVSC;

    // On start, invoke repeating SendGold() method
    private void Start()
    {
        SRVSC = GameObject.Find("Survival").GetComponent<Survival>();
        SRVSC.iridiumStorage += amount;
        SRVSC.UI.IridiumStorage.text = SRVSC.iridiumStorage + " MAX";
    }

    // Rotates a thing
    private void Update()
    {
        rotator.Rotate(0, 0, 100 * Time.deltaTime);
        symbol.localScale = new Vector2(grow, grow);
        if (growEnd)
        {
            grow += 0.001f;
            if (grow >= 1f)
                growEnd = false;
        } 
        else
        {
            grow -= 0.001f;
            if (grow <= 0.8f)
                growEnd = true;
        }

    }

    // Kill defense
    public override void DestroyTile()
    {
        SRVSC.decreasePowerConsumption(power);
        SRVSC.UpdateGoldStorage(amount);
        Instantiate(Effect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
