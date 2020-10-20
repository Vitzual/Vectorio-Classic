using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{

    public GameObject MainMenu;
    public GameObject Changelog;
    private GameObject MainMenuHolder;
    private GameObject ChangelogHolder;

    public void Start()
    {
        Application.targetFrameRate = 300;
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Creative");
    }

    public void OpenChangelog()
    {
        MainMenuHolder = GameObject.Find("Main Menu");
        Destroy(MainMenuHolder);
        ChangelogHolder = Instantiate(Changelog, transform.position, Quaternion.identity);
        ChangelogHolder.name = "Changelog Menu";
    }

    public void OpenMainMenu()
    {
        ChangelogHolder = GameObject.Find("Changelog Menu");
        Destroy(ChangelogHolder);
        MainMenuHolder = Instantiate(MainMenu, transform.position, Quaternion.identity);
        MainMenuHolder.name = "Main Menu";
    }
}
