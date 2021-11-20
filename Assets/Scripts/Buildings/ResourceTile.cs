using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceTile : BaseTile
{
    public Resource.CurrencyType type;
    public int amount = 0;
    public bool isFull = false;
    public bool isStorage;
    public bool hasAssignedDrone;

    public virtual int TakeResource()
    {
        Debug.Log("Script needs to override TakeResource() virtual method");
        return 0;
    }

    public virtual int AddResources(int amount)
    {
        Debug.Log("Script needs to override AddResources() virtual method");
        return amount;
    }
}
