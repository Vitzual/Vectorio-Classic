using UnityEngine;

public class Cooler : DefaultBuilding
{
    // Internal placement variables
    public Transform rotator1;
    public float speed1;
    public Transform rotator2;
    public float speed2;

    // Update is called once per frame
    void Start()
    {
        RotationHandler rotationHandler = GameObject.Find("Rotation Handler").GetComponent<RotationHandler>();
        rotationHandler.registerRotator(rotator1, speed1);
        if (rotator2 != null) rotationHandler.registerRotator(rotator2, speed2);
    }
}
