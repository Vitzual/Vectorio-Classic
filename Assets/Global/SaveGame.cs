using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{

    private static string SaveLocation;

    public static bool CheckForSave(int a)
    {
        return File.Exists(Application.persistentDataPath + "/save_" + a+".vectorio");
    }

    public static string[] GetSaveStrings(int a)
    {
        // Loads the save data and returns the name
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/save_" + a+".vectorio";
        FileStream stream = new FileStream(path, FileMode.Open);
        SaveData data = formatter.Deserialize(stream) as SaveData;
        stream.Close();
        
        // Return string data
        string[] saveInfo = new string[4];
        if(data.name != null) 
        { 
            saveInfo[0] = data.name;
            saveInfo[1] = data.mode;
            saveInfo[2] = data.heatt.ToString();
            saveInfo[3] = data.time.ToString();
        } 
        else
        {
            saveInfo[0] = "SAVE " + a;
            saveInfo[1] = "OLD SAVE";
            saveInfo[2] = "???";
            saveInfo[3] = "0:00";
        }

        Debug.Log(saveInfo[0] + " " + saveInfo[1]);
        return saveInfo;
    }

    public static void DeleteGame(int a)
    {
        if (File.Exists(Application.persistentDataPath + "/save_" + a + ".vectorio"))
        {
            File.Delete(Application.persistentDataPath + "/save_" + a + ".vectorio");
        }
    }

    public static void SaveGame (Survival data_1, Technology data_2, WaveSpawner data_3, Research data_4, Difficulties data_5, int time = 0, int heatt = 0, int groupSpawnEvery = 300)
    {
        string SavePath = Application.persistentDataPath + "/location.save";
        BinaryFormatter formatter = new BinaryFormatter();

        if (File.Exists(SavePath))
        {
            FileStream a = new FileStream(SavePath, FileMode.Open);
            SaveLocation = formatter.Deserialize(a) as string;
            Debug.Log("Found save location: "+SaveLocation);
            a.Close();
        }
        else
        {
            Debug.Log("Save location could not be found, defaulting to save 1");
            SaveLocation = "/save_1.vectorio";
        }

        string path = Application.persistentDataPath + SaveLocation;
        FileStream stream = new FileStream(path, FileMode.Create);

        SaveData data = new SaveData(data_1, data_2, data_3, data_4, data_5, time, heatt, groupSpawnEvery);

        formatter.Serialize(stream, data);
        stream.Close();

        Debug.Log("Saved file to " + path);
    }

    public static SaveData LoadGame()
    {

        string SavePath = Application.persistentDataPath + "/location.save";
        BinaryFormatter formatter = new BinaryFormatter();

        if (File.Exists(SavePath))
        {
            FileStream a = new FileStream(SavePath, FileMode.Open);
            SaveLocation = formatter.Deserialize(a) as string;
            Debug.Log("Found save location: " + SaveLocation);
            a.Close();
        }
        else
        {
            Debug.Log("Save location could not be found, defaulting to save 1");
            SaveLocation = "/save_1.vectorio";
        }

        string path = Application.persistentDataPath + SaveLocation;
        if (File.Exists(path))
        {
            FileStream stream = new FileStream(path, FileMode.Open);

            SaveData data = formatter.Deserialize(stream) as SaveData;
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
        string path = Application.persistentDataPath + "/settings.save";
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(path, FileMode.Create);

        SettingsData data = new SettingsData(width, height, volume, sound, fullscreen, glowMode);

        formatter.Serialize(stream, data);
        stream.Close();

        Debug.Log("Saved file to " + path);
    }

    public static SettingsData LoadSettings()
    {
        string path = Application.persistentDataPath + "/settings.save";
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
