using HeathenEngineering.SteamworksIntegration;
using Michsky.UI.ModernUIPack;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class JoinButton : MonoBehaviour
{
    public ButtonManager button;
    public List<Image> images;
    [SerializeField] public List<TextMeshProUGUI> sessions;
    private UserData userData;
    public string clientOf;

    public void SetUserData(UserData userData)
    {
        // Set userData
        this.userData = userData;

        // Set avatar
        userData.LoadAvatar((image) =>
        {
            foreach (Image img in images)
                img.sprite = Sprite.Create(image, new Rect(0, 0, 184, 184), new Vector2(0.5f, 0.5f), 100);
        });

        // Set name
        button.buttonText = userData.Name;
    }

    public void UpdateUserData()
    {
        // Set colors based on State
        if (clientOf != "")
            foreach (TextMeshProUGUI text in sessions)
                text.text = "In Game";

        else if (userData.GameInfo.m_gameID.AppID().Equals(SteamSettings.ApplicationId))
            foreach (TextMeshProUGUI text in sessions)
                text.text = "In Menus";

        else if (userData.State.HasFlag(Steamworks.EPersonaState.k_EPersonaStateOnline))
            foreach (TextMeshProUGUI text in sessions)
                text.text = "Online";

        else if (userData.State.HasFlag(Steamworks.EPersonaState.k_EPersonaStateAway))
            foreach (TextMeshProUGUI text in sessions)
                text.text = "Away";

        else
            foreach (TextMeshProUGUI text in sessions)
                text.text = "Offline";

        button.UpdateUI();
    }

    public void ConnectToUser()
    {
        if (userData.GameInfo.m_gameID.AppID().Equals(SteamSettings.ApplicationId))
        {
            print(clientOf);
            NetworkManagerSF.active.Join(clientOf);
        }
        else
        {
            Debug.Log($"{userData.Name} is not in a game!");
        }
    }
}
