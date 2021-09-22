using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewInterface : MonoBehaviour
{
    public GameObject buildingMenu;
    public GameObject quitMenu;
    private bool generatedBuildables = false;

    public void Start()
    {
        UIEvents.active.onBuildingMenuPressed += ToggleBuildingMenu;
        UIEvents.active.onQuitGame += ToggleQuitMenu;
    }

    public void ToggleQuitMenu()
    {
        quitMenu.SetActive(!quitMenu.activeSelf);

        if (quitMenu.activeSelf) Time.timeScale = 0f;
        else Time.timeScale = 1f;
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ToggleBuildingMenu()
    {
        if (!generatedBuildables)
        {
            Events.active.SetupBuildables();
            generatedBuildables = true;
        }

        buildingMenu.SetActive(!buildingMenu.activeInHierarchy);
    }
}
