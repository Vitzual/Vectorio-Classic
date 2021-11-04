using UnityEngine;
using System.Collections.Generic;

public class Hub : BaseTile
{
    public Building hub;

    // On start, assign weapon variables
    void Start()
    {
        InstantiationHandler.active.SetCells(hub, transform.position, this);
    }
}
