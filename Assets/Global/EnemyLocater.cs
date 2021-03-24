using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLocater : MonoBehaviour
{
    private int checkTime = 100;
    private int checkTracker = 0;
    private Collider2D[] colliders;
    public LayerMask BuildingLayer;

    // Update is called once per frame
    void Update()
    {
        if (checkTime == checkTracker)
            ScanForBuilings();
        else checkTracker++;
    }

    public void ScanForBuilings()
    {
        colliders = Physics2D.OverlapCircleAll(transform.position, 2000, BuildingLayer);
        checkTracker = 0;
    }

    public Transform requestTarget(Vector3 position, Transform PreferredTarget)
    {
        Transform result = null;
        float closest = float.PositiveInfinity;
        bool isTarget = false;

        if (colliders == null)
        {
            ScanForBuilings();
        }

        foreach (Collider2D collider in colliders)
        {
            if (collider == null) continue;

            if (PreferredTarget != null && isTarget && collider.name != PreferredTarget.name)
                continue;

            float distance = (collider.transform.position - position).sqrMagnitude;

            if (PreferredTarget != null && !isTarget && collider.name == PreferredTarget.name)
            {
                result = collider.transform;
                closest = distance;
                isTarget = true;
                continue;
            }

            if (distance < closest) {
                result = collider.transform;
                closest = distance;
            }
        }
        return result;
    }
}
