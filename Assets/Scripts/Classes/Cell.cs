[System.Serializable]
public class Cell
{
    public Cell(bool occupied, Entity entity, BaseTile obj)
    {
        this.occupied = occupied;
        this.entity = entity;
        this.obj = obj;
    }

    public bool occupied;
    public Entity entity;
    public BaseTile obj;
}
