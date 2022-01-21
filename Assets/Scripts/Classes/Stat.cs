using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Stat
{
    public Stat(string name, float value, float modifier = 0, Sprite icon = null, bool isResource = false)
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

    // I hate this method too, dont worry (and idc that it's a magic string)
    public float GetModifier()
    {
        switch(name.ToLower())
        {
            case "damage":
                return Research.damageBoost;
            case "health":
                return Research.healthBoost;
            case "pierce":
                return Research.pierceBoost;
            case "bullet":
                return Research.bulletBoost;
            case "firerate":
                return Research.firerateBoost;
            default:
                Debug.Log("Could not retrieve stat " + name + ", check GetModifier() method!");
                return 0;
        }
    }
}
