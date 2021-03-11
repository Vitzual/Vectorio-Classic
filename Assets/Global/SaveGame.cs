using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{

    private static string SaveLocation;

    public static void SaveGame (Survival data_1, Technology data_2, WaveSpawner data_3, Research data_4)
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
            SaveLocation = "/save1.vectorio";
        }

        string path = Application.persistentDataPath + SaveLocation;
        FileStream stream = new FileStream(path, FileMode.Create);

        SaveData data = new SaveData(data_1, data_2, data_3, data_4);

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
            SaveLocation = "/save1.vectorio";
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

    public static void SaveSettings(int width, int height, float volume, bool fullscreen, int glowMode)
    {
        string path = Application.persistentDataPath + "/settings.save";
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(path, FileMode.Create);

        SettingsData data = new SettingsData(width, height, volume, fullscreen, glowMode);

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
