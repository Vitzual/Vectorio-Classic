using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpAndDown : MonoBehaviour
{
    private float rotation = 2;
    private int amountToRotate = 0;
    private Vector3 scaleChange;
    private Vector3 originalSize;
    private bool growing = true;

    private void Start()
    {
        originalSize = transform.localScale;
        scaleChange = new Vector3(0.0005f, 0.0005f, 0.0005f);
    }

    // Update is called once per frame
    void Update()
    {
        if (growing)
        {
            transform.localScale += scaleChange;
            transform.Rotate(0, 0, rotation * Time.deltaTime);
            amountToRotate++;

            if (amountToRotate >= 250) growing = false;
        }
        else
        {
            transform.localScale -= scaleChange;
            transform.Rotate(0, 0, -rotation * Time.deltaTime);
            amountToRotate--;

            if (amountToRotate <= 0)
            {
                transform.localScale = originalSize;
                growing = true;
            }
        }
    }
}
