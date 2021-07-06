using UnityEngine;

public class Solar : TileClass
{
    public Survival SRV;
    public int amount;

    public void Start()
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
