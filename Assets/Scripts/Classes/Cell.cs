[System.Serializable]
public class Cell
{
    public Cell(Entity entity, BaseTile obj, bool occupied, bool ghostTile)
    {
        this.ghostTile = ghostTile;
        this.occupied = occupied;
        this.entity = entity;
        this.obj = obj;
    }

    public bool ghostTile;
    public bool occupied;
    public Entity entity;
    public BaseTile obj;
}
