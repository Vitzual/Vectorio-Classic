using Michsky.UI.ModernUIPack;
using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using HeathenEngineering.SteamworksIntegration;

public class PlayerList : MonoBehaviour
{
    // Network Manager
    [SerializeField] private NetworkManager networkManager;

    // Host Options
    [SerializeField] private SliderManager maxPlayersSlider;

    // Join Options
    [SerializeField] private JoinButton joinButton;
    [SerializeField] private Transform joinList;

    public void UpdateMaxPlayers()
    {
        maxPlayersSlider.GetComponentInChildren<TextMeshProUGUI>().text = $"Max Friends ({(int)maxPlayersSlider.mainSlider.value})";
    }

    public void UpdateFriendsList()
    {
        //List<UserData> friends = SortFriendsList(Steamworks.SteamFriends);

        //foreach (UserData friend in friends)
        //{
            JoinButton button = Instantiate(joinButton.gameObject, Vector3.zero, Quaternion.identity).GetComponent<JoinButton>();
        //    button.SetUserData(friend);
            button.UpdateUserData();
            button.GetComponent<RectTransform>().localScale = new Vector3(.8f, .8f, .8f);
            button.transform.SetParent(joinList);
        //}
    }

    private List<UserData> SortFriendsList(List<UserData> friends)
    {
        List<UserData> unsorted = friends;
        List<UserData> sorted = new List<UserData>();

        // Put list in alphabetical order
        //unsorted = unsorted.OrderBy(friend => friend.DisplayName).ToList();

        // Get friends currently in-game
        for (int i = 0; i < unsorted.Count; i++)
        {
            if (unsorted[i].GameInfo.m_gameID.AppID().Equals(SteamSettings.ApplicationId))
            {
                sorted.Add(unsorted[i]);
                unsorted.Remove(unsorted[i]);
                i--;
            }
        }

        // Get online friends
        for (int i = 0; i < unsorted.Count; i++)
        {
            if (unsorted[i].State.HasFlag(Steamworks.EPersonaState.k_EPersonaStateOnline))
            {
                sorted.Add(unsorted[i]);
                unsorted.Remove(unsorted[i]);
                i--;
            }
        }

        // Get away friends
        for (int i = 0; i < unsorted.Count; i++)
        {
            if (unsorted[i].State.HasFlag(Steamworks.EPersonaState.k_EPersonaStateAway))
            {
                sorted.Add(unsorted[i]);
                unsorted.Remove(unsorted[i]);
                i--;
            }
        }

        // Add remaining friends
        sorted.AddRange(unsorted);

        return sorted;
    }
}
