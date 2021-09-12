using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : ScriptableObject
{
    // Entity description
    public new string name;
    [TextArea] public string description;
    public GameObject obj;

    // Building base variables
    public int health;
    public int order;
    [HideInInspector] public int maxHealth;

    // Holds active amount in scene
    [HideInInspector] public int active = 0;
}
