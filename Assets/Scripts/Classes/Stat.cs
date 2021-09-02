using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Stat
{
    public Stat(string name, float value, float modifier, Sprite icon, bool isResource = false)
    {
        this.name = name;
        this.value = value;
        this.modifier = modifier;
        this.icon = icon;
        this.isResource = isResource;
    }

    public string name;
    public float value;
    public Sprite icon;
    public float modifier;
    public bool isResource;
}
