using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "New Parent Building", menuName = "Items/Tile")]
public class Tile : Item
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
