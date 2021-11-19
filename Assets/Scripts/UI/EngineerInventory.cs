using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EngineerInventory : MonoBehaviour
{
    public List<EngineerButton> slots;

    public void Start()
    {
        Events.active.onBlueprintCollected += AddBlueprint;
    }

    public void AddBlueprint(CollectedBlueprint blueprint)
    {
        foreach(EngineerButton slot in slots)
            if (slot.blueprint == null)
                slot.SetButton(blueprint);
    }
}
