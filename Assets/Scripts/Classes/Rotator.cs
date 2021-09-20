using UnityEngine;

public class Rotator : MonoBehaviour
{
    public float speed;

    public void Start()
    {
        if (RotationHandler.active != null)
            RotationHandler.active.RegisterRotator(transform, speed);

        enabled = false;
    }
}
