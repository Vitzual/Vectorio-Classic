using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Building", menuName = "Building/Building")]
public class Building : Entity
{
    // Lock things
    [Header("Unlock Requirement")]
    public int unlockOrder;
    public string unlockDesc;
    public bool isUnlocked = true;
}
