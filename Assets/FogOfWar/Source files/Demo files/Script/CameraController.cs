using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {


    public float movementspeed = 5;
    void Update() {
        float x = 0;
        float y = 0;
        if (Input.GetKey(KeyCode.RightArrow)) x = 1;
        if (Input.GetKey(KeyCode.LeftArrow)) x = -1;
        if (Input.GetKey(KeyCode.UpArrow)) y = 1;
        if (Input.GetKey(KeyCode.DownArrow)) y = -1;
        transform.position += new Vector3(x, 0, y) * Time.deltaTime * movementspeed;
    }
}
