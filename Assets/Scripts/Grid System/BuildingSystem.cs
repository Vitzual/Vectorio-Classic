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
    public Entity selected;
    public Vector2 position;
    private Vector2 offset;
    private GameObject lastObj;
    public Variant variant;

    // Sprite values
    private SpriteRenderer spriteRenderer;
    private float alphaAdjust = 0.005f;
    private float alphaHolder;
    public int cooldown = 10;

    // Start method grabs tilemap
    public void Awake()
    {
        // Grabs active component if it exists
        if (this != null) active = this;
        else active = null;

        // Sets static variables on start
        tileGrid = new GridSystem();
        tileGrid.cells = new Dictionary<Vector2Int, Cell>();
        selected = null;
        position = new Vector2(0, 0);
        offset = new Vector2(0, 0);
        lastObj = null;

        // Sets static anim variables
        spriteRenderer = GetComponent<SpriteRenderer>();
        alphaHolder = alphaAdjust;
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

    private void OffsetBuilding()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (selected != null)
        {
            if (selected.snap) active.transform.position = new Vector2(5 * Mathf.Round(mousePos.x / 5) + offset.x, 5 * Mathf.Round(mousePos.y / 5) + offset.y);
            else active.transform.position = new Vector2(mousePos.x + offset.x, mousePos.y + offset.y);
        }

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
    public void SetBuilding(Entity entity)
    {
        spriteRenderer.sprite = Sprites.GetSprite(entity.name);
        transform.localScale = new Vector2(entity.size, entity.size);
        selected = entity;
        if (entity != null) offset = entity.tile.offset;
    }

    // Creates a building
    public void CmdCreateBuilding()
    {
        // Check if active is null
        if (selected == null || selected.obj == null) return;

        // Check if snap is enabled
        if (!selected.snap)
        {
            RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, Vector2.zero);
            foreach(RaycastHit2D hit in hits)
                if (hit.collider.GetComponent<DefaultBuilding>() == null) return;

            if (cooldown != 0)
            {
                cooldown -= 1;
                return;
            }
            else cooldown = 10;
        }

        // Check to make sure the tiles are not being used
        if (!CheckTiles()) return;

        // Instantiate the object like usual
        RpcInstantiateObject(new EntityQueue(selected, position, Quaternion.identity));
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

        DefaultEnemy enemy = lastObj.GetComponent<DefaultEnemy>();
        if (enemy != null) enemy.variant = variant;

        lastObj.GetComponent<DefaultEntity>().Setup();

        SetCells(entity.scriptable, lastObj);
    }

    // Checks to make sure tile(s) isn't occupied
    public bool CheckTiles()
    {
        if (selected.tile.cells.Length > 0)
        {
            foreach (Tile.Cell cell in selected.tile.cells)
                if (tileGrid.RetrieveCell(Vector2Int.RoundToInt(new Vector2(position.x + cell.x, position.y + cell.y))) != null)
                    return false;
        }
        return true;
    }

    public void SetCells(Entity entity, GameObject obj)
    {
        // Attempt to get the default building script
        DefaultBuilding building = obj.GetComponent<DefaultBuilding>();

        // Set the tiles on the grid class
        if (entity.tile.cells.Length > 0)
        {
            foreach (Tile.Cell cell in entity.tile.cells)
                tileGrid.SetCell(Vector2Int.RoundToInt(new Vector2(obj.transform.position.x + cell.x, obj.transform.position.y + cell.y)), true, entity.tile, building);
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
}
