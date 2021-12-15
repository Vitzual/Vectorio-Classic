using HeathenEngineering.SteamworksIntegration;
using HeathenEngineering.SteamworksIntegration.API;
using System;
using System.Collections.Generic;
using UnityEngine;

// Script by Ben Nichols
//
// Place in onboarding scene to load all achievements via
// heathens engineering. Can then be referenced anywhere
// in the game whenever you wish!

public class Achievements : MonoBehaviour
{
    // Static instance
    public static Achievements active;
    public List<AchievementObject> _achievements;
    public static Dictionary<string, AchievementObject> achievements;
    public static bool isSetup = false;

    // Generate dictionary at runtime
    public void Start()
    {
        if (!isSetup)
        {
            achievements = new Dictionary<string, AchievementObject>();
            foreach (AchievementObject achievement in _achievements)
                achievements.Add(achievement.achievementId, achievement);
            _achievements = new List<AchievementObject>();
            isSetup = true;
        }
        else Debug.Log("[STEAM] Achievements are already setup!");
    }

    // Unlock achievement method
    public static void Unlock(string id)
    {
        try
        {
            if (achievements.ContainsKey(id))
            {
                Debug.Log("[STEAM] Received achievement request with ID " + id);

                UserData userData = User.Client.Id;
                if (!achievements[id].IsAchieved) achievements[id].Unlock();
                else Debug.Log("[STEAM] User already has achievement " + id);
            }
            else Debug.Log("[STEAM] No achievement found with ID " + id);
        }
        catch (Exception e)
        {
            Debug.Log("[STEAM] Achievement could not be added.");
            Debug.Log("[STEAM] Error message: " + e.Message);
            Debug.Log("[STEAM] Stack trace: " + e.StackTrace);
        }
    }
}
