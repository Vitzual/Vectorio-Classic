using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "New Currency", menuName = "Vectorio/Currency")]
public class Currency : IdentifiableScriptableObject
{
    public new string name;
    public string description;
    public Sprite sprite;
    public TileBase tile;
}
