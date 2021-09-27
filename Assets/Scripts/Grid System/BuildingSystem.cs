using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class BuildingSystem : MonoBehaviour
{
    // Building class
    public class EntityQueue
    {
        public EntityQueue(Entity scriptable, Vector2 pos, Quaternion rotation)
        {
            this.scriptable = scriptable;
            this.pos = pos;
            this.rotation = rotation;
        }

        public Entity scriptable;
        public Vector2 pos;
        public Quaternion rotation;
    }

    // Grid variable
    public GridSystem tileGrid;

    // Building variables
    public static BuildingSystem active;
    [HideInInspector] public Entity selected;
    private Vector2 offset;
    private GameObject lastObj;
    public Variant variant;
    public LayerMask enemyLayer;
    [HideInInspector] public bool canDelete = true;
    public float border = 742.5f;

    // Sprite values
    private SpriteRenderer spriteRenderer;
    private float alphaAdjust = 0.005f;
    private float alphaHolder;

    // Start method grabs tilemap
    public void Start()
    {
        // Grabs active component if it exists
        if (this != null) active = this;
        else active = null;

        // Sets static variables on start
        tileGrid = new GridSystem();
        tileGrid.cells = new Dictionary<Vector2Int, Cell>();
        selected = null;
        offset = new Vector2(0, 0);
        lastObj = null;

        // Sets static anim variables
        spriteRenderer = GetComponent<SpriteRenderer>();
        alphaHolder = alphaAdjust;

        // Setup events
        Events.active.onRightMousePressed += DeleteTile;
        Events.active.onRightMouseReleased += Deselect;
    }

    // Update is called once per frame
    public void Update()
    {
        // Check if active is null
        if (active == null) return;

        // Round to grid
        OffsetBuilding();
        AdjustTransparency();
    }
    
    private void Deselect()
    {
        if (selected != null)
            SetBuilding(null);
    }

    public void DeleteTile()
    {
        if (canDelete)
        {
            tileGrid.DestroyCell(Vector2Int.RoundToInt(transform.position));
        }
    }

    private void OffsetBuilding()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (selected != null)
        {
            if (selected.snap) transform.position = new Vector2(5 * Mathf.Round(mousePos.x / 5) + offset.x, 5 * Mathf.Round(mousePos.y / 5) + offset.y);
            else transform.position = new Vector2(mousePos.x + offset.x, mousePos.y + offset.y);
        }
        else
        {
            transform.position = new Vector2(5 * Mathf.Round(mousePos.x / 5), 5 * Mathf.Round(mousePos.y / 5));
        }
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
    public void SetBuilding(Entity entity)
    {
        selected = entity;
        if (entity != null)
        {
            canDelete = false;

            spriteRenderer.sprite = Sprites.GetSprite(entity.name);
            transform.localScale = new Vector2(entity.size, entity.size);
            offset = entity.tile.offset;
        }
        else
        {
            canDelete = true;

            spriteRenderer.sprite = Sprites.GetSprite("Transparent");
        }
    }

    // Creates a building
    public void CmdCreateBuilding()
    {
        // Check if active is null
        if (selected == null || selected.obj == null) return;

        // Check if snap is enabled
        if (!selected.snap)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.zero, enemyLayer);
            if (hit.collider != null) return;
        }

        // Check to make sure the tiles are not being used
        if (!CheckTiles()) return;

        // Instantiate the object like usual
        RpcInstantiateObject(new EntityQueue(selected, transform.position, Quaternion.identity));
    }

    // Creates a building at specified coords
    public void CmdCreateBuilding(Vector2 coords)
    {
        // Check if active is null
        if (selected == null || selected.obj == null) return;

        // Instantiate the object like usual
        RpcInstantiateObject(new EntityQueue(selected, coords, Quaternion.identity));
    }

    private void RpcInstantiateObject(EntityQueue entity)
    {
        // Create the tile
        lastObj = Instantiate(entity.scriptable.obj, entity.pos, entity.rotation);
        lastObj.name = entity.scriptable.name;
        lastObj.GetComponent<DefaultEntity>().Setup();

        if (selected.tile.cells.Length > 0)
            SetCells(entity.scriptable, lastObj);
    }

    // Checks to make sure tile(s) isn't occupied
    public bool CheckTiles()
    {
        float xCoord, yCoord;

        if (selected.tile.cells.Length > 0)
        {
            foreach (Tile.Cell cell in selected.tile.cells)
            {
                xCoord = transform.position.x + cell.x;
                yCoord = transform.position.y + cell.y;

                if (tileGrid.RetrieveCell(Vector2Int.RoundToInt(new Vector2(xCoord, yCoord))) != null) return false;
                else if (xCoord < -border || xCoord > border || yCoord < -border || yCoord > border) return false;
            }
        }
        return true;
    }

    public void SetCells(Entity entity, GameObject obj)
    {
        // Attempt to get the default building script
        DefaultBuilding building = obj.GetComponent<DefaultBuilding>();

        // Check to see if the building contains a DB script
        if (building == null)
        {
            Debug.LogError("Entity has cells but does not contain a DefaultBuilding script!\nRemoving from scene.");
            Destroy(obj);
            return;
        }

        // Set the tiles on the grid class
        Vector2Int coords;
        if (entity.tile.cells.Length > 0)
        {
            foreach (Tile.Cell cell in entity.tile.cells)
            {
                coords = Vector2Int.RoundToInt(new Vector2(obj.transform.position.x + cell.x, obj.transform.position.y + cell.y));
                tileGrid.SetCell(coords, true, entity.tile, building);
                building.cells.Add(coords);
            }
        }
    }

    public DefaultBuilding GetClosestBuilding(Vector2Int position)
    {
        DefaultBuilding nearest = null;
        float distance = float.PositiveInfinity;

        foreach (KeyValuePair<Vector2Int, Cell> cell in tileGrid.cells)
        {
            float holder = Vector2Int.Distance(position, cell.Key);
            if (holder < distance)
            {
                distance = holder;
                nearest = cell.Value.building;
            }
        }

        return nearest;
    }

    public void ClearBuildings()
    {
        tileGrid.DestroyAllCells();
    }
}
