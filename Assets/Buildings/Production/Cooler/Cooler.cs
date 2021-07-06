using UnityEngine;

public class Cooler : TileClass
{
    // Internal placement variables
    public Transform rotator;
    public float speed;

    // Update is called once per frame
    void Start()
    {
        GameObject.Find("Rotation Handler").GetComponent<RotationHandler>().registerRotator(rotator, speed);
    }
}
