using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    public float movementspeed=5;
	void Update () {
        float x = 0;
        float y = 0;
        if (Input.GetKey(KeyCode.D)) x = 1;
        if (Input.GetKey(KeyCode.A)) x = -1;
        if (Input.GetKey(KeyCode.W)) y = 1;
        if (Input.GetKey(KeyCode.S)) y = -1;
        transform.position += new Vector3(x, 0, y) * Time.deltaTime * movementspeed;
	}
    private void OnGUI() {
        GUI.color = Color.white;
        GUI.Label(new Rect(10, 10, 200, 25), "WASD to move player");
        GUI.Label(new Rect(10, 10+25, 200, 25), "Arrow keys to move camera");
    }
}
