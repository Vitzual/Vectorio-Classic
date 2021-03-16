using UnityEngine.SceneManagement;
using UnityEngine;

public class OpenMenu : MonoBehaviour
{
    public CanvasGroup thing;
    public int delay = 500;
    public float adjuster = 1f;

    public void Start()
    {
        InvokeRepeating("Lower", 0f, 0.01f);
    }

    public void Lower()
    {
        if (delay == 0)
        {
            adjuster -= 0.01f;
            thing.alpha = adjuster;
            if (adjuster <= 0f)
                SceneManager.LoadScene("Menu");
        }
        else delay -= 1;
        
    }
}
