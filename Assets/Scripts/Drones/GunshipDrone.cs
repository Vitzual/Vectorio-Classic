using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunshipDrone : Drone
{
    // Start is called before the first frame update
    public List<GunshipTurret> turrets = new List<GunshipTurret>();
    public List<DefaultEnemy> targets = new List<DefaultEnemy>();

    // If a collision is detected, add taget
    public virtual void OnTriggerEnter2D(Collider2D other)
    {
        DefaultEnemy enemy = other.GetComponent<DefaultEnemy>();
        if (enemy != null && !targets.Contains(enemy))
        {
            targets.Add(enemy);
            foreach (GunshipTurret turret in turrets)
                turret.AddTarget(enemy);
        }
    }

    // If entity leaves defense range, remove self from target list
    public virtual void OnTriggerExit2D(Collider2D other)
    {
        DefaultEnemy enemy = other.GetComponent<DefaultEnemy>();
        if (enemy != null && targets.Contains(enemy))
        {
            targets.Remove(enemy);
            foreach (GunshipTurret turret in turrets)
                turret.RemoveTarget(enemy);
        }
    }
}
