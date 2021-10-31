using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawnable : MonoBehaviour
{
    public Resource.CurrencyType type;
    public float spawnScale;
    public float spawnThreshold;
    [HideInInspector] public float spawnOffset;

}
