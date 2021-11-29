using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LowresMap : MonoBehaviour
{
    // Lowres textures
    public static LowresMap active;
    public static Tilemap lowresMap;
    public static TileBase lowresTile;
    public Tilemap _lowresMap;
    public TileBase _lowresTile;
    public bool useLowresMap;

    // Get active instance
    public void Awake() { active = this; }

    // Start is called before the first frame update
    public void Start()
    {
        // Check if lowres being used
        useLowresMap = lowresMap != null && lowresTile != null;

        // Set lowres
        lowresMap = _lowresMap;
        lowresTile = _lowresTile;
    }

    // Check if a resource node exists
    public static void AddLowres(Vector2 coords, Color color)
    {
        Vector3Int adjustedCoords = new Vector3Int((int)(coords.x / 5), (int)(coords.y / 5), 0);
        Debug.Log("Setting lowres map coords at " + adjustedCoords);
        lowresMap.SetTile(adjustedCoords, lowresTile);
        lowresMap.SetColor(adjustedCoords, color);
    }

    // Check if a resource node exists
    public static void RemoveLowres(Vector2 coords)
    {
        Vector3Int adjustedCoords = new Vector3Int((int)(coords.x / 5), (int)(coords.y / 5), 0);
        lowresMap.SetTile(adjustedCoords, null);
    }
}
