using System.Collections.Generic;
using UnityEngine;

public class Grid
{
    // Cell class. Holds info about each cell
    public class Cell
    {
        public Cell(bool occupied, Tile tile, GameObject obj)
        {
            this.occupied = occupied;
            this.tile = tile;
            this.obj = obj;
        }

        public bool occupied;
        public Tile tile;
        public GameObject obj;
    }

    // Holds a dictionary of all cells
    // Int represents coords of the tile 
    public Dictionary<Vector2Int, Cell> cells;

    // Grid values
    public int gridSize;
    public int cellSize;

    public Cell RetrieveCell(Vector2Int coords)
    {
        if (cells.TryGetValue(coords, out Cell cell))
        {
            return cell;
        }
        else return null;
    }

    public void SetCell(Vector2Int coords, bool occupy, Tile tile, GameObject obj)
    {
        if (cells.TryGetValue(coords, out Cell cell))
        {
            cell.tile = tile;
            cell.occupied = occupy;
            cell.obj = obj;
        }
        else cells.Add(coords, new Cell(occupy, tile, obj));
    }
}
