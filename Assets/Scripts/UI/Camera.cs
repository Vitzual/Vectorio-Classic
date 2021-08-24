using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSetup : MonoBehaviour
{
    // Start is called before the first frame update
    public void Start()
    {
        Color CameraColor;
        ColorUtility.TryParseHtmlString("#333333", out CameraColor);
        GetComponent<Camera>().backgroundColor = CameraColor;
    }
}
