using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Tile", menuName = "Buildings/Building")]
public class Building : Tile
{
    // Building base variables
    public int health;
    public int maxHealth;
    public int cost;
    public int power;
    public int heat;
    public Material material;
    public ParticleSystem particle;
}
