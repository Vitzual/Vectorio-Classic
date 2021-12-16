using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Cosmetic", menuName = "Building/Cosmetic")]
public class Cosmetic : IdentifiableScriptableObject
{
    // Cosmetic part
    [System.Serializable]
    public class Layer
    {
        public Sprite model;
        public Color color;
        public Material material;
        public Vector2 position;
        public int order;
    }

    [System.Serializable]
    public class Particle
    {
        public ParticleSystem effect;
        public Material material;
        public Vector2 position;
        public int order;
    }

    // Cosmetic variables
    public new string name;
    public BaseEntity entity;
    public Sprite hologram;
    public string description;
    public bool isUnlockable;

    // Cosmetic parts
    public List<Layer> layers;
    public List<Particle> particles;
}
