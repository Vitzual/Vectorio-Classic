using UnityEngine;

public class Turbine : TileClass
{
    public Survival SRV;
    public int amount;

    // Internal placement variables
    public Transform rotator;
    public float speed;

    private void Start()
    {
        GameObject.Find("Rotation Handler").GetComponent<RotationHandler>().registerRotator(rotator, 150f);
        SRV = GameObject.Find("Survival").GetComponent<Survival>();
        SRV.increaseAvailablePower(amount);
    }

    public override void UpdatePower() { SRV.decreaseAvailablePower(amount); }
}
