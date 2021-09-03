using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Mirror;

public class Gamemode : MonoBehaviour
{
    // Gamemode information
    public new string name;
    public string version;

    // Register for events
    public void Start()
    {
        Events.active.onBuildingPlaced += PlaceBuilding;
    }

    // Tells the gamemode how to handle building placements
    public virtual void PlaceBuilding()
    {
        Debug.Log("Mode does not contain definition for building placed");
    }
}
