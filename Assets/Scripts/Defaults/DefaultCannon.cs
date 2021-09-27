using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultCannon : MonoBehaviour
{
    private DefaultTurret turret;

    // Start is called before the first frame update
    public void Setup(DefaultTurret script)
    {
        turret = script;

        CircleCollider2D collider = GetComponent<CircleCollider2D>();

        if (collider != null)
            collider.radius = turret.turret.range;
        else Debug.LogError("Cannon does not have a circle collider!");
    }
}
