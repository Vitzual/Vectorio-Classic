using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Tilemaps;

public class Tile : Entity
{
    [System.Serializable]
    public struct Cell
    {
        public float x;
        public float y;
    }

    public Cell[] cells;
    public Vector2 offset;
}
