using System.Collections.Generic;
using UnityEngine;

public class BuildingHandler : MonoBehaviour
{
    public static List<Transform> buildings = new List<Transform>();
    public static List<Transform> damagedBuildings = new List<Transform>();

    public Transform getClosestDamagedBuilding(Vector2 pos)
    {
        Transform found = null;
        float closest = float.PositiveInfinity;

        foreach (Transform building in damagedBuildings)
        {
            if (building == null)
            {
                damagedBuildings.Remove(building);
                continue;
            }
            else
            {
                float distance = Vector2.Distance(building.position, pos);
                if (distance < closest)
                {
                    found = building;
                    closest = distance;
                }
            }
        }

        return found;
    }
}
