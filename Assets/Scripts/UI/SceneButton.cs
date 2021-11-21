using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneButton : MonoBehaviour
{
    public void LoadScene(string name) { SceneManager.LoadScene(name); }
}
