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

        try
        {
            List<Tile> tiles = Resources.LoadAll("Scriptables", typeof(Tile)).Cast<Tile>().ToList();
            Debug.Log("Loaded " + tiles.Count + " tiles from Resources/Scriptables");
            LoadScriptables(tiles);
        }
        catch
        {
            Debug.LogError("The folder Resources/Scriptables/ contains a non-scriptable object. Please remove it!");
        }
    }

    // Tells the gamemode how to handle building placements
    public virtual void PlaceBuilding()
    {
        Debug.Log("Mode does not contain definition for building placed");
    }

    // Loads scriptables to determine how to set them up
    //
    // Ex. Survival will load scriptables into a list of type Unlock
    // whereas Creative will load scriptables into an array of type Building
    public virtual void LoadScriptables(List<Tile> tiles)
    {
        Debug.Log("Mode does not contain definition for load scriptables");
    }
}
