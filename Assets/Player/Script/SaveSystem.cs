using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveSystem
{
    public static void SavePlayerData(PlayerStatSO playerData)
    {
        string path = Application.persistentDataPath + Path.DirectorySeparatorChar + "player.json";

        string json = JsonUtility.ToJson(playerData, true);
        File.WriteAllText(path, json);
    }

    public static bool LoadPlayerData(PlayerStatSO playerStat)
    {
        string path = Application.persistentDataPath + "/player.json";
        if (File.Exists(path))
        {
            JsonUtility.FromJsonOverwrite(File.ReadAllText(path), playerStat);
            return true;
        }
        else
        {
            Debug.Log("Save file not found");
            return false;
        }
    }

    public static void DeleteExistingSave()
    {
        string path = Application.persistentDataPath + "/player.json";
        if (File.Exists(path))
            File.Delete(path);
    }
}