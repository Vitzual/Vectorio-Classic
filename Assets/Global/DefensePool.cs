// Add this script to a defense to add it to a enemies
// targetting AI. If this script is not added to a
// defense, then it will not be instantiated in memory
// as a defense object and will not be targetted by AI.

using System.Collections.Generic;
using UnityEngine;

public class DefensePool : MonoBehaviour
{
    // Get the global HashSet of every enemy using this script
    public readonly static HashSet<DefensePool> Pool = new HashSet<DefensePool>();

    // On enable, add enemy to the HashSet
    private void OnEnable()
    {
        DefensePool.Pool.Add(this);
    }

    // On disable, remove enemy from the HashSet
    private void OnDisable()
    {
        DefensePool.Pool.Remove(this);
    }

    // Find the nearest enemy, and return the object
    // Updates ever 0.5s and caches (very cpu efficient)
    public static DefensePool FindClosestDefense(Vector3 pos)
    {
        DefensePool result = null;
        float dist = float.PositiveInfinity;
        var e = DefensePool.Pool.GetEnumerator();
        while (e.MoveNext())
        {
            float d = (e.Current.transform.position - pos).sqrMagnitude;
            if (d < dist)
            {
                result = e.Current;
                dist = d;
            }
        }
        return result;
    }

    // Find the nearest enemy, and return the distance
    public static float FindClosestPosition(Vector3 pos)
    {
        DefensePool result = null;
        float dist = float.PositiveInfinity;
        var e = DefensePool.Pool.GetEnumerator();
        while (e.MoveNext())
        {
            float d = (e.Current.transform.position - pos).sqrMagnitude;
            if (d < dist)
            {
                result = e.Current;
                dist = d;
            }
        }
        return dist;
    }

}
