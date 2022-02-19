using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceTile : BaseTile
{
    [SerializeField]
    public Resource.Type type;
    [HideInInspector] public int amount = 0;
    [HideInInspector] public bool isFull = false;
    [SerializeField]
    public bool isStorage;
    [HideInInspector] public bool hasAssignedDrone;

    public virtual int TakeResource()
    {
        Debug.Log("Entity cannot distribute resources");
        return 0;
    }

    public virtual int AddResources(int amount, bool showPopup)
    {
        Debug.Log("Entity does not accept resources");
        return amount;
    }
}
