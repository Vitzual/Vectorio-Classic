using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneButton : MonoBehaviour
{
    public void LoadScene(string name)
    {
        if (name != "Menu")
        {
            NetworkManagerSF.active.onlineScene = name;
        }
        NetworkManagerSF.active.StartHost();
    }
}
