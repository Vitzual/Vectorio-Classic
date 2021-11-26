using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewInterface : MonoBehaviour
{
    public static NewInterface active;
    public static bool isOpen;
    public GameObject quitMenu;
    public Button saveButton;

    public void Awake()
    {
        active = this;
    }

    public void Start()
    {
        InputEvents.active.onInventoryPressed += ToggleBuildingMenu;
        //InputEvents.active.onEscapePressed += ToggleQuitMenu;
    }

    public void ToggleQuitMenu()
    {
        if (StatsPanel.isActive)
        {
            StatsPanel.CloseMenu();
            return;
        }
        else if (ResearchUI.isOpen)
        {
            ResearchUI.active.CloseResearch();
            return;
        }

        saveButton.interactable = true;
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
        if (StatsPanel.isActive)
            StatsPanel.CloseMenu();

        if (Inventory.isOpen) Inventory.Close();
        else Inventory.Open();
    }
}
