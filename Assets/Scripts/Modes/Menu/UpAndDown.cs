using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpAndDown : MonoBehaviour
{
    private float rotation = 2;
    private Vector3 scaleChange;
    private bool growing = true;

    private void Start()
    {
        scaleChange = new Vector3(0.0003f, 0.0003f, 0.0003f);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (growing)
        {
            transform.localScale += scaleChange;
            transform.Rotate(0, 0, rotation * Time.deltaTime);

            if (transform.localScale.x >= 1.1) growing = false;
        }
        else
        {
            transform.localScale -= scaleChange;
            transform.Rotate(0, 0, -rotation * Time.deltaTime);

            if (transform.localScale.x <= 1) growing = true;
        }
    }
}
