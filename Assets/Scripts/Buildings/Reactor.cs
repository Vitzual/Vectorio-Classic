using UnityEngine;

public class Reactor : BaseBuilding
{
    public Survival SRV;
    public int amount;

    private void Start()
    {
        SRV = GameObject.Find("Survival").GetComponent<Survival>();
        SRV.increaseAvailablePower(amount);
    }
}
