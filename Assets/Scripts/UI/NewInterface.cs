using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewInterface : MonoBehaviour
{
    public static bool isOpen;
    public GameObject buildingMenu;
    public GameObject quitMenu;

    public void Start()
    {
        InputEvents.active.onInventoryPressed += ToggleBuildingMenu;
        InputEvents.active.onEscapePressed += ToggleQuitMenu;
    }

    public void ToggleQuitMenu()
    {
        if (buildingMenu.activeSelf)
            ToggleBuildingMenu();
        else
        {
            quitMenu.SetActive(!quitMenu.activeSelf);
            isOpen = quitMenu.activeSelf;

            if (quitMenu.activeSelf) Time.timeScale = 0f;
            else Time.timeScale = 1f;
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ToggleBuildingMenu()
    {
        buildingMenu.SetActive(!buildingMenu.activeInHierarchy);
        isOpen = buildingMenu.activeSelf;
    }
}
