using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretHandler : MonoBehaviour
{
    // Active class
    public static TurretHandler active;

    // Barrel class
    public List<TurretEntity> turretEntities;

    // IRecoilAnim interface variables
    public bool animationEnabled { get; set; }
    public bool animPlaying { get; set; }
    public bool animRebound { get; set; }
    public int animTracker { get; set; }
    public int animHolder { get; set; }
    public float animMovement { get; set; }

    public void Start()
    {
        if (this != null)
            active = this;
    }

    // Plays the recoil animation (IRecoilAnim interface method)
    public void PlayRecoilAnimation(Transform obj)
    {
        if (!animRebound)
        {
            animTracker -= 1;
            obj.localPosition -= obj.up * animMovement * Time.deltaTime;
            if (animTracker == animHolder / 2)
            {
                animTracker = 0;
                animRebound = true;
            }
        }
        else
        {
            animTracker += 1;
            obj.localPosition += obj.up * animMovement / 2 * Time.deltaTime;
            if (animTracker == animHolder)
            {
                obj.localPosition = new Vector2(0, 0);
                animRebound = false;
                animPlaying = false;
            }
        }
        return;
    }




    


}
