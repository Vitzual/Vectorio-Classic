using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stat
{
    public Stat(string name, int value, int modifier, Sprite icon, bool isResource = false)
    {
        this.name = name;
        this.value = value;
        this.modifier = modifier;
        this.icon = icon;
        this.isResource = isResource;
    }

    public string name;
    public int value;
    public int modifier;
    public Sprite icon;
    public bool isResource;
}
