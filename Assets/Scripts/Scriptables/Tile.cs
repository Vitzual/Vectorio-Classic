using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Tilemaps;

public class Tile : ScriptableObject
{
    public new string name;
    [TextArea] public string description;
    public GameObject obj;

    [System.Serializable]
    public struct Cell
    {
        public float x;
        public float y;
    }

    public Cell[] cells;
    public Vector2 offset;
}
