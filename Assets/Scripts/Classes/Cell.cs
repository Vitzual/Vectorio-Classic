[System.Serializable]
public class Cell
{
    public Cell(bool occupied, Tile tile, DefaultBuilding building)
    {
        this.occupied = occupied;
        this.tile = tile;
        this.building = building;
    }

    public bool occupied;
    public Tile tile;
    public DefaultBuilding building;
}
