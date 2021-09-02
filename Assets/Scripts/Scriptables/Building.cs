using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Tile", menuName = "Buildings/Building")]
public class Building : Tile
{
    // Resource class
    [System.Serializable]
    public class Resources
    {
        public Resource.Currency resource;
        public Sprite icon;
        public int amount;
    }

    // Building base variables
    public int health;
    public int maxHealth;
    
    // Resources
    public Resources[] resources;

    // Materials
    public Material material;
    public ParticleSystem particle;
}
