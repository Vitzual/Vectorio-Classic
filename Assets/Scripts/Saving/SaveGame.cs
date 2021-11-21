using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{

    private static string SaveLocation;

    public static bool CheckForSave(int a)
    {
        return File.Exists(Application.persistentDataPath + "/world_" + a+".save");
    }

    public static string[] GetSaveStrings(int a)
    {
        // Loads the save data and returns the name
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/world_" + a+ ".save";
        FileStream stream = new FileStream(path, FileMode.Open);
        SurvivalData data = formatter.Deserialize(stream) as SurvivalData;
        stream.Close();
        
        // Return string data
        string[] saveInfo = new string[5];
        try {
            saveInfo[0] = data.worldName;
            saveInfo[1] = data.worldMode;
            saveInfo[2] = data.heat.ToString();
            saveInfo[3] = data.worldPlaytime.ToString();
            saveInfo[4] = data.worldVersion;
        } 
        catch
        {
            saveInfo[0] = "SAVE " + a;
            saveInfo[1] = "OLD SAVE";
            saveInfo[2] = "???";
            saveInfo[3] = "0:00";
            saveInfo[4] = "v0.3";
        }

        return saveInfo;
    }

    public static void DeleteGame(int a)
    {
        if (File.Exists(Application.persistentDataPath + "/world_" + a + ".save"))
        {
            File.Delete(Application.persistentDataPath + "/world_" + a + ".save");
        }
    }

    public static void SaveGame (/*Technology data_2,*/ EnemySpawner data_3, Research data_4, int time = 0)
    {
        string SavePath = Application.persistentDataPath + "/location.vectorio";
        BinaryFormatter formatter = new BinaryFormatter();

        if (File.Exists(SavePath))
        {
            FileStream a = new FileStream(SavePath, FileMode.Open);
            SaveLocation = formatter.Deserialize(a) as string;
            if (!SaveLocation.Contains("world"))
            {
                Debug.Log("Save location is invalid!");
                SaveLocation = "/world_1.save";
            }
            else Debug.Log("Found save location: " + SaveLocation);
            a.Close();
        }
        else
        {
            Debug.Log("Save location could not be found, defaulting to save 1");
            SaveLocation = "/world_1.save";
        }

        string path = Application.persistentDataPath + SaveLocation;
        FileStream stream = new FileStream(path, FileMode.Create);

        //SurvivalData data = new SurvivalData(data_2, data_3, data_4, time);

        //formatter.Serialize(stream, data);
        stream.Close();

        Debug.Log("Saved file to " + path);
    }

    public static SurvivalData LoadGame()
    {

        string SavePath = Application.persistentDataPath + "/location.vectorio";
        BinaryFormatter formatter = new BinaryFormatter();

        if (File.Exists(SavePath))
        {
            FileStream a = new FileStream(SavePath, FileMode.Open);
            SaveLocation = formatter.Deserialize(a) as string;
            if (!SaveLocation.Contains("world"))
            {
                Debug.Log("Save location is invalid!");
                SaveLocation = "/world_1.save";
            }
            else Debug.Log("Found save location: " + SaveLocation);
            a.Close();
        }
        else
        {
            Debug.Log("Save location could not be found, defaulting to save 1");
            SaveLocation = "/world_1.save";
        }

        string path = Application.persistentDataPath + SaveLocation;
        if (File.Exists(path))
        {
            FileStream stream = new FileStream(path, FileMode.Open);

            SurvivalData data = formatter.Deserialize(stream) as SurvivalData;
            stream.Close();

            Debug.Log("Loaded file from " + path);

            return data;
        }
        else
        {
            Debug.LogError("Save file not found in " + path);
            return null;
        }
    }

    public static void SaveSettings(int width, int height, float volume, float sound, bool fullscreen, int glowMode)
    {
        string path = Application.persistentDataPath + "/settings.vectorio";
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(path, FileMode.Create);

        SettingsData data = new SettingsData(width, height, volume, sound, fullscreen, glowMode);

        formatter.Serialize(stream, data);
        stream.Close();

        Debug.Log("Saved file to " + path);
    }

    public static SettingsData LoadSettings()
    {
        string path = Application.persistentDataPath + "/settings.vectorio";
        BinaryFormatter formatter = new BinaryFormatter();
        if (File.Exists(path))
        {
            FileStream stream = new FileStream(path, FileMode.Open);

            SettingsData data = formatter.Deserialize(stream) as SettingsData;
            stream.Close();

            Debug.Log("Loaded file from " + path);

            return data;
        }
        else
        {
            Debug.LogError("Save file not found in " + path);
            return null;
        }
    }

}
