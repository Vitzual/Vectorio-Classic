using UnityEngine;

public class Hotbar
{
    // Parameterized constructor
    public Hotbar(int hotbarSize)
    {
        slot = new Entity[hotbarSize];
        this.hotbarSize = hotbarSize;
    }

    // Default constructor
    public Hotbar()
    {
        slot = new Entity[9];
        hotbarSize = 9;
    }

    // Hotbar variables
    public Entity[] slot;
    public int hotbarSize;

    // Sets a hotbar slot and broadcasts it 
    public void SetSlot(Entity entity, int slot)
    {
        if (slot < hotbarSize && slot >= 0)
        {
            this.slot[slot] = entity;
            //Events.active.HotbarSet(entity, slot);
        }
        else Debug.LogError("Slot number was outside the bounds of the hotbar!");
    }
}
