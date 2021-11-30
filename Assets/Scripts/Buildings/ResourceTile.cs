using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceTile : BaseTile
{
    [SerializeField]
    public Resource.CurrencyType type;
    [HideInInspector] public int amount = 0;
    [HideInInspector] public bool isFull = false;
    [SerializeField]
    public bool isStorage;
    [HideInInspector] public bool hasAssignedDrone;

    public virtual int TakeResource()
    {
        Debug.Log("Script needs to override TakeResource() virtual method");
        return 0;
    }

    public virtual int AddResources(int amount, bool showPopup)
    {
        Debug.Log("Script needs to override AddResources() virtual method");
        return amount;
    }
}
