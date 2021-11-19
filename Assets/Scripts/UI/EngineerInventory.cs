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

    public void AddBlueprint(Blueprint blueprint, Blueprint.Rarity rarity)
    {
        foreach(EngineerButton slot in slots)
            if (slot.blueprint == null)
                slot.SetButton(blueprint, rarity);
    }
}
