using HeathenEngineering.SteamworksIntegration;
using HeathenEngineering.SteamworksIntegration.API;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SteamList : MonoBehaviour
{
    private Dictionary<UserData, JoinButton> activeButtons = new Dictionary<UserData, JoinButton>();
    private List<UserData> friendList = new List<UserData>();
    public JoinButton joinButton;
    public Transform joinList;
    public bool generated = false;

    public void UpdateFriendsList()
    {
        if (!generated)
        {
            friendList = SortFriendsList(Friends.Client.GetFriends(Steamworks.EFriendFlags.k_EFriendFlagAll).ToList());

            foreach (UserData friend in friendList)
            {
                JoinButton button = Instantiate(joinButton.gameObject, Vector3.zero, Quaternion.identity).GetComponent<JoinButton>();
                button.SetUserData(friend);
                button.UpdateUserData();
                button.transform.SetParent(joinList);
                button.GetComponent<RectTransform>().localScale = new Vector3(1f, 1f, 1f);
                activeButtons.Add(friend, button);
            }
            generated = true;
        }
        else
        {
            // Re-sort friends list and organize
            friendList = SortFriendsList(friendList);
            for (int i = 0; i < friendList.Count; i++)
            {
                if (activeButtons.ContainsKey(friendList[i]))
                {
                    activeButtons[friendList[i]].UpdateUserData();
                    activeButtons[friendList[i]].button.transform.SetSiblingIndex(i);
                }
            }
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
