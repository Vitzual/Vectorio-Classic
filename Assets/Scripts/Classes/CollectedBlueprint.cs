using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectedBlueprint
{
    // Constructor
    public CollectedBlueprint(Blueprint blueprint, Blueprint.Rarity rarity)
    {
        this.blueprint = blueprint;
        this.rarity = rarity;
    }

    public Blueprint blueprint;
    public Blueprint.Rarity rarity;
}
