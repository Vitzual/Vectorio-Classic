using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Stage", menuName = "Stage")]
public class Stage : IdentifiableScriptableObject
{
    // Stage variables
    public Variant variant;
    public Guardian guardian;
    public Vector2 guardianSpawnPos;
    public Border.Direction guardianDirection;

    // Materials
    public Material borderOutline;
    public Color borderFill;

    // Heat variables
    public int heat; // Exclusive

    // Next stage
    public Stage previousStage;
    public Stage nextStage;
}
