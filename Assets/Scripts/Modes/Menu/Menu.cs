using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using System;
using Michsky.UI.ModernUIPack;

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

    // Camera target
    public Camera cam;
    [HideInInspector]
    public Transform camTarget;
    public Transform newSaveLocation, mainLocation;
    public List<GameObject> newSaveVariables, mainVariables;
    public bool cameraMoving;

    // Save variabels
    public Dictionary<int, SaveButton> saveButtons = new Dictionary<int, SaveButton>();
    public SaveButton saveButton;
    public Transform saveList;
    public GameObject loadingScreen;
    public int availableSave = -1;

    // Outdated info
    public int deleteIndex = -1;
    public string deletePath = "";
    public ModalWindowManager confirmDelete;
    public int outdatedIndex = -1;
    public ModalWindowManager outdated;

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
        Gamemode.saveData = null;
        Gamemode.difficulty = null;
        Gamemode.saveName = "Unnamed Save";
        Gamemode.savePath = "/world_0.vectorio";
        Gamemode.seed = "Vectorio";
        Gamemode.loadGame = false;

        // Reset time scale
        Time.timeScale = 1f;

        // Set camera target
        camTarget = mainLocation;

        // Setup dictionary
        buttons = new Dictionary<string, MenuButton>();
        foreach(MenuButton button in _buttons)
            buttons.Add(button.buttonName, button);
        _buttons = new List<MenuButton>();

        CheckSaves();
    }

    // Update method
    public void FixedUpdate()
    {
        if (cameraMoving)
        {
            cam.transform.position = Vector3.Lerp(cam.transform.position, camTarget.position, 4f * Time.deltaTime);
            if (camTarget == newSaveLocation && Vector2.Distance(cam.transform.position, camTarget.transform.position) <= 3f)
            {
                cameraMoving = false;
                EnableNewGameMenu();
            } 
            else if (camTarget == mainLocation && Vector2.Distance(cam.transform.position, camTarget.transform.position) <= 3f)
            {
                cameraMoving = false;
                EnableMainMenu();
            }
        }
    }

    // Start new game
    public void StartNewGame()
    {
        foreach (GameObject obj in mainVariables)
            obj.SetActive(false);

        camTarget = newSaveLocation;
        cameraMoving = true;
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

    // Enable new game menu
    public void EnableNewGameMenu()
    {
        foreach(GameObject obj in newSaveVariables)
            obj.SetActive(true);
    }

    // Go back to menu
    public void BackToMenu()
    {
        foreach (GameObject obj in newSaveVariables)
            obj.SetActive(false);

        camTarget = mainLocation;
        cameraMoving = true;
    }

    // Enable new game menu
    public void EnableMainMenu()
    {
        foreach (GameObject obj in mainVariables)
            obj.SetActive(true);
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
                if (saveData.worldVersion == "v0.3") button.outdated = true;

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
            else if (availableSave == -1) availableSave = i;
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

        Gamemode.saveData = button.saveData;
        Gamemode.difficulty = button.saveData.difficultyData;
        Gamemode.saveName = button.name.text;
        Gamemode.savePath = "/world_" + button.pathNumber + ".vectorio";
        Gamemode.seed = button.seed;
        Gamemode.loadGame = true;

        StartGame(button.mode, number);
    }

    // Confirm load
    public void ConfirmLoad()
    {
        LoadSave(outdatedIndex, false);
    }

    // Starts a game
    public void StartGame(string mode, int number = -1)
    {
        if (number == -1)
        {
            if (availableSave != -1) Gamemode.savePath = "/world_" + availableSave + ".vectorio";
            else Gamemode.savePath = "/world_1.vectorio";
        }

        loadingScreen.SetActive(true);
        SceneManager.LoadScene(mode);
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
