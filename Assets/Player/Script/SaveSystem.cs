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

    public static void LoadPlayerData(PlayerStatSO playerStat)
    {
        string path = Application.persistentDataPath + "/player.dat";
        if (File.Exists(path))
        {
            PlayerStatSO loadedPlayerData = JsonUtility.FromJson<PlayerStatSO>(File.ReadAllText(path));
            playerStat = loadedPlayerData;
        }
        else
        {
            Debug.Log("Save file not found");
        }
    }
}