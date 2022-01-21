using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "New Building", menuName = "Building/Building")]
public class Building : Entity
{
    // Cell class
    [System.Serializable]
    public struct Cell
    {
        public float x;
        public float y;
    }

    [FoldoutGroup("Default Cosmetic")]
    public Cosmetic defaultCosmetic;
    [FoldoutGroup("Building Variables")]
    public Cell[] cells;
    [FoldoutGroup("Building Variables")]
    public Cost[] resources;
    [FoldoutGroup("Building Variables")]
    public float radialCheck = 0;
    [FoldoutGroup("Building Variables")]
    public bool restrictPlacement;
    [FoldoutGroup("Building Variables")]
    public bool isSellable = true;
    [FoldoutGroup("Building Variables")]
    public bool isSaveable = true;
    [FoldoutGroup("Building Variables")]
    public Resource.CurrencyType placedOn;
    [FoldoutGroup("Building Variables")]
    public int engineeringSlots;
    [FoldoutGroup("Building Variables")]
    public ParticleSystem deathParticle;

    // Creates stats
    public override void CreateStats(Panel panel)
    {
        // Resource stats
        Buildable buildable = Buildables.RequestBuildable(this);
        if (buildable != null)
        {
            panel.CreateStat(new Stat("Health", health, 0, Sprites.GetSprite("Health")));

            foreach (Cost type in buildable.resources)
            {
                string name = Resource.active.GetName(type.type);
                Sprite sprite = Resource.active.GetSprite(type.type);
                if (!type.storage) panel.CreateStat(new Stat(name, type.amount, 0, sprite, true));
            }
        }
        else
        {
            foreach (Cost type in resources)
            {
                string name = Resource.active.GetName(type.type);
                Sprite sprite = Resource.active.GetSprite(type.type);
                panel.CreateStat(new Stat(name, type.amount, 0, sprite, true));
            }
        }
    }
}
