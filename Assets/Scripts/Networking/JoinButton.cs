using HeathenEngineering.SteamworksIntegration;
using Michsky.UI.ModernUIPack;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class JoinButton : MonoBehaviour
{
    public ButtonManagerBasic button;
    public Image image;
    [SerializeField] public TextMeshProUGUI session;
    private UserData userData;
    public string clientOf;

    public void SetUserData(UserData userData)
    {
        this.userData = userData;
    }

    public void UpdateUserData()
    {
        button.buttonText = userData.Name;
        //image.sprite = Sprite.Create(userData.avatar, image.sprite.rect, Vector2.zero);

        // Set colors based on State
        if (clientOf != "")
        {
            session.text = "In Game";
        }
        else if (userData.GameInfo.m_gameID.AppID().Equals(SteamSettings.ApplicationId))
        {
            session.text = "In Menus";
        }
        else if (userData.State.HasFlag(Steamworks.EPersonaState.k_EPersonaStateOnline))
        {
            session.text = "Online";
        }
        else if (userData.State.HasFlag(Steamworks.EPersonaState.k_EPersonaStateAway))
        {
            session.text = "Away";
        }
        else
        {
            session.text = "Offline";
        }

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
