using UnityEngine;

public class RipperAI : TurretClass
{
    public Transform rotator;
    private bool firstAwake = true;

    // Update is called once per frame
    private void Awake()
    {
        if (!firstAwake) return;
        firstAwake = false;
        GameObject.Find("Rotation Handler").GetComponent<RotationHandler>().registerRotator(rotator, 50f);
    }

    // Targetting system
    void Update()
    {
        // If animation is playing, wait
        if (animPlaying)
        {
            PlayAnim();
            return;
        }

        if (isRotating)
            RotationHandler();

        // If a target exists, shoot at it
        if (target != null && !isRotating)
        {
            // Unflag hasTarget
            hasTarget = false;
                
            // Call shoot function
            Shoot(Bullet, FirePoints[0]);
        } else {
            // Unflag hasTarget when target is null
            hasTarget = false;
        }
    }
}
