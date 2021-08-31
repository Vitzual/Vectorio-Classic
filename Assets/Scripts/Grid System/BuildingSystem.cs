using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class BuildingSystem : NetworkBehaviour
{
    // Grid variable
    public static GridSystem tileGrid;

    // Building variables
    public static BuildingSystem active;
    private static Tile selectedTile;
    private static Vector2 position;
    private static Vector2 offset;
    private static GameObject lastObj;
    private static bool changeSprite;

    // Sprite values
    private static SpriteRenderer spriteRenderer;
    private static float alphaAdjust = 0.005f;
    private static float alphaHolder;

    // Start method grabs tilemap
    public void Awake()
    {
        // Grabs active component if it exists
        if (this != null) active = this;
        else active = null;

        // Sets static variables on start
        tileGrid = new GridSystem();
        tileGrid.cells = new Dictionary<Vector2Int, GridSystem.Cell>();
        selectedTile = null;
        position = new Vector2(0, 0);
        offset = new Vector2(0, 0);
        changeSprite = false;
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

    private static void OffsetBuilding()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        active.transform.position = new Vector2(5 * Mathf.Round(mousePos.x / 5) + offset.x, 5 * Mathf.Round(mousePos.y / 5) + offset.y);
        position = active.transform.position;
    }

    // Adjusts the alpha transparency of the SR component 
    private void AdjustTransparency()
    {
        // Check if building changed
        if (changeSprite)
        {
            try
            {
                if (selectedTile != null) spriteRenderer.sprite = UnityEngine.Resources.Load<Sprite>("Sprites/Buildings/" + selectedTile.name);
                else spriteRenderer.sprite = UnityEngine.Resources.Load<Sprite>("Sprites/Interface/Empty");
            }
            catch
            {
                Debug.LogError("Sprite could not be retrieved. Please check you have placed the sprite in resources with the correct name!");
            }
            changeSprite = false;
        }

        // Switches
        if (spriteRenderer.color.a >= 1f)
            alphaHolder = -alphaAdjust;
        else if (spriteRenderer.color.a <= 0f)
            alphaHolder = alphaAdjust;

        // Set alpha
        spriteRenderer.color = new Color(1f, 1f, 1f, spriteRenderer.color.a + alphaHolder);
    }

    // Sets the selected building
    public static void SetBuilding(Tile tile)
    {
        // Check if active is null
        if (active == null) return;

        changeSprite = true;
        selectedTile = tile;
        if (tile != null) offset = tile.offset;
    }

    // Creates a building
    public static void CmdCreateBuilding()
    {
        // Check if active is null
        if (active == null || selectedTile == null || selectedTile.obj == null) return;

        // Check to make sure the tiles are not being used
        if (!CheckTiles()) return;

        // Instantiate the object like usual
        RpcInstantiateObject(selectedTile.obj, position, active.transform.rotation);
    }

    private static void RpcInstantiateObject(GameObject obj, Vector2 position, Quaternion rotation)
    {
        // Create the tile
        lastObj = Instantiate(obj, position, rotation);
        lastObj.name = obj.name;

        // Set the tiles on the grid class
        if (selectedTile.cells.Length > 0)
        {
            foreach (Tile.Cell cell in selectedTile.cells)
                tileGrid.SetCell(Vector2Int.RoundToInt(new Vector2(lastObj.transform.position.x + cell.x, lastObj.transform.position.y + cell.y)), true, selectedTile, lastObj);
        }
        else tileGrid.SetCell(Vector2Int.RoundToInt(lastObj.transform.position), true, selectedTile, lastObj);
    }

    // Checks to make sure tile(s) isn't occupied
    public static bool CheckTiles()
    {
        if (selectedTile.cells.Length > 0)
        {
            foreach (Tile.Cell cell in selectedTile.cells)
                if (tileGrid.RetrieveCell(Vector2Int.RoundToInt(new Vector2(position.x + cell.x, position.y + cell.y))) != null)
                    return false;
        }
        else return tileGrid.RetrieveCell(Vector2Int.RoundToInt(position)) == null;
        return true;
    }
}
