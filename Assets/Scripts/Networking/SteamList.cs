using HeathenEngineering.SteamworksIntegration;
using HeathenEngineering.SteamworksIntegration.API;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SteamList : MonoBehaviour
{
    public List<Transform> activeButtons = new List<Transform>();
    public JoinButton joinButton;
    public Transform joinList;

    public void UpdateFriendsList()
    {
        // Reset old list
        if (activeButtons != null)
            foreach (Transform obj in activeButtons)
                Recycler.AddRecyclable(obj);
        activeButtons = new List<Transform>();

        List<UserData> friends = SortFriendsList(Friends.Client.GetFriends(Steamworks.EFriendFlags.k_EFriendFlagAll).ToList());

        foreach (UserData friend in friends)
        {
            JoinButton button = Instantiate(joinButton.gameObject, Vector3.zero, Quaternion.identity).GetComponent<JoinButton>();
            button.SetUserData(friend);
            button.UpdateUserData();
            button.transform.SetParent(joinList);
            button.GetComponent<RectTransform>().localScale = new Vector3(1f, 1f, 1f);
            activeButtons.Add(button.transform);
        }
    }

    private List<UserData> SortFriendsList(List<UserData> friends)
    {
        List<UserData> unsorted = friends;
        List<UserData> sorted = new List<UserData>();

        // Put list in alphabetical order
        unsorted = unsorted.OrderBy(friend => friend.Name).ToList();

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
