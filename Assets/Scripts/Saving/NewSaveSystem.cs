using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class NewSaveSystem : MonoBehaviour
{
    // Save a game
    public static void SaveGame(string name)
    {
        // Create new save data instance
        SaveData saveData = new SaveData();

        // Get all active entities in scene
        BaseEntity[] activeEntities = FindObjectsOfType<BaseEntity>();

        // Entity lists
        List<SaveData.BuildingData> buildings = new List<SaveData.BuildingData>();
        List<SaveData.EnemyData> enemies = new List<SaveData.EnemyData>();

        // Generate all unlocks
        List<string> unlocks = new List<string>();
        foreach (KeyValuePair<Entity, Buildable> buildable in Buildables.active)
            if (buildable.Value.isUnlocked) unlocks.Add(buildable.Value.building.InternalID);
        saveData.unlocked = unlocks.ToArray();

        // Loop through entities and assign to corresponding list
        for (int i = 0; i < activeEntities.Length; i++)
        {
            // Check if tile should be saved
            BaseTile tile = activeEntities[i].GetComponent<BaseTile>();

            if (tile != null)
            {
                // Check if tile should be saved
                if (!tile.saveBuilding) continue;

                // Building data struct
                SaveData.BuildingData buildingData = new SaveData.BuildingData();

                buildingData.id = tile.buildable.building.InternalID;
                buildingData.xCoord = (int)tile.transform.position.x;
                buildingData.yCoord = (int)tile.transform.position.y;
                buildingData.health = tile.health;
                buildingData.metadata = new int[1];
                buildingData.metadata[0] = tile.metadata;
                buildingData.blueprintIDs = new string[0];
                buildingData.ghostBuilding = tile.GetComponent<GhostTile>() != null;

                buildings.Add(buildingData);
            }
            else
            {
                // Enemy data struct
                DefaultEnemy enemy = activeEntities[i].GetComponent<DefaultEnemy>();
                if (enemy != null)
                {
                    // Enemy data struct
                    SaveData.EnemyData enemyData = new SaveData.EnemyData();

                    enemyData.id = enemy.enemy.InternalID;
                    enemyData.xCoord = (int)enemy.transform.position.x;
                    enemyData.yCoord = (int)enemy.transform.position.y;
                    enemyData.health = enemy.health;
                    enemyData.metadata = new int[1];
                    enemyData.metadata[0] = enemy.metadata;
                    enemyData.variantID = enemy.variant.InternalID;

                    enemies.Add(enemyData);
                }
            }
        }

        // Set difficulty
        saveData.difficultyData = Gamemode.difficulty;

        // Set lists
        saveData.buildings = buildings.ToArray();
        saveData.enemies = enemies.ToArray();

        // Set the rest of the data
        saveData.stage = Gamemode.stage.InternalID;
        saveData.gold = Resource.active.GetAmount(Resource.CurrencyType.Gold);
        saveData.essence = Resource.active.GetAmount(Resource.CurrencyType.Essence);
        saveData.iridium = Resource.active.GetAmount(Resource.CurrencyType.Iridium);

        // Set string variables
        saveData.worldName = Gamemode.saveName;
        saveData.worldMode = Gamemode.active.name;
        saveData.worldSeed = Gamemode.seed;
        saveData.worldVersion = Gamemode.active.version;
        saveData.worldPlaytime = Gamemode.time;
        saveData.worldCompletion = Resource.active.GetAmount(Resource.CurrencyType.Heat) / 1000000;

        // Convert to json and save
        string data = JsonUtility.ToJson(saveData);
        File.WriteAllText(Application.persistentDataPath + name, data);
    }

    // Load a game 
    public static void LoadGame(SaveData saveData)
    {
        // Set string variables
        Gamemode.saveName = saveData.worldName;
        Gamemode.seed = saveData.worldSeed;
        Gamemode.time = saveData.worldPlaytime;

        // Get active border stage
        if (ScriptableLoader.stages.ContainsKey(saveData.stage))
            Gamemode.stage = ScriptableLoader.stages[saveData.stage];

        // Generate world data
        #pragma warning disable CS0612 
        WorldGenerator.active.GenerateWorldData(Gamemode.seed);
        #pragma warning restore CS0612

        // Generate all unlocks
        if (saveData.unlocked != null) 
        {
            foreach (string unlock in saveData.unlocked)
            {
                // Get buildable
                Buildable buildable = Buildables.RequestBuildable(ScriptableLoader.buildings[unlock]);
                Buildables.UnlockBuildable(buildable);
            }
        }

        // Generate all research
        if (saveData.research != null)
            ResearchUI.active.Setup(saveData.research.ToList());
        else ResearchUI.active.Setup();

        // Apply data
        foreach (SaveData.BuildingData buildingData in saveData.buildings)
        {
            if (ScriptableLoader.buildings.ContainsKey(buildingData.id))
            {
                // Get buildable
                Buildable buildable = Buildables.RequestBuildable(ScriptableLoader.buildings[buildingData.id]);

                // Set metadata and create building / ghost tile
                InstantiationHandler.active.metadata = buildingData.metadata[0];

                // Create ghost building if data exists
                #pragma warning disable CS0472 
                if (buildingData.ghostBuilding != null && buildingData.ghostBuilding) InstantiationHandler.active.RpcInstatiateGhost(buildable, 
                    new Vector2(buildingData.xCoord, buildingData.yCoord), Quaternion.identity);
                #pragma warning restore CS0472

                // If data doesn't exist or is not a ghost building, create it
                else
                {
                    InstantiationHandler.active.RpcInstantiateBuilding(buildable, new Vector2(buildingData.xCoord, buildingData.yCoord), 
                        Quaternion.identity, true, buildingData.health);
                }

                // Reset metadat
                InstantiationHandler.active.metadata = -1;
            }
            else Debug.Log("Building with ID " + buildingData.id + " could not be found!");
        }

        // Apply enemies
        foreach (SaveData.EnemyData enemyData in saveData.enemies)
        {
            if (ScriptableLoader.enemies.ContainsKey(enemyData.id) && ScriptableLoader.variants.ContainsKey(enemyData.variantID))
            {
                Enemy enemy = ScriptableLoader.enemies[enemyData.id];
                Variant variant = ScriptableLoader.variants[enemyData.variantID];
                EnemyHandler.active.CreateEntity(enemy, variant, new Vector2(enemyData.xCoord, enemyData.yCoord), Quaternion.identity, enemyData.health);
            }
            else Debug.Log("Enemy with ID " + enemyData.id + " and variant ID " + enemyData.variantID + "could not be found!");
        }

        // Check resources
        Resource.active.Add(Resource.CurrencyType.Gold, saveData.gold, true);
        Resource.active.Add(Resource.CurrencyType.Essence, saveData.essence, true);
        Resource.active.Add(Resource.CurrencyType.Iridium, saveData.iridium, true);
    }

    // Delete save file
    public static void DeleteGame(string path)
    {
        if (File.Exists(path)) 
            File.Delete(path);
        else Debug.Log("The file " + path + " does not exist!");
    }
}
