using UnityEngine;
using Mirror;
using System.Collections.Generic;

// TODO: Fix scriptable object reference in commands

public class BuildingHandler : NetworkBehaviour
{
    // Grid variable
    [HideInInspector] public Grid tileGrid;

    // Building variables
    public static BuildingHandler active;

    public void Start()
    {
        // Get reference to active instance
        active = this;

        // Sets static variables on start
        tileGrid = new Grid();
        tileGrid.cells = new Dictionary<Vector2Int, Grid.Cell>();
    }

    // Creates a building
    public void CreateBuilding(Building tile, Vector2 position, Quaternion rotation, int option)
    {
        // Untiy is so fucky it is now in a new dimension of bullshit
        if (tile == null) return;

        // Check to make sure the tiles are not being used
        if (!CheckTiles(tile, position)) return;

        // Instantiate the object like usual
        RpcInstantiateBuilding(tile, position, rotation, option);
    }

    [ClientRpc]
    private void RpcInstantiateBuilding(Building tile, Vector2 position, Quaternion rotation, int option)
    {
        // Get game objected from scriptable manager
        GameObject obj = ScriptableManager.RequestBuildingByName(tile.name);
        if (obj == null) return;

        // Create the tile
        BaseTile lastBuilding = Instantiate(obj, position, rotation).GetComponent<BaseTile>();
        lastBuilding.transform.position = new Vector3(position.x, position.y, -1);
        lastBuilding.name = tile.name;

        // Set the tiles on the grid class
        if (tile.cells.Length > 0)
        {
            foreach (Building.Cell cell in tile.cells)
                tileGrid.SetCell(Vector2Int.RoundToInt(new Vector2(lastBuilding.transform.position.x + cell.x, lastBuilding.transform.position.y + cell.y)), true, tile, lastBuilding);
        }
        else tileGrid.SetCell(Vector2Int.RoundToInt(lastBuilding.transform.position), true, tile, lastBuilding);
    }

    // Destroys a building
    [ClientRpc]
    public void RpcDestroyBuilding(Vector3 position)
    {
        tileGrid.DestroyCell(Vector2Int.RoundToInt(position));
    }

    // Checks to make sure tile(s) isn't occupied
    [Server]
    public bool CheckTiles(Building tile, Vector3 position)
    {
        if (tile.cells.Length > 0)
        {
            foreach (Building.Cell cell in tile.cells)
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

    // Attempts to return a building
    public Building TryGetBuilding(Vector2 position)
    {
        Grid.Cell cell = tileGrid.RetrieveCell(Vector2Int.RoundToInt(position));
        if (cell != null)
        {
            Building building = cell.obj.GetComponent<Building>();
            return building;
        }
        return null;
    }
}
