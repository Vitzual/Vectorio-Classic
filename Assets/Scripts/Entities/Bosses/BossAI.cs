using UnityEngine;
using System.Collections;
using Michsky.UI.ModernUIPack;

public class RevenantAI : BossClass
{
    public float minDist;

    // Gets called when entering another defenses range or hitting the defense all together
    public new void OnTriggerEnter2D(Collider2D collider)
    {
        if (Vector3.Distance(collider.transform.position, transform.position) >= minDist && collider.tag == "Defense")
        {
            Debug.Log(collider.name + " " + Vector3.Distance(collider.transform.position, transform.position));
            if (collider.name != "Wall" && collider.name != "Drone Port")
            {
                DefaultTurret holder = collider.GetComponent<DefaultTurret>();
                if (!holder.enabled) holder.enabled = true;
                holder.forceTarget(transform);
            }
        }
        else 
        {
            if (!collider.name.ToLower().Contains("bullet"))
            {
                try
                {
                    Debug.Log(collider.name + " X");
                    collider.GetComponent<TileClass>().DestroyTile(true);
                }
                catch{ return; }
            }
        } 
    }
}
