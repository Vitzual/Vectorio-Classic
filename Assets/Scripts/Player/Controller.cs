using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    [HideInInspector]
    public bool SettingHotbar = false;

    // Checks if for numeric input
    public void CheckNumberInput()
    {
        if (Input.GetKeyDown(Keybinds.hotbar_1))
            Events.active.NumberInput(1, SettingHotbar);
        else if (Input.GetKeyDown(Keybinds.hotbar_2))
            Events.active.NumberInput(2, SettingHotbar);
        else if (Input.GetKeyDown(Keybinds.hotbar_3))
            Events.active.NumberInput(3, SettingHotbar);
        else if (Input.GetKeyDown(Keybinds.hotbar_4))
            Events.active.NumberInput(4, SettingHotbar);
        else if (Input.GetKeyDown(Keybinds.hotbar_5))
            Events.active.NumberInput(5, SettingHotbar);
        else if (Input.GetKeyDown(Keybinds.hotbar_6))
            Events.active.NumberInput(6, SettingHotbar);
        else if (Input.GetKeyDown(Keybinds.hotbar_7))
            Events.active.NumberInput(7, SettingHotbar);
        else if (Input.GetKeyDown(Keybinds.hotbar_8))
            Events.active.NumberInput(8, SettingHotbar);
        else if (Input.GetKeyDown(Keybinds.hotbar_9))
            Events.active.NumberInput(9, SettingHotbar);
    }
}
