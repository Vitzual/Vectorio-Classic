using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "New Currency", menuName = "Vectorio/Currency")]
public class Currency : IdentifiableScriptableObject
{
    public new string name;
    [TextArea]
    public string description;
    public Resource.Type type;
    public bool unlimited;
    public int minAmount;
    public int maxAmount;
    public Sprite sprite;
    public TileBase tile;
    public Perlin perlin;
}
