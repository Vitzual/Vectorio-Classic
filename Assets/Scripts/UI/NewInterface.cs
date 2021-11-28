using Michsky.UI.ModernUIPack;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NewInterface : MonoBehaviour
{
    public static NewInterface active;
    public GameObject quitMenu;
    public GameObject loadingScreen;
    public ButtonManager saveButton;
    public ButtonManager reloadButton;

    public int fps;
    public TextMeshProUGUI fpsText;

    public void Awake()
    {
        active = this;

        if (fpsText != null)
            InvokeRepeating("UpdateFPS", 0.2f, 1f);
    }

    public void Start()
    {
        InputEvents.active.onInventoryPressed += ToggleBuildingMenu;
    }

    public void UpdateFPS()
    {
        fps = (int)(1f / Time.unscaledDeltaTime);
        fpsText.text = fps + "fps";
    }

    // Toggle the quit menu
    public void ToggleQuitMenu()
    {
        // Check if other panels open
        if (!CheckPanels()) return;

        // Check if menu open. If so, reset and open
        if (quitMenu.activeSelf)
        {
            quitMenu.SetActive(false);
            Time.timeScale = 1f;
        }
        else
        {
            if (InstantiationHandler.amountPlaced > 0) reloadButton.buttonText = "RELOAD";
            else reloadButton.buttonText = "RESEED";
            reloadButton.UpdateUI();

            saveButton.buttonText = "SAVE";
            saveButton.UpdateUI();

            quitMenu.SetActive(true);
            Time.timeScale = 0f;
        }
    }

    // Toggle the building menu
    public void ToggleBuildingMenu()
    {
        // Check animation in progress
        if (GuardianHandler.animInProgress) return;

        // Check if other panels open
        if (!CheckPanels()) return;
        else Inventory.Open();
    }

    // Check all UI panels
    public bool CheckPanels()
    {
        if (SettingsUI.isOpen)
        {
            SettingsUI.active.Back();
            return false;
        }
        if (Inventory.isOpen)
        {
            Inventory.CloseMenu();
            return false;
        }
        else if (StatsPanel.isOpen)
        {
            StatsPanel.CloseMenu();
            return false;
        }
        else if (ResearchUI.isOpen)
        {
            ResearchUI.CloseMenu();
            return false;
        }
        return true;
    }

    // Save game
    public void Save()
    {
        if (GuardianHandler.active.guardians.Count == 0)
        {
            Gamemode.active.SaveGame();
            saveButton.buttonText = "SAVED";
            saveButton.UpdateUI();
        }
    }

    // Reload game
    public void Reload()
    {
        if (InstantiationHandler.amountPlaced > 0)
        {
            // Clear registry
            Buildables.ClearRegistry();

            // Reset interface variables
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

    // Quit the game
    public void QuitGame()
    {
        Application.Quit();
    }
}
