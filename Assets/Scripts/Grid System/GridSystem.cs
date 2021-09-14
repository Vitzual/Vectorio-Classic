using System.Collections.Generic;
using UnityEngine;

public class GridSystem
{
    // Holds a dictionary of all cells
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

    public void SetCell(Vector2Int coords, bool occupy, Tile tile, DefaultBuilding building)
    {
        if (cells.TryGetValue(coords, out Cell cell))
        {            cell.tile = tile;
            cell.occupied = occupy;
            cell.building = building;
        }
        else cells.Add(coords, new Cell(occupy, tile, building));
    }
}
