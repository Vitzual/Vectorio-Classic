using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using System;
using HeathenEngineering.SteamworksIntegration;
using Michsky.UI.ModernUIPack;
using Mirror;
using TMPro;

public class Menu : MonoBehaviour
{
    // Active instance
    public static Menu active;

    // Button actions enumerator
    public enum ButtonActions
    {
        LoadScene,
        OpenLink,
        Exit
    }

    // Steam friends list
    public SteamList steamList;

    // Camera target
    public Camera cam;
    public GameObject background;

    // Save variabels
    public Dictionary<int, SaveButton> saveButtons = new Dictionary<int, SaveButton>();
    public SaveButton saveButton;
    public Transform saveList;
    public GameObject loadingScreen;
    public int availableSave = -1;

    // Outdated info
    public int deleteIndex = -1;
    public string deletePath = "";
    public TextMeshProUGUI friendCode;
    public ModalWindowManager confirmDelete;
    public ModalWindowManager outdated;
    public int outdatedIndex = -1;

    [System.Serializable]
    public class MenuButton
    {
        public string buttonName;
        [FoldoutGroup("Button Variables")]
        public ButtonActions action;
        [FoldoutGroup("Button Variables")]
        public string argument;
    }

    // Buttons
    public List<MenuButton> _buttons;
    private Dictionary<string, MenuButton> buttons;

    public void Awake() { active = this; }

    // Start method
    public void Start()
    {
        // Clear registry
        Buildables.ClearRegistry();

        // Reset gamemode static variables
        NewSaveSystem.saveData = null;
        Gamemode.difficulty = null;
        Gamemode.stage = null;
        Gamemode.online = null;
        NewSaveSystem.saveName = "Unnamed Save";
        Gamemode.seed = "Vectorio";
        NewSaveSystem.loadGame = false;

        // Reset time scale
        Time.timeScale = 1f;
        SettingsUI.isOpen = false;
        Inventory.isOpen = false;
        ResearchUI.isOpen = false;
        StatsPanel.isOpen = false;

        // Setup dictionary
        buttons = new Dictionary<string, MenuButton>();
        foreach(MenuButton button in _buttons)
            buttons.Add(button.buttonName, button);
        _buttons = new List<MenuButton>();

        Instantiate(background, Vector2.zero, Quaternion.identity);
        CheckSaves();
    }

    // Open DLC
    public void OpenDLC(DownloadableContentObject dlc)
    {
        dlc.OpenStore();
    }

    // Join friend
    public void JoinFriend()
    {
        print("[SERVER] Attempting connection to client with ID " + friendCode.text);
        NetworkManagerSF.active.Join(friendCode.text);
    }

    // Load online friends
    public void GenerateFriendsList()
    {
        steamList.UpdateFriendsList();
    }

    // Delete agame
    public void DeleteGame()
    {
        if (deletePath != "" && deleteIndex != -1)
        {
            NewSaveSystem.DeleteGame(deletePath);
            Destroy(saveButtons[deleteIndex].gameObject);
            confirmDelete.CloseWindow();
        }
    }

    // Check saves
    public void CheckSaves()
    {
        for(int i = 0; i < 100; i++)
        {
            string path = Application.persistentDataPath + "/world_" + i + ".vectorio";
            if (File.Exists(path))
            {
                // Load json file
                string data = File.ReadAllText(path);
                SaveData saveData = JsonUtility.FromJson<SaveData>(data);

                // Create new save button
                SaveButton button = Instantiate(saveButton, Vector3.zero, Quaternion.identity);
                Debug.Log("Save button created for slot " + i);
                button.transform.SetParent(saveList);
                button.transform.SetSiblingIndex(i);
                button.rect.localScale = new Vector3(1, 1, 1);

                // Apply to save buttons
                button.saveData = saveData;
                button.savePath = path;
                button.name.text = saveData.worldName;
                button.pathNumber = i;
                button.seed = saveData.worldSeed;
                button.mode = saveData.worldMode;

                // Check for outdated thing
                if (saveData.worldVersion == "v0.3" || saveData.worldVersion == "v0.3.0" ||
                    saveData.worldVersion == "v0.3.1" || saveData.worldVersion == "v0.3.2") button.outdated = true;

                // Apply playtime
                TimeSpan time = TimeSpan.FromSeconds(saveData.worldPlaytime);
                string output = string.Format("{0:D2}d {1:D2}h {2:D2}m", time.Days, time.Hours, time.Minutes);
                button.timeAndVersion.text = "<b>" + output + "</b>\n" + saveData.worldVersion;

                // Apply version
                button.worldMode.text = saveData.worldMode + " (" + saveData.worldCompletion * 100 + "%)";

                // Set active
                button.obj.SetActive(true);
                saveButtons.Add(i, button);
            }
            else if (availableSave == -1)
            {
                availableSave = i;
                NewSaveSystem.savePath = "/world_"+ availableSave + ".vectorio";
                Debug.Log("Set new save path to " + NewSaveSystem.savePath);
            }
        }
    }

    // Load a save
    public void LoadSave(int number, bool warning)
    {
        if (warning)
        {
            outdatedIndex = number;
            outdated.OpenWindow();
            return;
        }

        SaveButton button = saveButtons[number];

        if (button == null)
        {
            Debug.Log("Save button could not be found, please report!");
            return;
        }

        NewSaveSystem.saveData = button.saveData;
        Gamemode.difficulty = button.saveData.difficultyData;
        NewSaveSystem.saveName = button.name.text;
        NewSaveSystem.savePath = "/world_" + button.pathNumber + ".vectorio";
        Gamemode.seed = button.seed;
        NewSaveSystem.loadGame = true;

        if (button.saveData.onlineData != null)
            Gamemode.online = button.saveData.onlineData;
        else Gamemode.online = new OnlineData();

        StartSurvivalGame(Gamemode.online.maxConnections, number);
    }

    // Confirm load
    public void ConfirmLoad()
    {
        LoadSave(outdatedIndex, false);
    }

    // Starts a game
    public void StartSurvivalGame(int connections, int number = -1)
    {
        // Get save path
        if (number == -1 && availableSave != -1)
            NewSaveSystem.savePath = "/world_" + availableSave + ".vectorio";
        else NewSaveSystem.savePath = "/world_" + number + ".vectorio";

        // Multiplayer settings
        NetworkManagerSF.active.maxConnections = connections;

        // Check network
        Debug.Log("[SERVER] Server / Host = " + NetworkServer.active + " / " + NetworkClient.active);
        if (NetworkServer.active) NetworkServer.Shutdown();
        if (NetworkClient.active) NetworkClient.Shutdown();
        Debug.Log("[SERVER] Server / Host = " + NetworkServer.active + " / " + NetworkClient.active);

        // Start the game
        NetworkManagerSF.active.onlineScene = "Survival";
        loadingScreen.SetActive(true);
        NetworkManagerSF.active.StartHost();
    }

    public void ExitGame()
    {
        Application.Quit(0);
    }

    // Menu button event
    public void ButtonClicked(string name)
    {
        // Attempt to get menu button
        MenuButton button;
        buttons.TryGetValue(name, out button);

        // Check if exists
        if (button != null)
        {
            switch(button.action)
            {
                case ButtonActions.LoadScene:
                    SceneManager.LoadScene(button.argument);
                    break;
                case ButtonActions.OpenLink:
                    Application.OpenURL(button.argument);
                    break;
                case ButtonActions.Exit:
                    Application.Quit(0);
                    break;
            }
        }
    }
}
