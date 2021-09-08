using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretHandler : MonoBehaviour
{
    // Active class
    public static TurretHandler active;

    // Barrel class
    public List<TurretEntity> turretEntities;

    public void Start()
    {
        if (this != null)
            active = this;
    }

    public void Update()
    {
        for (int i=0; i<turretEntities.Count; i++)
        {
            if (turretEntities[i].obj == null)
            {
                RemoveTurretEntity(turretEntities[i]);
                i--;
            }
            else if (turretEntities[i].hasTarget)
            {

            }
        }
    }

    public void AddTurretEntity(Turret turret, Transform[] firePoints, Transform obj, GameObject bullet = null)
    {
        turretEntities.Add(new TurretEntity(turret, firePoints, obj, bullet));
    }

    public void RemoveTurretEntity(TurretEntity turretEntity)
    {

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
