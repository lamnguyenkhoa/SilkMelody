using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveSystem
{
    public static void SavePlayerData()
    {
        string path = Application.persistentDataPath + Path.DirectorySeparatorChar + "player.json";
        string json = JsonUtility.ToJson(GameMaster.instance.playerData, true);
        File.WriteAllText(path, json);

        path = Application.persistentDataPath + Path.DirectorySeparatorChar + "world.json";
        json = JsonUtility.ToJson(GameMaster.instance.worldData, true);
        File.WriteAllText(path, json);
    }

    public static void LoadPlayerData()
    {
        string path = Application.persistentDataPath + "/player.json";
        if (File.Exists(path))
        {
            JsonUtility.FromJsonOverwrite(File.ReadAllText(path), GameMaster.instance.playerData);
        }
        else
        {
            Debug.Log("Player save file not found");
        }

        path = Application.persistentDataPath + "/world.json";
        if (File.Exists(path))
        {
            JsonUtility.FromJsonOverwrite(File.ReadAllText(path), GameMaster.instance.worldData);
        }
        else
        {
            Debug.Log("World save file not found");
        }
    }

    public static void DeleteExistingSave()
    {
        string path = Application.persistentDataPath + "/player.json";
        if (File.Exists(path))
            File.Delete(path);

        path = Application.persistentDataPath + "/world.json";
        if (File.Exists(path))
            File.Delete(path);
    }

    public static bool CheckSaveExist()
    {
        string playerPath = Application.persistentDataPath + "/player.json";
        string worldPath = Application.persistentDataPath + "/world.json";

        if (File.Exists(playerPath) && File.Exists(worldPath))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}