using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DissolveSphere : MonoBehaviour {

    Material mat;

    private void Start() {
        mat = GetComponent<Renderer>().material;
    }

    private void Update() {
        mat.SetFloat("_DissolveAmount", Mathf.Sin(Time.time) / 2 + 0.1f);
    }
}