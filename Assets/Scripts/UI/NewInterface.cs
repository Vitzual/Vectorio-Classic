using Michsky.UI.ModernUIPack;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Mirror;
using HeathenEngineering.SteamworksIntegration;
using HeathenEngineering.SteamworksIntegration.API;

public class NewInterface : MonoBehaviour
{
    public static NewInterface active;

    public GameObject quitMenu;
    public GameObject loadingScreen;
    public ButtonManager saveButton;
    public ButtonManager reloadButton;
    public CanvasGroup canvasGroup;
    public TextMeshProUGUI friendcode;
    public Toggle onlineEnabled;

    public void Awake()
    {
        active = this;
    }

    public void Start()
    {
        InputEvents.active.onInventoryPressed += ToggleBuildingMenu;
        InputEvents.active.onMapPressed += DebugToggle;

        onlineEnabled.isOn = !Gamemode.online.privateSession;
        UserData userData = User.Client.Id;
        friendcode.text = "<b>FRIEND CODE: </b>" + userData.cSteamId.ToString();
    }

    public void OnOnlineChanged(bool enabled)
    {
        if (enabled)
        {
            Gamemode.online.privateSession = false;
            Gamemode.online.maxConnections = 10;
            NetworkManagerSF.active.maxConnections = 10;
        }
        else
        {
            Gamemode.online.privateSession = true;
            Gamemode.online.maxConnections = 1;
            NetworkManagerSF.active.maxConnections = 1;
        }
    }

    public void DebugToggle()
    {
        ToggleUI(!canvasGroup.interactable);
    }

    public void ToggleUI(bool status)
    {
        if (status)
        {
            canvasGroup.alpha = 1f;
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
        }
        else
        {
            canvasGroup.alpha = 0f;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        }
    }

    // Toggle the quit menu
    public void ToggleQuitMenu()
    {
        // Check if other panels open
        if (!CheckPanels() || CameraController.mapEnabled) return;

        // Check if UI isenabled
        if (canvasGroup.alpha == 0f) ToggleUI(true);

        // Check if menu open. If so, reset and open
        if (quitMenu.activeSelf)
        {
            quitMenu.SetActive(false);
            Time.timeScale = 1f;
        }
        else
        {
            if (InstantiationHandler.amountPlaced > 1 || 
                NetworkManagerSF.active.maxConnections > 1) 
                reloadButton.GetComponent<Button>().interactable = false;
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
        if (InstantiationHandler.amountPlaced <= 1)
        {
            #pragma warning disable CS0612
            WorldGenerator.active.Reseed();
            #pragma warning restore CS0612
        }
    }

    // Quit the game
    public void QuitGame()
    {
        // Check if client active
        // if (NetworkServer.active) NetworkManagerSF.active.StopHost();
        // if (NetworkClient.active) NetworkManagerSF.active.StopClient();

        Application.Quit();
    }
}
