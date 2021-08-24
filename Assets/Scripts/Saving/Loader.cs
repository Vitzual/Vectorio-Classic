using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loader : MonoBehaviour
{





    public void LoadSurvivalSave()
    {
        // Load save data to file
        SurvivalData data = SaveSystem.LoadGame();

        // Check if data is null
        if (data != null)
        {
            try { Playtime = data.time; }
            catch { Debug.Log("Save file does not contain new time tracking data!"); Playtime = 0; }

            // Set power usage
            UI.PowerUsageBar.currentPercent = (float)PowerConsumption / (float)AvailablePower * 100;
            UI.AvailablePower.text = AvailablePower.ToString() + " MAX";

            // Attempt to load save data 
            //try
            //{
            // Force tech tree update
            try { tech.LoadSaveData(data.unlocked); }
            catch (Exception e) { Debug.Log("Save file does not contain new unlock data!\nStack: " + e); }

            // Generate world data
            GameObject.Find("OnSpawn").GetComponent<WorldGenerator>().GenerateWorldData(Difficulties.seed, true);

            // Get research save data
            rsrch.SetResearchData(data.researchTechs);

            // Attempt to place saved buildings
            float soundHolder = manager.GetComponent<Settings>().GetSound();
            manager.GetComponent<Settings>().SetSound(0);
            PlaceSavedBuildings(data);

            // Update bosses
            //try
            //{
            Spawner.updateBosses(data.bossesDefeated);
            //}
            //catch
            //{
            Debug.Log("Save file does not contain new Guardian data!");
            //}

            // Set power usage
            UI.PowerUsageBar.currentPercent = (float)PowerConsumption / (float)AvailablePower * 100;
            UI.AvailablePower.text = AvailablePower.ToString() + " MAX";

            // Place saved enemies 
            //try
            //{
            PlaceSavedEnemies(data.enemies);
            //}
            //catch
            //{
            Debug.Log("Save file does not contain new enemy data!");
            //}

            // Update hotbar with saved ID's
            //try
            //{
            SetHotbarData(data.hotbar);
            //}
            //catch
            //{
            Debug.Log("Save file does not contain new hotbar data!");
            //}

            manager.GetComponent<Settings>().SetSound(soundHolder);
            //}
            //catch (System.Exception e)
            //{
            Spawner.loadingSave = false;
            //Debug.Log("The save data found was corrupt.\n\nStacktrace: " + e.Message + "\n" + e.StackTrace);
            GameObject.Find("OnSpawn").GetComponent<WorldGenerator>().GenerateWorldData(Difficulties.seed, false);
            //}
        }
    }
}
