using UnityEngine;
//using Mirror;
using System.Collections.Generic;
using UnityEngine.Tilemaps;

// This script is ported from Automa.
// https://github.com/Vitzual/Automa

public class InstantiationHandler : MonoBehaviour
{
    // Amount placed
    public static int amountPlaced = 0;

    // Grid variable
    [HideInInspector] public Grid tileGrid;

    // Building variables
    public static InstantiationHandler active;
    public GhostTile ghostTile;
    public AudioClip placementSound;
    public LayerMask enemyLayer;
    public LayerMask aocbLayer;

    // Debug variables
    public bool debug = false;
    public GameObject debugCircle;
    public List<GameObject> activeCircles;

    public void Awake()
    {
        // Get reference to active instance
        active = this;
        amountPlaced = 0;

        // Sets static variables on start
        tileGrid = new Grid();
        tileGrid.cells = new Dictionary<Vector2Int, Cell>();
    }

    // Creates an entity
    public void CreateEnemy(Entity entity, Variant variant, Vector2 position, Quaternion rotation)
    {
        // Check if entity is null
        if (entity == null) return;

        // Check if the entity is an enemy
        RpcInstantiateEnemy(entity, variant, position, rotation);
    }

    // Creates a building
    public void CreateBuilding(Buildable buildable, Vector2 position, Quaternion rotation, int metadata = -1)
    {
        // Determine if the building is free
        bool isFree = Resource.active.CheckFreebie(buildable);

        // Get buildable
        if (buildable == null)
        {
            Debug.Log("Could not retrieve buildable " + buildable.building.name);
            return;
        }

        // Check resources if applicable
        if (!isFree && !Gamemode.active.useDroneConstruction && !Resource.active.CheckResources(buildable.building.resources)) return;
        else if (!Gamemode.active.useResources && !Gamemode.active.useDroneConstruction && !Resource.active.CheckOutputsOnly(buildable.building.resources)) return;

        // Check to make sure the tiles are not being used
        if (!CheckTiles(buildable.building, position)) return;

        // Instantiate the object like usual
        if (Gamemode.active.useDroneConstruction) RpcInstatiateGhost(buildable, position, rotation, metadata);
        else RpcInstantiateBuilding(buildable, position, rotation, isFree, metadata);
    }

    //[ClientRpc]
    private void RpcInstantiateEnemy(Entity entity, Variant variant, Vector2 position, Quaternion rotation)
    {
        // Use enemy handler thing
        RaycastHit2D[] hits = Physics2D.RaycastAll(position, Vector2.zero, Mathf.Infinity, enemyLayer);
        foreach (RaycastHit2D hit in hits)
            if (hit.collider != null) return;
        EnemyHandler.active.CreateEntity(entity, variant, position, rotation);
    }

    //[ClientRpc]
    public void RpcInstantiateBuilding(Buildable buildable, Vector2 position, Quaternion rotation, bool free, int metadata = -1, float health = -1)
    {
        // Create the tile
        BaseTile lastBuilding = Instantiate(buildable.obj, position, rotation).GetComponent<BaseTile>();
        lastBuilding.name = buildable.building.name;
        lastBuilding.buildable = buildable;
        amountPlaced += 1;

        // Set the tiles on the grid class
        SetCells(buildable.building, position, lastBuilding);

        // Update resource values promptly
        if (!free) Resource.active.ApplyResources(buildable.building.resources);
        else Resource.active.ApplyOutputsOnly(buildable.building.resources);

        // Call buildings setup method and metadata method if metadata is applied
        if (metadata != -1) lastBuilding.ApplyMetadata(metadata); 
        lastBuilding.Setup();

        // Check health
        if (health != -1) lastBuilding.health = health;
        
        // Set sellable values
        lastBuilding.saveBuilding = buildable.building.isSaveable;
        lastBuilding.isSellable = buildable.building.isSellable;

        // Create sound effect
        if (!NewSaveSystem.loadGame && placementSound != null)
            AudioSource.PlayClipAtPoint(placementSound, position, Settings.sound);
    }

    // temp cause im tired af
    public void RpcInstantiateBuilding(Buildable buildable, Vector2 position, Quaternion rotation, int metadata = -1)
    {
        // Create the tile
        BaseTile lastBuilding = Instantiate(buildable.obj, position, rotation).GetComponent<BaseTile>();
        lastBuilding.name = buildable.building.name;
        lastBuilding.buildable = buildable;
        amountPlaced += 1;

        // Set the tiles on the grid class
        SetCells(buildable.building, position, lastBuilding);

        // Call buildings setup method and metadata method if metadata is applied
        if (metadata != -1) lastBuilding.ApplyMetadata(metadata);
        lastBuilding.Setup();

        // Set sellable values
        lastBuilding.saveBuilding = buildable.building.isSaveable;
        lastBuilding.isSellable = buildable.building.isSellable;

        // Create sound effect
        if (!NewSaveSystem.loadGame && placementSound != null)
            AudioSource.PlayClipAtPoint(placementSound, position, Settings.sound);
    }

    //[ClientRpc]
    public void RpcInstatiateGhost(Buildable buildable, Vector2 position, Quaternion rotation, int metadata = -1)
    {
        // Create the tile
        GhostTile holder = Instantiate(ghostTile, position, rotation).GetComponent<GhostTile>();
        holder.name = buildable.building.name;

        // Setup the ghost tile
        holder.SetBuilding(buildable, metadata);
        DroneManager.active.AddGhost(holder);

        // Set the tiles on the grid class
        SetCells(buildable.building, position, holder, true);
    }

    public void SetCells(Building building, Vector2 position, BaseTile obj, bool isGhost = false)
    {
        // Set the tiles on the grid class
        if (building.cells.Length > 0)
        {
            foreach (Building.Cell cell in building.cells)
            {
                if (debug) SpawnDebugCircle(new Vector2(position.x + cell.x, position.y + cell.y));
                tileGrid.SetCell(Vector2Int.RoundToInt(new Vector2(position.x + cell.x, position.y + cell.y)), true, building, obj, isGhost);
            }
        }
    }

    // Destroys a buildingg
    //[ClientRpc]
    public void RpcDestroyBuilding(Vector3 position)
    {
        Vector2Int coords = GetCellCoords(position);
        BaseTile tile = tileGrid.RetrieveTile(coords);
        if (tile != null && tile.isSellable) tileGrid.DestroyCell(coords);
    }

    // Checks to make sure tile(s) isn't occupied
    //[Server]
    public bool CheckTiles(Building building, Vector3 position)
    {
        bool aocbCheck = false;

        if (building.radialCheck > 0)
        {
            Collider2D[] aocbColliders = Physics2D.OverlapBoxAll(position, new Vector2(building.radialCheck, building.radialCheck), 0f, aocbLayer);
            if (aocbColliders.Length > 0) aocbCheck = true;
            else return false;
        }
        if (building.cells.Length > 0)
        {
            foreach (Building.Cell cell in building.cells)
            {
                Vector2Int checkTile = Vector2Int.RoundToInt(new Vector2(position.x + cell.x, position.y + cell.y));
                if (!CheckTile(checkTile, building)) return false;
                else if (Gamemode.active.useEnergizers && !aocbCheck)
                {
                    RaycastHit2D hit = Physics2D.Raycast(checkTile, Vector2.zero, Mathf.Infinity, aocbLayer);
                    if (hit.collider == null) return false;
                }
            }
        }
        else
        {
            Vector2Int checkTile = Vector2Int.RoundToInt(new Vector2(position.x, position.y));
            if (!CheckTile(checkTile, building)) return false;
            else if (Gamemode.active.useEnergizers && !aocbCheck)
            {
                RaycastHit2D hit = Physics2D.Raycast(checkTile, Vector2.zero, Mathf.Infinity, aocbLayer);
                if (hit.collider == null) return false;
            }
        }
        return true;
    }

    // Check a specific tile
    public bool CheckTile(Vector2Int coords, Building building)
    {
        // Check if tile is in bounds
        if (coords.y > Border.north || coords.y < Border.south ||
            coords.x > Border.east || coords.x < Border.west)
            return false;

        // Check if tile already occupied
        if (tileGrid.RetrieveCell(coords) != null)
            return false;

        // Check if tile is restricted to a specific node
        if (building.restrictPlacement && !WorldGenerator.active.CheckNode(coords, building.placedOn))
            return false;

        // If all checks passed, return true
        return true;
    }

    // Returns closest building to position given
    public BaseTile GetClosestBuilding(Vector2Int position)
    {
        BaseTile nearest = null;
        float distance = float.PositiveInfinity;

        foreach (KeyValuePair<Vector2Int, Cell> cell in tileGrid.cells)
        {
            if (cell.Value.ghostTile) continue;

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
        Cell cell = tileGrid.RetrieveCell(GetCellCoords(position));
        if (cell != null)
        {
            BaseTile building = cell.obj.GetComponent<BaseTile>();
            return building;
        }
        return null;
    }

    // Get cell coords function
    public Vector2Int GetCellCoords(Vector2 position)
    {
        // Create adjustment variables
        float xAdjustment = 2.5f;
        float yAdjustment = 2.5f;

        // Calculate adjustment amount
        if (position.x >= 0) xAdjustment = -xAdjustment;
        if (position.y >= 0) yAdjustment = -yAdjustment;

        // Get cell coordinate
        position = new Vector2(position.x - xAdjustment, position.y - yAdjustment);
        Vector2Int cellCoords = new Vector2Int((int)position.x / 5 * 5, (int)position.y / 5 *5);
        return cellCoords;
    }

    // Spawn a debug circle
    public void SpawnDebugCircle(Vector2 position)
    {
        activeCircles.Add(Instantiate(debugCircle, position, Quaternion.identity));
    }

    // Remove all debug circles
    public void RemoveDebugCircles()
    {
        foreach (GameObject circle in activeCircles)
            Recycler.AddRecyclable(circle.transform);
        activeCircles = new List<GameObject>();
    }
}
