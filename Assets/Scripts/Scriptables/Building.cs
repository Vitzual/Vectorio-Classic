using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Building", menuName = "Building/Building")]
public class Building : Entity
{
    // Resource class
    [System.Serializable]
    public class Resources
    {
        public Resource.CurrencyType resource;
        public int amount;
    }

    // Resources
    public Resources[] resources;

    // Lock things
    [Header("Unlock Requirement")]
    public int unlockOrder;
    public string unlockDesc;
    public bool isUnlocked = true;

    public override void CreateStats(Panel panel)
    {
        // Resource stats
        foreach (Resources type in resources)
        {
            string name = Resource.active.GetName(type.resource);
            Sprite sprite = Resource.active.GetSprite(type.resource);
            panel.CreateStat(new Stat(name, type.amount, 0, sprite, true));
        }

        panel.CreateStat(new Stat("Health", health, healthModifier, Sprites.GetSprite("Health")));
    }
}
