using Michsky.UI.ModernUIPack;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardianButton : MonoBehaviour
{
    // Confirm screen
    public ModalWindowManager confirmScreen;

    // Set confirm screen thing
    public void SetConfirmScreen(Guardian guardian)
    {
        confirmScreen.gameObject.SetActive(true);
        confirmScreen.CloseWindow();
        confirmScreen.descriptionText = "In order to progress to the next stage, you must defeat <b>" + guardian.name + "</b> " +
            "guardian. The guardian will come from the <b>" + guardian.directionName + "</b>, so prepare your defenses!";
        confirmScreen.icon = Sprites.GetSprite(guardian.name);
    }

    // Open confirm screen
    public void ToggleConfirmScreen()
    {
        if (confirmScreen.GetComponent<CanvasGroup>().alpha > 0f) 
            confirmScreen.CloseWindow();
        else confirmScreen.OpenWindow();
    }

    // Begin battle
    public void StartBattle()
    {
        confirmScreen.gameObject.SetActive(false);
        Events.active.FireHubLaser();
        gameObject.SetActive(false);
    }
}
