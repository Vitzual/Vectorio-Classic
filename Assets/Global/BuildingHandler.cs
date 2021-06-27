using System.Collections.Generic;
using UnityEngine;

public class BuildingHandler : MonoBehaviour
{
    public static List<Transform> buildings = new List<Transform>();
    public static List<Transform> damagedBuildings = new List<Transform>();
    public static List<StorageAI> storages = new List<StorageAI>();

    // 1 = gold, 2 = essence, 3 = iridium
    public static void removeStorageResources(int amount, int type)
    {
        for (int i = 0; i < storages.Count; i++)
        {
            if (storages[i] == null)
            {
                storages.RemoveAt(i);
                i--;
            }
            else
            {
                StorageAI storageScript = storages[i];
                if (storageScript.type == type)
                {
                    amount = storageScript.takeResources(amount);
                    if (amount <= 0) return;
                }
            }
        }
    }

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
