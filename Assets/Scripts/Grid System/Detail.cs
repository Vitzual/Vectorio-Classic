using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detail : MonoBehaviour
{
    public static Detail active;

    public List<GameObject> close;
    public List<GameObject> far;

    public bool closeEnabled = true;
    public bool farEnabled = true;

    // Start is called before the first frame update
    public void Start()
    {
        if (this != null)
            active = this;
        else active = null;
    }

    //
    public void ToggleClose()
    {
        Debug.Log("Toggling close for " + close.Count);

        closeEnabled = !closeEnabled;
        foreach (GameObject obj in close)
        {
            if (obj == null)
            {
                close.Remove(obj);
                continue;
            }
            obj.SetActive(!obj.activeSelf);
        }
    }

    public void ToggleFar()
    {
        Debug.Log("Toggling far for " + far.Count);

        farEnabled = !farEnabled;
        foreach (GameObject obj in far)
        {
            if (obj == null)
            {
                far.Remove(obj);
                continue;
            }
            obj.SetActive(!obj.activeSelf);
        }
    }
}
