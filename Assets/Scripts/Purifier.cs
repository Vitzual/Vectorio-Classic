using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Purifier : BaseTile
{
    public CircleCollider2D circle;
    public float attractionRadius;

    public override void Setup()
    {
        circle = GetComponent<CircleCollider2D>();
        circle.radius = attractionRadius;
        base.Setup();
    }

    public override void OnCircleCollision(DefaultEnemy enemy)
    {
        enemy.purified = true;
    }

    public override void OnCircleLeave(DefaultEnemy enemy)
    {
        enemy.purified = false;
    }
}
