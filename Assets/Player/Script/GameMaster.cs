using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
    public PlayerData playerData;
    public WorldData worldData;
    public static GameMaster instance;
    public AudioSource bgm;

    private void Awake()
    {
        if (!instance)
        {
            instance = this;
            playerData = new PlayerData();
            worldData = new WorldData();
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            // Change area/region BGM
            if (instance.bgm.clip != this.bgm.clip)
            {
                instance.bgm.clip = this.bgm.clip;
                instance.bgm.Play();
            }
            Destroy(this.gameObject);
        }
    }
}