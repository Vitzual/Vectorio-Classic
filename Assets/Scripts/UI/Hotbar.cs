using UnityEngine;

public class Hotbar : MonoBehaviour
{
    // Grab the panel
    public Panel panel;

    // Hotbar variables
    public HotbarSlot[] _slots;
    public static HotbarSlot[] slots;

    // Default entities
    public Entity[] defaultEntities;

    public void Awake()
    {
        slots = new HotbarSlot[_slots.Length];
        for (int i = 0; i < slots.Length; i++)
            slots[i] = _slots[i];
    }

    public void Start()
    {
        InputEvents.active.onNumberInput += UseSlot;
        UIEvents.active.onHotbarPressed += UseSlot;
    }

    public void SetDefaultSlots()
    {
        for(int i = 0; i < defaultEntities.Length; i++)
        {
            if (i < slots.Length)
            {
                SetSlot(defaultEntities[i], i);
            }
        }
    }

    // Sets a hotbar slot and broadcasts it 
    public void SetSlot(Entity entity, int index)
    {
        Debug.Log("Settings slot " + index + " " + entity.name);

        if (index < slots.Length && index >= 0)
        {
            Buildable buildable = Buildables.RequestBuildable(entity);
            if (buildable != null && buildable.cosmetic != null)
                slots[index].SetSlot(entity, buildable);
            else slots[index].SetSlot(entity, Sprites.GetSprite(entity.name));
        }
        else Debug.LogError("Slot number was outside the bounds of the hotbar!");
    }

    // Broadcasts to building handler
    public void UseSlot(int index)
    {
        if (Inventory.isOpen) SetSlot(panel.entity, index);
        else if (slots[index].entity != null)
        {
            if (slots[index].cosmetic != null && slots[index].cosmetic.validateLocalApplication())
                slots[index].buildable.ApplyCosmetic(slots[index].cosmetic);
            if (index < slots.Length && index >= 0)
                UIEvents.active.EntityPressed(slots[index].entity, -1);
            else Debug.LogError("Slot number was outside the bounds of the hotbar!");
        }
    }
}
