using UnityEngine.SceneManagement;
using UnityEngine;

public class EndCanvas : MonoBehaviour
{

    public CanvasGroup screen;

    void Update()
    {
        if(screen.alpha < 1)
        {
            screen.alpha += 0.01f;
        }
    }

    public void SetAlpha(float alpha)
    {
        screen.alpha = alpha;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("Survival");
    }

    public void ReturnToMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}
