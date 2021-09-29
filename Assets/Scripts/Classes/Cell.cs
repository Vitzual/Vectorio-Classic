[System.Serializable]
public class Cell
{
    public Cell(bool occupied, Tile tile, BaseTile building)
    {
        this.occupied = occupied;
        this.tile = tile;
        this.building = building;
    }

    public bool occupied;
    public Tile tile;
    public BaseTile building;
}
