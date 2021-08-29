using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "New Tile", menuName = "Buildings/Tile")]
public class Tile : ScriptableObject
{
    public new string name;
    [TextArea] public string description;
    public GameObject obj;
    public Sprite icon;

    [System.Serializable]
    public struct Cell
    {
        public float x;
        public float y;
    }

    public Cell[] cells;
    public Vector2 offset;
}
