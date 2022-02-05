using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
    public PlayerData playerData;
    public WorldData worldData;
    public static GameMaster instance;
    public AudioSource bgm;

    [Header("Talisman")]
    public GameObject[] talismanData; // prefabs
    public List<Talisman.TalismanName> foundTalismans = new List<Talisman.TalismanName>();
    public Talisman equippedTalisman; // Set the default talisman in inspector. Cannot null.
    public GameObject talismanHolder;
    public delegate void TalismanChangeAction();
    public event TalismanChangeAction OnTalismanChange;

    [Header("RedTool")]
    public RedTool[] redToolData; // SO Database for all redTool. Order is important.
    public float[] redToolsCurrentCharge; // for ALL redTool, not just equipped one
    public List<RedTool.ToolName> foundRedTools = new List<RedTool.ToolName>();
    public List<RedTool.ToolName> equippedRedTools = new List<RedTool.ToolName>();
    public int selectedId; // index of equippedTools

    [Header("BlueTool")]
    public BlueTool[] blueToolData;
    public List<BlueTool.ToolName> foundBlueTools = new List<BlueTool.ToolName>();
    public List<BlueTool.ToolName> equippedBlueTools = new List<BlueTool.ToolName>();

    [Header("YellowTool")]
    public YellowTool[] yellowToolData;
    public List<YellowTool.ToolName> foundyellowTools = new List<YellowTool.ToolName>();
    public List<YellowTool.ToolName> equippedYellowTools = new List<YellowTool.ToolName>();

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

    public void PatchInventoryReference()
    {
        GameObject inventoryMenu = GameObject.Find("InventoryMenu");
        if (inventoryMenu == null)
        {
            Debug.Log("Scene does not have inventoryMenu");
        }
        else
        {
            talismanHolder = inventoryMenu.transform.GetChild(0).Find("TalismanGroup").Find("TalismanHolder").gameObject;
            equippedTalisman = talismanHolder.transform.GetChild(0).GetComponent<Talisman>();
        }
    }

    public void ChangeToNextTalisman()
    {
        if (foundTalismans.Count <= 1)
            return;

        UnequipAllTools();

        // Get current equip talisman id;
        Talisman.TalismanName currentTalismanName = equippedTalisman.thisTalismanName;
        int index = foundTalismans.FindIndex(x => x == currentTalismanName);

        // Get the next index
        index++;
        if (index > foundTalismans.Count - 1)
            index = 0;
        Destroy(talismanHolder.transform.GetChild(0).gameObject);
        GameObject newTalisman = Instantiate(talismanData[(int)foundTalismans[index]], talismanHolder.transform, false);
        equippedTalisman = newTalisman.GetComponent<Talisman>();
    }

    public void UnequipAllTools()
    {
        equippedRedTools.Clear();
        equippedBlueTools.Clear();
        equippedYellowTools.Clear();
        equippedTalisman.UpdateSlotImage();
        OnTalismanChange();
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
        if (equippedRedTools.Count == 0)
            return;

        if (rightDirection)
            selectedId++;
        else
            selectedId--;

        if (selectedId > equippedRedTools.Count - 1)
            selectedId = 0;
        if (selectedId < 0)
            selectedId = equippedRedTools.Count - 1;
    }

    public void EquipUnequipRedTool(RedTool.ToolName tool)
    {
        int nRedSlot = equippedTalisman.redSlots.Length;
        // Unequip
        if (equippedRedTools.Contains(tool))
        {
            equippedRedTools.Remove(tool);
        }
        // Equip
        else if (equippedRedTools.Count < nRedSlot)
        {
            equippedRedTools.Add(tool);
        }

        // Remove unequipped but selected tool
        if (selectedId >= equippedRedTools.Count)
        {
            if (equippedRedTools.Count > 0)
            {
                selectedId = 0;
                Debug.Log("Fallback, used " + equippedRedTools[selectedId] + " to replaced " + tool);
            }
        }

        // Update crest
        equippedTalisman.UpdateSlotImage();
        OnTalismanChange();
    }

    public void EquipUnequipBlueTool(BlueTool.ToolName tool)
    {
        int nBlueSlot = equippedTalisman.blueSlots.Length;
        // Unequip
        if (equippedBlueTools.Contains(tool))
        {
            equippedBlueTools.Remove(tool);
        }
        // Equip
        else if (equippedBlueTools.Count < nBlueSlot)
        {
            equippedBlueTools.Add(tool);
        }

        // Update crest
        equippedTalisman.UpdateSlotImage();
        OnTalismanChange();
    }

    public void EquipUnequipYellowTool(YellowTool.ToolName tool)
    {
        int nYellowTool = equippedTalisman.yellowSlots.Length;
        // Unequip
        if (equippedYellowTools.Contains(tool))
        {
            equippedYellowTools.Remove(tool);
        }
        // Equip
        else if (equippedYellowTools.Count < nYellowTool)
        {
            equippedYellowTools.Add(tool);
        }

        // Update crest
        equippedTalisman.UpdateSlotImage();
        OnTalismanChange();
    }
}
