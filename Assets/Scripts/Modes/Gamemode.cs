using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Mirror;

public class Gamemode : NetworkBehaviour
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
            List<Tile> tiles = new List<Tile>();
            var loadedObjects = Resources.LoadAll("GameObjects", typeof(GameObject)).Cast<GameObject>();

            foreach(GameObject obj in loadedObjects)
            {
                Tile tile = obj.GetComponent<Tile>();
                if (tile != null) tiles.Add(tile);
            }

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
