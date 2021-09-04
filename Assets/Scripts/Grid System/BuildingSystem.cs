using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class BuildingSystem : MonoBehaviour
{
    // Building class
    public class BuildingQueue
    {
        public BuildingQueue(GameObject obj, Vector2 pos, Quaternion rotation)
        {
            this.obj = obj;
            this.pos = pos;
            this.rotation = rotation;
        }

        public GameObject obj;
        public Vector2 pos;
        public Quaternion rotation;
    }

    // Grid variable
    public GridSystem tileGrid;

    // Building variables
    public static BuildingSystem active;
    public Building building;
    public Vector2 position;
    private Vector2 offset;
    private GameObject lastObj;

    // Sprite values
    private SpriteRenderer spriteRenderer;
    private float alphaAdjust = 0.005f;
    private float alphaHolder;

    private List<BuildingQueue> buildingQueue;
    private int cooldown;

    // Start method grabs tilemap
    public void Awake()
    {
        // Grabs active component if it exists
        if (this != null) active = this;
        else active = null;

        // Sets static variables on start
        tileGrid = new GridSystem();
        tileGrid.cells = new Dictionary<Vector2Int, GridSystem.Cell>();
        building = null;
        position = new Vector2(0, 0);
        offset = new Vector2(0, 0);
        lastObj = null;
        buildingQueue = new List<BuildingQueue>();

        // Sets static anim variables
        spriteRenderer = GetComponent<SpriteRenderer>();
        alphaHolder = alphaAdjust;

        cooldown = 3;
    }

    // Update is called once per frame
    public void Update()
    {
        // Check if active is null
        if (active == null) return;

        // Round to grid
        OffsetBuilding();
        AdjustTransparency();

        if (buildingQueue.Count > 0 && cooldown == 0)
        {
            RpcInstantiateObject(buildingQueue[0]);
            buildingQueue.RemoveAt(0);
            cooldown = 3;
        }
        else cooldown -= 1;
    }

    private void OffsetBuilding()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        active.transform.position = new Vector2(5 * Mathf.Round(mousePos.x / 5) + offset.x, 5 * Mathf.Round(mousePos.y / 5) + offset.y);
        position = active.transform.position;
    }

    // Adjusts the alpha transparency of the SR component 
    private void AdjustTransparency()
    {
        // Switches
        if (spriteRenderer.color.a >= 1f)
            alphaHolder = -alphaAdjust;
        else if (spriteRenderer.color.a <= 0f)
            alphaHolder = alphaAdjust;

        // Set alpha
        spriteRenderer.color = new Color(1f, 1f, 1f, spriteRenderer.color.a + alphaHolder);
    }

    // Sets the selected building
    public void SetBuilding(Building building)
    {
        spriteRenderer.sprite = building.icon;
        this.building = building;
        if (building != null) offset = building.offset;
    }

    // Creates a building
    public void CmdCreateBuilding()
    {
        // Check if active is null
        if (building == null || building.obj == null) return;

        // Check to make sure the tiles are not being used
        if (!CheckTiles()) return;

        // Instantiate the object like usual
        buildingQueue.Add(new BuildingQueue(building.obj, position, Quaternion.identity));
    }

    // Creates a building at specified coords
    public void CmdCreateBuilding(Vector2 coords)
    {
        // Check if active is null
        if (building == null || building.obj == null) return;

        // Instantiate the object like usual
        buildingQueue.Add(new BuildingQueue(building.obj, coords, Quaternion.identity));
    }

    private void RpcInstantiateObject(BuildingQueue building)
    {
        // Create the tile
        lastObj = Instantiate(building.obj, building.pos, building.rotation);
        lastObj.name = building.obj.name;

        SetCells();
    }

    // Checks to make sure tile(s) isn't occupied
    public bool CheckTiles()
    {
        if (building.cells.Length > 0)
        {
            foreach (Tile.Cell cell in building.cells)
                if (tileGrid.RetrieveCell(Vector2Int.RoundToInt(new Vector2(position.x + cell.x, position.y + cell.y))) != null)
                    return false;
        }
        else return tileGrid.RetrieveCell(Vector2Int.RoundToInt(position)) == null;
        return true;
    }

    public void SetCells()
    {
        // Set the tiles on the grid class
        if (building.cells.Length > 0)
        {
            foreach (Tile.Cell cell in building.cells)
                tileGrid.SetCell(Vector2Int.RoundToInt(new Vector2(lastObj.transform.position.x + cell.x, lastObj.transform.position.y + cell.y)), true, building, lastObj);
        }
        else tileGrid.SetCell(Vector2Int.RoundToInt(lastObj.transform.position), true, building, lastObj);
    }
}
