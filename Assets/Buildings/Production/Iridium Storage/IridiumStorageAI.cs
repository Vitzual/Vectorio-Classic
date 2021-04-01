using UnityEngine;

public class IridiumStorageAI: TileClass
{
    // Declare local object variables
    public int amount;
    public float grow;
    public bool growEnd;
    public Transform symbol;
    private Survival SRVSC;

    // Internal placement variables
    public Transform rotator;
    public float speed;

    // Update is called once per frame
    void Start()
    {
        BuildingHandler.buildings.Add(transform);
        GameObject.Find("Rotation Handler").GetComponent<RotationHandler>().registerRotator(rotator, speed);
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
            if (grow >= 0.25f)
                growEnd = false;
        } 
        else
        {
            grow -= 0.001f;
            if (grow <= 0.2f)
                growEnd = true;
        }
    }

    // Kill defense
    public override void DestroyTile()
    {
        SRVSC.decreasePowerConsumption(power);
        BuildingHandler.buildings.Remove(transform);
        SRVSC.UpdateGoldStorage(amount);
        GameObject.Find("Spawner").GetComponent<WaveSpawner>().decreaseHeat(heat);
        Instantiate(Effect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
