using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Guardian", menuName = "Enemy/Guardian")]
public class Guardian : Enemy
{
    public int heat;
    public int newMaxHeat;
    public ParticleSystem particleEffect;
    public string directionName;
    public Vector2 guardianSpawnPosition;
    public Vector2 guardianLerpPosition;
}
