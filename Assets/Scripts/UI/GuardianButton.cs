using Michsky.UI.ModernUIPack;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardianButton : MonoBehaviour
{
    // Active instance
    public static GuardianButton active;

    // Confirm screen
    public ModalWindowManager confirmScreen;
    public CanvasGroup canvasGroup;

    // Setup events
    public void Start()
    {
        active = this;
    }

    // Set confirm screen thing
    public void SetConfirmScreen(Guardian guardian)
    {
        confirmScreen.descriptionText = "In order to progress to the next stage, you must defeat <b>" + guardian.name + "</b> " +
            "guardian. The guardian will come from the <b>" + guardian.directionName + "</b>, so prepare your defenses!";
        confirmScreen.icon = Sprites.GetSprite(guardian.name);
        confirmScreen.UpdateUI();
    }

    // Open confirm screen
    public void OpenConfirmScreen()
    {
        confirmScreen.gameObject.SetActive(true);
        confirmScreen.OpenWindow();
    }

    // Close confirm screen
    public void CloseConfirmScreen()
    {
        confirmScreen.CloseWindow();
    }

    // Show button confirmation
    public void ShowButton(Guardian guardian)
    {
        SetConfirmScreen(guardian);
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
        canvasGroup.interactable = true;
    }

    // Hide button confirmation
    public void HideButton()
    {
        canvasGroup.alpha = 0f;
        canvasGroup.blocksRaycasts = false;
        canvasGroup.interactable = false;
    }

    // Begin battle
    public void StartBattle()
    {
        // Save game
        Gamemode.active.SaveGame();

        // Spawn guardian
        CloseConfirmScreen();
        Events.active.StartGuardianBattle();
        gameObject.SetActive(false);
    }
}
