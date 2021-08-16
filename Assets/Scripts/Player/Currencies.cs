using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource : MonoBehaviour
{
    public enum Type
    {
        Gold,
        Essence,
        Iridium
    }
    public Dictionary<Type, int> type = new Dictionary<Type, int>();

    public static void AddResource(int amount)
    {
        
    }
}
