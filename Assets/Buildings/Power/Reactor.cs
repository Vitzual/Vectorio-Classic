using UnityEngine;

public class Reactor : TileClass
{
    public Survival SRV;
    public int amount;

    private void Start()
    {
        SRV = GameObject.Find("Survival").GetComponent<Survival>();
        SRV.increaseAvailablePower(amount);
    }

    // Kill defense
    public override void UpdatePower()
    {
        SRV.decreaseAvailablePower(amount);
    }
}
