using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Guardian", menuName = "Vectorio/Enemy/Guardian")]
public class Guardian : EnemyData
{
    // Variable stuff
    public int minimumHeatValue;
    public int newMaximumHeat;

    // Colors
    public Color barBackgroundColor;
    public Color barForegroundColor;

    // Directional stuff
    public ParticleSystem particleEffect;
    public string directionName;
}
