using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class StaticData
{
    public static string fileSavePath = Application.persistentDataPath + "save1.dat";
    public static string fileSettingsPath = Application.persistentDataPath + "settings.dat";

    public static DataStruct data;

    public static SettingsStruct settings;

    public static void CompleteLevel(int level, int score)
    {
        if (data.levelsScore[level] < score)
        {
            data.levelsScore[level] = score;
            SaveData();
        }
    }

    public static void LoadData()
    {
        FileStream file = null;
        try
        {
             file = new FileStream(fileSavePath, FileMode.Open);
        }
        catch (FileNotFoundException)
        {
            file?.Close();
            CreateSaveFile();
            file = new FileStream(fileSavePath, FileMode.Open);
        }
        
        BinaryFormatter bf = new BinaryFormatter();
        data = (DataStruct)bf.Deserialize(file);
        file.Close();
    }

    public static void SaveData()
    {
        FileStream file = new FileStream(fileSavePath, FileMode.OpenOrCreate);
        BinaryFormatter bf = new BinaryFormatter();
        bf.Serialize(file, data);
        file.Close();
    }

    public static void CreateSaveFile()
    {
        FileStream file = new FileStream(fileSavePath, FileMode.CreateNew);
        BinaryFormatter bf = new BinaryFormatter();
        bf.Serialize(file, new DataStruct() { isTutorialComplete=false, levelsScore = new int[] { 0, 0, 0, 0, 0, 0 } });
        file.Close();
    }

    public static void DeleteSaveFile()
    {
        File.Delete(fileSavePath);
    }

    public static void LoadSettings()
    {
        FileStream file = null;
        try
        {
            file = new FileStream(fileSettingsPath, FileMode.Open);
        }
        catch (FileNotFoundException)
        {
            file?.Close();
            CreateSettingsFile();
            file = new FileStream(fileSettingsPath, FileMode.Open);
        }

        BinaryFormatter bf = new BinaryFormatter();
        settings = (SettingsStruct)bf.Deserialize(file);
        file.Close();
    }

    public static void SaveSettings()
    {
        FileStream file = new FileStream(fileSettingsPath, FileMode.OpenOrCreate);
        BinaryFormatter bf = new BinaryFormatter();
        bf.Serialize(file, settings);
        file.Close();
    }

    public static void CreateSettingsFile()
    {
        FileStream file = new FileStream(fileSettingsPath, FileMode.CreateNew);
        BinaryFormatter bf = new BinaryFormatter();
        bf.Serialize(file, new SettingsStruct()
        {
            globalVolume = 0.75f,
            musicVolume = 0.75f,
            soundVolume = 0.75f,
            inputMode = SettingsStruct.InputMode.SingleFinger,
            camMoveSpeed = 0.18f,
            camZoomSpeed = 0.18f
        });
        file.Close();
    }

    public static void DeleteSettingsFile()
    {
        File.Delete(fileSettingsPath);
    }
}

[System.Serializable]
public struct DataStruct
{
    public bool isTutorialComplete;

    public int[] levelsScore;
}

[System.Serializable]
public struct SettingsStruct
{
    public InputMode inputMode;

    public float globalVolume;
    public float musicVolume;
    public float soundVolume;

    public enum InputMode
    {
        SingleFinger,
        TwoFingers
    }

    public float camMoveSpeed;
    public float camZoomSpeed;
}
