using UnityEngine.SceneManagement;
using UnityEngine;

public class EndCanvas : MonoBehaviour
{

    protected CanvasGroup screen;

    private void Start()
    {
        screen = GetComponent<CanvasGroup>();
    }

    void Update()
    {
        if(screen.alpha < 1)
        {
            screen.alpha += 0.001f;
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("Creative");
    }

    public void ReturnToMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}
