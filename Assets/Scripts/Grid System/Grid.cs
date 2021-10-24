using System.Collections.Generic;
using UnityEngine;

public class Grid
{
    // Cell class. Holds info about each cell
    public class Cell
    {
        public Cell(bool occupied, Building building, BaseTile obj)
        {
            this.occupied = occupied;
            this.building = building;
            this.obj = obj;
        }

        public bool occupied;
        public Building building;
        public BaseTile obj;
    }

    // Holds a dictionary of all cells
    // Int represents coords of the tile 
    public Dictionary<Vector2Int, Cell> cells;

    // Grid values
    public int gridSize;
    public int cellSize;

    public BaseTile RetrieveTile(Vector2Int coords)
    {
        if (cells.TryGetValue(coords, out Cell cell))
        {
            return cell.obj;
        }
        else return null;
    }

    public Cell RetrieveCell(Vector2Int coords)
    {
        if (cells.TryGetValue(coords, out Cell cell))
        {
            return cell;
        }
        else return null;
    }

    public void SetCell(Vector2Int coords, bool occupy, Building tile = null, BaseTile obj = null)
    {
        if (cells.TryGetValue(coords, out Cell cell))
        {
            cell.building = tile;
            cell.occupied = occupy;
            cell.obj = obj;
        }
        else cells.Add(coords, new Cell(occupy, tile, obj));
        if (obj != null) obj.cells.Add(coords);
    }

    public void DestroyCell(Vector2Int coords)
    {
        if (cells.TryGetValue(coords, out Cell cell))
        {
            BaseTile building = cell.obj.GetComponent<BaseTile>();

            if (building != null)
            {
                for (int i = 0; i < building.cells.Count; i++)
                    cells.Remove(building.cells[i]);
                building.DestroyEntity();
            }
        }
    }
}