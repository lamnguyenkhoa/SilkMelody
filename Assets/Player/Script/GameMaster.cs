using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
    public PlayerData playerData;
    public WorldData worldData;
    public static GameMaster instance;
    public AudioSource bgm;

    [Header("RedTool")]
    public RedTool[] redTools;
    public float[] redToolsCurrentCharge;
    public RedTool.ToolName[] foundTools;
    public RedTool.ToolName[] equippedTools;
    public RedTool.ToolName selectedTool;

    private void Awake()
    {
        if (!instance)
        {
            instance = this;
            playerData = new PlayerData();
            worldData = new WorldData();
            InitRedToolsCharge();
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

    private void InitRedToolsCharge()
    {
        redToolsCurrentCharge = new float[redTools.Length];
        for (int i = 0; i < redTools.Length; i++)
        {
            redToolsCurrentCharge[i] = redTools[i].maxCharge;
        }
    }
}