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

    public void SetCell(Vector2Int coords, bool occupy, Tile tile, BaseTile building)
    {
        if (cells.TryGetValue(coords, out Cell cell))
        {            
            cell.tile = tile;
            cell.occupied = occupy;
            cell.building = building;
        }
        else cells.Add(coords, new Cell(occupy, tile, building));
    }

    public void RemoveCell(Vector2Int coords)
    {
        if (cells.ContainsKey(coords))
            cells.Remove(coords);
    }

    public void DestroyCell(Vector2Int coords)
    {
        if (cells.ContainsKey(coords))
            cells[coords].building.DestroyEntity();
    }

    public void DestroyAllCells()
    {
        foreach (Cell cell in cells.Values)
            Recycler.AddRecyclable(cell.building.transform);
        cells = new Dictionary<Vector2Int, Cell>();
    }
}
