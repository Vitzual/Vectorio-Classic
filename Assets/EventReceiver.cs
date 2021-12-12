using Michsky.UI.ModernUIPack;
using UnityEngine;

public class EventReceiver : MonoBehaviour
{
    // Modal window
    public ModalWindowManager gameOverScreen;

    // Start is called before the first frame update
    public void Start()
    {
        Events.active.onHubDestroyed += GameOver;
    }

    public void GameOver()
    {
        gameOverScreen.OpenWindow();
    }
}
