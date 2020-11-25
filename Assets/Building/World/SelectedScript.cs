using UnityEngine;

public class SelectedScript : MonoBehaviour
{
    private Vector3 scaleChange;
    private bool directionalGrowth = true;

    private void Start()
    {
        scaleChange = new Vector3(0.001f, 0.001f, 0);
        Debug.Log(transform.localScale);
    }

    private void Update()
    {
        if (directionalGrowth)
        {
            transform.localScale += scaleChange;
        }
        else
        {
            transform.localScale -= scaleChange;
        }

        if (transform.localScale.x > 1.2f)
        {
            directionalGrowth = false;
        } 
        else if (transform.localScale.x < 1f)
        {
            directionalGrowth = true;
        }
    }
}