using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BuildingHandler : MonoBehaviour
{
    public static Technology tech;
    public static bool isMenu = false;
    public static List<Transform> activeBuildings = new List<Transform>();
    public static List<Transform> damagedBuildings = new List<Transform>();
    public static List<StorageAI> storages = new List<StorageAI>();
    public static Dictionary<string, int> buildingAmount = new Dictionary<string, int>();

    public void Start()
    {
        if (SceneManager.GetActiveScene().name == "Menu") isMenu = true;
        else
        {
            tech = GameObject.Find("Survival").GetComponent<Technology>();
            isMenu = false;
        }

        // Reset all lists (this doesn't happe on scene change)
        activeBuildings = new List<Transform>();
        damagedBuildings = new List<Transform>();
        storages = new List<StorageAI>();
        buildingAmount = new Dictionary<string, int>();
    }

    public static void addBuilding(Transform building)
    {
        activeBuildings.Add(building);
        if (isMenu) return;

        if (buildingAmount.ContainsKey(building.name))
            buildingAmount[building.name] += 1;
        else buildingAmount.Add(building.name, 1);

        tech.UpdateUnlock("Place");
    }


    public static void removeBuilding(Transform building)
    {
        try
        {
            activeBuildings.Remove(building);
            if (isMenu) return;

            if (buildingAmount.ContainsKey(building.name))
                buildingAmount[building.name] -= 1;
            else buildingAmount.Add(building.name, 0);
        }
        catch { Debug.LogError("Issue while removing building " + building.name);}
    }

    // 1 = gold, 2 = essence, 3 = iridium
    public static void removeStorageResources(int amount, int type)
    {
        if (isMenu) return;

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
