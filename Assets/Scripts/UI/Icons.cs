using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Icons : MonoBehaviour
{
    [System.Serializable]
    public class Icon
    {
        public string name;
        public Sprite icon;
    }
    public Icon[] _icons;
    public static Icon[] icons;

    public static void GetIcon(string name)
    {
        
    }
    
    
}
