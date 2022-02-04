using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
    public PlayerData playerData;
    public WorldData worldData;
    public static GameMaster instance;
    public AudioSource bgm;

    [Header("Crest")]
    public Talisman equippedTalisman; // Must alway wear a crest. Cannot null.

    [Header("RedTool")]
    public RedTool[] redToolData; // SO Database for all redTool. Order is important.
    public float[] redToolsCurrentCharge; // for ALL redTool, not just equipped one
    public RedTool.ToolName[] foundTools;
    public List<RedTool.ToolName> equippedTools = new List<RedTool.ToolName>();
    public int selectedId; // index of equippedTools

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
        // For new game
        redToolsCurrentCharge = new float[redToolData.Length];
        for (int i = 0; i < redToolData.Length; i++)
        {
            redToolsCurrentCharge[i] = redToolData[i].maxCharge;
        }
    }

    public void SwapTool(bool rightDirection)
    {
        if (equippedTools.Count == 0)
            return;

        if (rightDirection)
            selectedId++;
        else
            selectedId--;

        if (selectedId > equippedTools.Count - 1)
            selectedId = 0;
        if (selectedId < 0)
            selectedId = equippedTools.Count - 1;
    }

    public void EquipUnequipRedTool(RedTool.ToolName tool)
    {
        int nRedSlot = equippedTalisman.redSlots.Length;
        // Unequip
        if (equippedTools.Contains(tool))
        {
            equippedTools.Remove(tool);
        }
        // Equip
        else if (equippedTools.Count < nRedSlot)
        {
            equippedTools.Add(tool);
        }

        // Remove uneqipped but selected tool
        if (selectedId >= equippedTools.Count)
        {
            if (equippedTools.Count > 0)
            {
                selectedId = 0;
                Debug.Log("Fallback, used " + equippedTools[selectedId] + " to replaced " + tool);
            }
        }

        // Update crest
        equippedTalisman.UpdateSlotImage();
    }
}