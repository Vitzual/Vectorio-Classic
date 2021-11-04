using UnityEngine;
//using Mirror;
using System.Collections.Generic;

// This script is ported from Automa.
// https://github.com/Vitzual/Automa

public class InstantiationHandler : MonoBehaviour
{
    // Grid variable
    [HideInInspector] public Grid tileGrid;

    // Building variables
    public static InstantiationHandler active;
    public LayerMask enemyLayer;

    public void Awake()
    {
        // Get reference to active instance
        active = this;

        // Sets static variables on start
        tileGrid = new Grid();
        tileGrid.cells = new Dictionary<Vector2Int, Cell>();
    }

    // Creates an entity
    public void CreateEnemy(Entity entity, Vector2 position, Quaternion rotation)
    {
        // Check if entity is null
        if (entity == null) return;

        // Check if the entity is an enemy
        RpcInstantiateEnemy(entity, position, rotation);
    }

    // Creates a building
    public void CreateBuilding(Building building, Vector2 position, Quaternion rotation)
    {
        // Check if entity is null
        if (building == null) return;

        // Check if resource should be used
        if (Gamemode.active.useResources) 
        {
            foreach (Building.Resources resource in building.resources) 
            {
                if (!resource.storage)
                {
                    int amount = Resource.active.GetAmount(resource.resource);
                    if (resource.add && amount + resource.amount > Resource.active.GetStorage(resource.resource)) return;
                    else if (!resource.add && amount < resource.amount) return;
                }
            }
        }

        // Check to make sure the tiles are not being used
        if (!CheckTiles(building, position)) return;

        // Instantiate the object like usual
        RpcInstantiateBuilding(building, position, rotation);
    }

    //[ClientRpc]
    private void RpcInstantiateEnemy(Entity entity, Vector2 position, Quaternion rotation)
    {
        // Use enemy handler thing
        RaycastHit2D[] hits = Physics2D.RaycastAll(position, Vector2.zero, Mathf.Infinity, enemyLayer);
        foreach (RaycastHit2D hit in hits)
            if (hit.collider != null) return;
        EnemyHandler.active.CreateEntity(entity, position, rotation);
    }

    //[ClientRpc]
    private void RpcInstantiateBuilding(Building building, Vector2 position, Quaternion rotation)
    {
        // Get game objected from scriptable manager
        GameObject obj = ScriptableManager.RequestBuildingByName(building.name);
        if (obj == null) return;

        // Create the tile
        BaseTile lastBuilding = Instantiate(obj, position, rotation).GetComponent<BaseTile>();
        lastBuilding.name = building.name;

        // Set the tiles on the grid class
        SetCells(building, position, lastBuilding); 

        // Update resource values promptly
        if (Gamemode.active.useResources)
        {
            foreach (Building.Resources resource in building.resources)
            {
                if (resource.storage)
                {
                    if (resource.add) Resource.active.AddStorage(resource.resource, resource.amount);
                    else Resource.active.RemoveStorage(resource.resource, resource.amount);
                }
                else
                {
                    if (resource.add) Resource.active.Add(resource.resource, resource.amount);
                    else Resource.active.Remove(resource.resource, resource.amount);
                }
            }
        }

        // Call buildings setup method
        lastBuilding.Setup();
    }

    public void SetCells(Building building, Vector2 position, BaseTile obj)
    {
        // Set the tiles on the grid class
        if (building.cells.Length > 0)
        {
            foreach (Building.Cell cell in building.cells)
                tileGrid.SetCell(Vector2Int.RoundToInt(new Vector2(position.x + cell.x, position.y + cell.y)), true, building, obj);
        }
    }

    // Destroys a buildingg
    //[ClientRpc]
    public void RpcDestroyBuilding(Vector3 position)
    {
        tileGrid.DestroyCell(Vector2Int.RoundToInt(position));
    }

    // Checks to make sure tile(s) isn't occupied
    //[Server]
    public bool CheckTiles(Building building, Vector3 position)
    {
        if (building.cells.Length > 0)
        {
            foreach (Building.Cell cell in building.cells)
            {
                // Check to make sure nothing occupying tile
                Vector2Int coords = Vector2Int.RoundToInt(new Vector2(position.x + cell.x, position.y + cell.y));
                if (tileGrid.RetrieveCell(coords) != null)
                    return false;
            }
        }
        else
        {
            // Check to make sure nothing occupying tile
            Vector2Int coords = Vector2Int.RoundToInt(new Vector2(position.x, position.y));
            if (tileGrid.RetrieveCell(coords) != null)
                return false;
        }
        return true;
    }

    // Returns closest building to position given
    public BaseTile GetClosestBuilding(Vector2Int position)
    {
        BaseTile nearest = null;
        float distance = float.PositiveInfinity;

        foreach (KeyValuePair<Vector2Int, Cell> cell in tileGrid.cells)
        {
            float holder = Vector2Int.Distance(position, cell.Key);
            if (holder < distance)
            {
                distance = holder;
                nearest = cell.Value.obj;
            }
        }

        return nearest;
    }

    // Attempts to return a building
    public BaseTile TryGetBuilding(Vector2 position)
    {
        Cell cell = tileGrid.RetrieveCell(Vector2Int.RoundToInt(position));
        if (cell != null)
        {
            BaseTile building = cell.obj.GetComponent<BaseTile>();
            return building;
        }
        return null;
    }
}
