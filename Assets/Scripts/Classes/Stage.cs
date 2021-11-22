using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Stage", menuName = "Stage")]
public class Stage : IdentifiableScriptableObject
{
    // Guardian stage
    public enum GuardianStage
    {
        Revenant,
        Kraken,
        Atlas,
        Serpent
    }

    // Stage variables
    public Variant variant;
    public Guardian guardian;
    public Vector2 guardianSpawnPos;
    public GuardianStage type;
    public Border.Direction direction;

    // Heat variables
    public int heat; // Exclusive

    // Next stage
    public Stage nextStage;
}
