[System.Serializable]
public class Cell
{
    public Cell(bool occupied, Building tile, BaseTile obj)
    {
        this.occupied = occupied;
        this.tile = tile;
        this.obj = obj;
    }

    public bool occupied;
    public Building tile;
    public BaseTile obj;
}
