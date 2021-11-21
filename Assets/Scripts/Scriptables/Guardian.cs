using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Guardian", menuName = "Enemy/Guardian")]
public class Guardian : Enemy
{
    // Variable stuff
    public int heat;
    public int newMaxHeat;

    // Colors
    public Color barBackgroundColor;
    public Color barForegroundColor;

    // Directional stuff
    public ParticleSystem particleEffect;
    public string directionName;
    public Vector2 guardianSpawnPosition;
    public Vector2 guardianLerpPosition;
}
