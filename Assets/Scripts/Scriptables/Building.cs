using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "New Building", menuName = "Building/Building")]
public class Building : Entity
{
    // Resource class
    [System.Serializable]
    public class Resources
    {
        public Resource.CurrencyType resource;
        public int amount;
        public bool add;
        public bool storage;
    }

    // Cell class
    [System.Serializable]
    public struct Cell
    {
        public float x;
        public float y;
    }

    [Header("Building Variables")]
    public Cell[] cells;
    public Resources[] resources;
    public bool restrictPlacement;
    public Resource.CurrencyType placedOn;

    // Creates stats
    public override void CreateStats(Panel panel)
    {
        // Resource stats
        foreach (Resources type in resources)
        {
            string name = Resource.active.GetName(type.resource);
            Sprite sprite = Resource.active.GetSprite(type.resource);
            panel.CreateStat(new Stat(name, type.amount, 0, sprite, true));
        }
    }
}
