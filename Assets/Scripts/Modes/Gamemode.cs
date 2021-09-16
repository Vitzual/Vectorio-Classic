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
        Events.active.onLeftMousePressed += PlaceBuilding;
        Events.active.setupBuildables += InitEntities;
    }

    // Tells the gamemode how to handle building placements
    public virtual void PlaceBuilding()
    {
        Debug.Log("Mode does not contain definition for building placed");
    }

    // Tells the gamemode how to generate inventory
    public virtual void InitEntities()
    {
        Debug.Log("Mode does not contain definition for initializing buildables");
    }
}
