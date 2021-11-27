using Michsky.UI.ModernUIPack;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NewInterface : MonoBehaviour
{
    public static NewInterface active;
    public static bool isOpen;
    public GameObject quitMenu;
    public GameObject loadingScreen;
    public ButtonManager saveButton;
    public ButtonManager reloadButton;

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

        if (InstantiationHandler.active.amountPlaced > 0) reloadButton.buttonText = "RELOAD";
        else reloadButton.buttonText = "RESEED";
        reloadButton.UpdateUI();

        saveButton.buttonText = "SAVE";
        saveButton.UpdateUI();
        quitMenu.SetActive(!quitMenu.activeSelf);

        if (quitMenu.activeSelf) Time.timeScale = 0f;
        else Time.timeScale = 1f;
    }

    public void Save()
    {
        saveButton.buttonText = "SAVED";
        saveButton.UpdateUI();
        Gamemode.active.SaveGame();
    }

    public void Reload()
    {
        if (InstantiationHandler.active.amountPlaced > 0)
        {
            // Clear registry
            Buildables.ClearRegistry();

            // Reset interface variables
            isOpen = false;
            Time.timeScale = 1f;

            // Load last save
            Gamemode.loadGame = true;
            loadingScreen.SetActive(true);
            SceneManager.LoadScene("Survival");
        }

        #pragma warning disable CS0612
        else WorldGenerator.active.Reseed();
        #pragma warning restore CS0612
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
