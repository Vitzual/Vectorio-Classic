using UnityEngine;

public class IridiumStorageAI: TileClass
{
    // Declare local object variables
    public int amount;
    public Transform rotator;
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
        rotator.Rotate(0, 0, 300 * Time.deltaTime);
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
