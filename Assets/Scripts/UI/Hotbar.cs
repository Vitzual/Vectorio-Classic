using UnityEngine;

public class Hotbar : MonoBehaviour
{
    // Grab the panel
    public Panel panel;

    // Hotbar variables
    public HotbarSlot[] slots;

    public void Start()
    {
        InputEvents.active.onNumberInput += UseSlot;
        UIEvents.active.onHotbarPressed += UseSlot;
    }

    // Sets a hotbar slot and broadcasts it 
    public void SetSlot(Entity entity, int index)
    {
        Debug.Log("Settings slot " + index + " " + entity.name);

        if (index < slots.Length && index >= 0)
            slots[index].SetSlot(entity, Sprites.GetSprite(entity.name));
        else Debug.LogError("Slot number was outside the bounds of the hotbar!");
    }

    // Broadcasts to building handler
    public void UseSlot(int index)
    {
        Debug.Log("Using slot " + index);

        if (panel != null && panel.settingHotbar)
        {
            SetSlot(panel.entity, index);
            panel.DisableHotbar();
        }
        else
        {
            if (index < slots.Length && index >= 0)
                UIEvents.active.EntityPressed(slots[index].entity);
            else Debug.LogError("Slot number was outside the bounds of the hotbar!");
        }
    }
}
