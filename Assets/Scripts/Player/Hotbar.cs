using UnityEngine;

public class Hotbar
{
    // Parameterized constructor
    public Hotbar(int hotbarSize)
    {
        slot = new Tile[hotbarSize];
        this.hotbarSize = hotbarSize;
    }

    // Default constructor
    public Hotbar()
    {
        slot = new Tile[9];
        hotbarSize = 9;
    }

    // Hotbar variables
    public Tile[] slot;
    public int hotbarSize;

    // Sets a hotbar slot and broadcasts it 
    public void SetSlot(Tile tile, int slot)
    {
        if (slot < hotbarSize && slot >= 0)
        {
            this.slot[slot] = tile;
            Events.active.HotbarSet(tile, slot);
        }
        else Debug.LogError("Slot number was outside the bounds of the hotbar!");
    }
}
