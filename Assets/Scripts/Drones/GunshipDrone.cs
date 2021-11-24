using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunshipDrone : Drone
{
    // Home reference
    public GunshipPort homePort;

    // Start is called before the first frame update
    public List<GunshipTurret> turrets = new List<GunshipTurret>();
    public List<DefaultEnemy> targets = new List<DefaultEnemy>();

    // Doors closed
    private bool doorsClosed = false;

    // Setup the drone
    public override void ExitPort()
    {
        stage = Stage.ExitingPort;
    }

    // Exit port
    public override void ExitingPort()
    {
        if (transform.localScale.x <= 1.2f)
            transform.localScale += new Vector3(0.001f, 0.001f, 0f);
        if (homePort.OpenDoors()) StartRoute();
    }

    // Move around map freely
    public override void StartRoute()
    {
        droneSpeed = 10f;
        homePort.ChangePanelLayers();
        stage = Stage.MovingToTarget;
    }

    // Move towards target
    public override void Move()
    {
        // Move around
        if (target != null)
        {
            transform.position = Vector2.MoveTowards(transform.position, target.transform.position, droneSpeed * Time.deltaTime);
            float dot = Vector3.Dot(transform.forward, (target.transform.position - transform.position).normalized);
            if (dot < 0.99f) transform.Rotate(Vector3.up * 2f * Time.deltaTime);
        }

        // Close port layers
        if (!doorsClosed) doorsClosed = homePort.CloseDoors();
    }

    // Get new target
    public void FindNewTarget()
    {
        target = EnemyHandler.active.enemies[Random.Range(0, EnemyHandler.active.enemies.Count)];
    }

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
