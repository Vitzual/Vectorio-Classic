using HeathenEngineering.SteamworksIntegration;
using Michsky.UI.ModernUIPack;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class JoinButton : MonoBehaviour
{
    // Button state colors class
    [System.Serializable]
    public class StateColors
    {
        public Color borderDefault;
        public Color backgroundDefault;
        public Color borderHighlighted;
        public Color backgroundHighlighted;
    }

    // Buttons colors
    public StateColors inGame;
    public StateColors online;
    public StateColors away;
    public StateColors offline;

    // Button variables
    public ButtonManager button;
    public List<Image> images;
    [SerializeField] public List<TextMeshProUGUI> sessions;
    public List<Image> buttonModels;
    private UserData userData;
    public string clientOf;

    public void SetUserData(UserData userData)
    {
        // Set userData
        this.userData = userData;
        clientOf = userData.cSteamId.ToString();

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
        // Check clientOf
        clientOf = userData.SteamId.ToString();

        // Set colors based on State
        if (userData.GameInfo.m_gameID.AppID().Equals(SteamSettings.ApplicationId))
            foreach (TextMeshProUGUI text in sessions)
            {
                text.text = "In Game";
                buttonModels[0].color = inGame.borderDefault;
                buttonModels[1].color = inGame.backgroundDefault;
                buttonModels[2].color = inGame.borderHighlighted;
                buttonModels[3].color = inGame.backgroundHighlighted;
            }

        else if (userData.State.HasFlag(Steamworks.EPersonaState.k_EPersonaStateOnline))
            foreach (TextMeshProUGUI text in sessions)
            {
                text.text = "Online";
                buttonModels[0].color = online.borderDefault;
                buttonModels[1].color = online.backgroundDefault;
                buttonModels[2].color = online.borderHighlighted;
                buttonModels[3].color = online.backgroundHighlighted;
            }

        else if (userData.State.HasFlag(Steamworks.EPersonaState.k_EPersonaStateAway) ||
                 userData.State.HasFlag(Steamworks.EPersonaState.k_EPersonaStateSnooze) ||
                 userData.State.HasFlag(Steamworks.EPersonaState.k_EPersonaStateBusy))
            foreach (TextMeshProUGUI text in sessions)
            {
                text.text = "Away";
                buttonModels[0].color = away.borderDefault;
                buttonModels[1].color = away.backgroundDefault;
                buttonModels[2].color = away.borderHighlighted;
                buttonModels[3].color = away.backgroundHighlighted;
            }

        else
            foreach (TextMeshProUGUI text in sessions)
            {
                text.text = "Offline";
                buttonModels[0].color = offline.borderDefault;
                buttonModels[1].color = offline.backgroundDefault;
                buttonModels[2].color = offline.borderHighlighted;
                buttonModels[3].color = offline.backgroundHighlighted;
            }

        button.UpdateUI();
    }

    public void ConnectToUser()
    {
        if (userData.GameInfo.m_gameID.AppID().Equals(SteamSettings.ApplicationId))
        {
            print("[SERVER] Attempting connection to client with ID " + clientOf);
            NetworkManagerSF.active.Join(clientOf);
        }
        else
        {
            Debug.Log($"{userData.Name} is not in a game!");
        }
    }
}
