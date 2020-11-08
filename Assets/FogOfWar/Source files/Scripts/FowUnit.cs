using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FowUnit : MonoBehaviour {
    public float radius=10;
    public float edgeSharpness;


    void OnEnable() {
        Fow_Script.fowUnits.Add(this);
    }
    void OnDisable() {
        Fow_Script.fowUnits.Remove(this);
    }
    void OnDestroy() {
        Fow_Script.fowUnits.Remove(this);
    }

    // Update is called once per frame
    void Update () {
		
	}
}
