using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class NewSaveSystem : MonoBehaviour
{
    private static string SaveLocation;

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

        // Loop through entities and assign to corresponding list
        for(int i = 0; i < activeEntities.Length; i++)
        {
            BaseTile tile = activeEntities[i].GetComponent<BaseTile>();
            if (tile != null)
            {
                // Building data struct
                SaveData.BuildingData buildingData = new SaveData.BuildingData();

                buildingData.id = tile.buildable.building.InternalID;
                buildingData.coords = new System.Tuple<int, int>((int)tile.transform.position.x, (int)tile.transform.position.y);
                buildingData.health = tile.health;
                buildingData.metadata = new int[1];
                buildingData.metadata[0] = tile.metadata;
                buildingData.blueprintIDs = new string[0];

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
                    enemyData.coords = new System.Tuple<int, int>((int)enemy.transform.position.x, (int)enemy.transform.position.y);
                    enemyData.health = enemy.health;
                    enemyData.metadata = new int[0];
                    enemyData.variantID = enemy.variant.InternalID;

                    enemies.Add(enemyData);
                }
            }
        }

        // Set the rest of the data
        saveData.stage = Border.active.borderStage;
        saveData.resources = new Dictionary<Resource.CurrencyType, int>();
        foreach (KeyValuePair<Resource.CurrencyType, Resource.Currency> currency in Resource.active.currencies)
            if (currency.Key != Resource.CurrencyType.Heat && currency.Key != Resource.CurrencyType.Power)
                saveData.resources.Add(currency.Key, currency.Value.amount);

        // Set string variables
        saveData.worldName = Gamemode.worldName;
        saveData.worldMode = Gamemode.active.name + " (" + Resource.active.GetAmount(Resource.CurrencyType.Heat) / 1000000 * 100 + "%)";
        saveData.worldSeed = Gamemode.seed;
        saveData.worldVersion = Gamemode.active.version;
        saveData.worldPlaytime = Gamemode.time;

        // Convert to json and save
        string data = JsonUtility.ToJson(saveData);
        File.WriteAllText(Application.persistentDataPath + name, data);
    }

    // Load a game 
    public static void LoadGame(string path)
    {
        // Set use resources
        bool useResources = Gamemode.active.useResources;
        Gamemode.active.useResources = false;

        // Try loop
        try
        {
            // Load json file
            string data = File.ReadAllText(Application.persistentDataPath + path);
            SaveData saveData = JsonUtility.FromJson<SaveData>(data);

            // Set normal data
            Border.active.borderStage = saveData.stage;
            foreach (KeyValuePair<Resource.CurrencyType, int> currency in saveData.resources)
                Resource.active.Add(currency.Key, currency.Value);

            // Set string variables
            Gamemode.worldName = saveData.worldName;
            Gamemode.seed = saveData.worldSeed;
            Gamemode.time = saveData.worldPlaytime;

            // Generate world data
            #pragma warning disable CS0612 
            WorldGenerator.active.GenerateWorldData(Gamemode.seed);
            #pragma warning restore CS0612 

            // Apply data
            foreach (SaveData.BuildingData buildingData in saveData.buildings)
            {
                if (ScriptableLoader.buildings.ContainsKey(buildingData.id))
                {
                    Buildable buildable = Buildables.RequestBuildable(ScriptableLoader.buildings[buildingData.id]);
                    if (buildingData.metadata.Length > 0) InstantiationHandler.active.metadata = buildingData.metadata[0];
                    InstantiationHandler.active.RpcInstantiateBuilding(buildable, new Vector2(buildingData.coords.Item1, buildingData.coords.Item2),
                        Quaternion.identity, buildingData.health);
                }
                else Debug.Log("Building with ID " + buildingData.id + " could not be found!");
            }

            // Apply enemies
            foreach (SaveData.EnemyData enemyData in saveData.enemies)
            {
                if (ScriptableLoader.enemies.ContainsKey(enemyData.id))
                {
                    Enemy enemy = ScriptableLoader.enemies[enemyData.id];
                    EnemyHandler.active.CreateEntity(enemy, new Vector2(enemyData.coords.Item1, enemyData.coords.Item2), Quaternion.identity, enemyData.health);
                }
                else Debug.Log("Enemy with ID " + enemyData.id + " could not be found!");
            }
        }
        catch (Exception e) { Debug.Log("Save system ran into a critical error!\nError: " + e); }

        // Set back to 
        Gamemode.active.useResources = useResources;
    }



    // Delete save file
    public static void DeleteGame(int a)
    {
        if (File.Exists(Application.persistentDataPath + "/world_" + a + ".save"))
            File.Delete(Application.persistentDataPath + "/world_" + a + ".save");
    }
}
