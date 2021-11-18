using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Michsky.UI.ModernUIPack;
using TMPro;
using System.Collections.Generic;
using Sirenix.OdinInspector;

public class Menu : MonoBehaviour
{
    // Button actions enumerator
    public enum ButtonActions
    {
        ActivateDeactivate,
        LoadScene,
        OpenLink,
        Exit
    }

    [System.Serializable]
    public class MenuButton
    {
        public string buttonName;
        [FoldoutGroup("Button Variables")]
        public ButtonActions action;
        [FoldoutGroup("Button Variables")]
        public List<GameObject> activateObjects;
        [FoldoutGroup("Button Variables")]
        public List<GameObject> deactivateObjects;
        [FoldoutGroup("Button Variables")]
        public string argument;
    }

    // Buttons
    public List<MenuButton> _buttons;
    private Dictionary<string, MenuButton> buttons;


    // Start method
    public void Start()
    {
        // Setup dictionary
        buttons = new Dictionary<string, MenuButton>();
        foreach(MenuButton button in _buttons)
            buttons.Add(button.buttonName, button);
        _buttons = new List<MenuButton>();
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
                case ButtonActions.ActivateDeactivate:
                    foreach(GameObject obj in button.activateObjects) obj.SetActive(true);
                    foreach (GameObject obj in button.deactivateObjects) obj.SetActive(false);
                    break;
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
