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

    [Header("Debug")]
    public bool debugInventory;

    [Header("InventoryData")]
    public GameObject[] inventorySlotPrefabs; // Order matter.

    [Header("JournalData")]
    public EnemyLore[] enemyLoreData; // Order matter.

    [Header("Talisman")]
    public GameObject[] talismanData; // prefabs
    public Talisman equippedTalisman; // Set the default talisman in inspector. Cannot null.
    public GameObject talismanHolder;
    public delegate void TalismanChangeAction();
    public event TalismanChangeAction OnTalismanChange;
    private bool loadTalismanFromSave = false;

    [Header("ToolsData")]
    public RedTool[] redToolData; // SO Database for all redTool. Order is important.
    public BlueTool[] blueToolData;
    public YellowTool[] yellowToolData;

    private void Awake()
    {
        if (!instance)
        {
            instance = this;
            InitNewGameData();
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

            if (!loadTalismanFromSave)
                LoadTalismanFromSave();
            else
                equippedTalisman = talismanHolder.transform.GetChild(0).GetComponent<Talisman>();
        }
    }

    public void ChangeToNextTalisman()
    {
        if (playerData.foundTalismans.Count <= 1)
            return;

        UnequipAllTools();

        // Get current equip talisman id;
        Talisman.TalismanName currentTalismanName = equippedTalisman.thisTalismanName;
        int index = playerData.foundTalismans.FindIndex(x => x == currentTalismanName);

        // Get the next index
        index++;
        if (index > playerData.foundTalismans.Count - 1)
            index = 0;
        Destroy(talismanHolder.transform.GetChild(0).gameObject);
        GameObject newTalisman = Instantiate(talismanData[(int)playerData.foundTalismans[index]], talismanHolder.transform, false);
        equippedTalisman = newTalisman.GetComponent<Talisman>();
        playerData.equippedTalismanName = equippedTalisman.thisTalismanName;
    }

    /// <summary>
    /// For after loading data.
    /// </summary>
    /// <returns></returns>
    public void LoadTalismanFromSave()
    {
        if (playerData.foundTalismans.Contains(playerData.equippedTalismanName))
        {
            equippedTalisman = null;
            Destroy(talismanHolder.transform.GetChild(0).gameObject);
            GameObject newTalisman = Instantiate(talismanData[(int)playerData.equippedTalismanName], talismanHolder.transform, false);
            equippedTalisman = newTalisman.GetComponent<Talisman>();
            equippedTalisman.UpdateSlotImage();
            loadTalismanFromSave = true;
        }
        else
        {
            Debug.Log("Cannot found this talisman in inventory " + playerData.equippedTalismanName);
        }
    }

    public void UnequipAllTools()
    {
        playerData.equippedRedTools.Clear();
        playerData.equippedBlueTools.Clear();
        playerData.equippedYellowTools.Clear();
        equippedTalisman.UpdateSlotImage();
        OnTalismanChange();
    }

    // Init data for new game (if not overwrite by loading)
    private void InitNewGameData()
    {
        playerData = new PlayerData();
        worldData = new WorldData();
        playerData.foundTalismans.Add(Talisman.TalismanName.wanderer);
        playerData.redToolsCurrentCharge = new float[redToolData.Length];

        for (int i = 0; i < redToolData.Length; i++)
            playerData.redToolsCurrentCharge[i] = redToolData[i].maxCharge;
        playerData.inventoryItemAmount = new int[inventorySlotPrefabs.Length];
        for (int i = 0; i < inventorySlotPrefabs.Length; i++)
            playerData.inventoryItemAmount[i] = 0;
        worldData.enemyKillCount = new int[enemyLoreData.Length];
        for (int i = 0; i < enemyLoreData.Length; i++)
            worldData.enemyKillCount[i] = 0;

        playerData.inventoryItemAmount[(int)InventoryItem.ItemName.verdantMantle] = 1;
        playerData.inventoryItemAmount[(int)InventoryItem.ItemName.silkSpool] = 1;
        playerData.inventoryItemAmount[(int)InventoryItem.ItemName.mask] = 1;
        playerData.inventoryItemAmount[(int)InventoryItem.ItemName.needle] = 1;
        playerData.inventoryItemAmount[(int)InventoryItem.ItemName.carapaceBackpack] = 1;
        playerData.inventoryItemAmount[(int)InventoryItem.ItemName.notebookQuill] = 1;

        // For debugging
        if (Application.isEditor && debugInventory)
        {
            foreach (var talismaPrefab in talismanData)
            {
                Talisman.TalismanName talismanName = talismaPrefab.GetComponent<Talisman>().thisTalismanName;
                if (talismanName != Talisman.TalismanName.wanderer)
                {
                    playerData.foundTalismans.Add(talismanName);
                }
            }
            foreach (RedTool tool in redToolData)
            {
                playerData.foundRedTools.Add(tool.thisToolName);
            }
            foreach (BlueTool tool in blueToolData)
            {
                playerData.foundBlueTools.Add(tool.thisToolName);
            }
            foreach (YellowTool tool in yellowToolData)
            {
                playerData.foundYellowTools.Add(tool.thisToolName);
            }
        }
    }

    public void RefillRedTool()
    {
        // This should cost some coppershard or stuff, but not implemented yet.
        for (int i = 0; i < redToolData.Length; i++)
        {
            playerData.redToolsCurrentCharge[i] = redToolData[i].maxCharge;
        }
    }

    public void SwapTool(bool rightDirection)
    {
        if (playerData.equippedRedTools.Count == 0)
            return;

        if (rightDirection)
            playerData.selectedRedToolId++;
        else
            playerData.selectedRedToolId--;

        if (playerData.selectedRedToolId > playerData.equippedRedTools.Count - 1)
            playerData.selectedRedToolId = 0;
        if (playerData.selectedRedToolId < 0)
            playerData.selectedRedToolId = playerData.equippedRedTools.Count - 1;
    }

    public void EquipUnequipRedTool(RedTool.ToolName tool)
    {
        int nRedSlot = equippedTalisman.redSlots.Length;
        // Unequip
        if (playerData.equippedRedTools.Contains(tool))
        {
            playerData.equippedRedTools.Remove(tool);
        }
        // Equip
        else if (playerData.equippedRedTools.Count < nRedSlot)
        {
            playerData.equippedRedTools.Add(tool);
        }

        // Remove unequipped but selected tool
        if (playerData.selectedRedToolId >= playerData.equippedRedTools.Count)
        {
            if (playerData.equippedRedTools.Count > 0)
            {
                playerData.selectedRedToolId = 0;
                Debug.Log("Fallback, used " + playerData.equippedRedTools[playerData.selectedRedToolId] + " to replaced " + tool);
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
        if (playerData.equippedBlueTools.Contains(tool))
        {
            playerData.equippedBlueTools.Remove(tool);
        }
        // Equip
        else if (playerData.equippedBlueTools.Count < nBlueSlot)
        {
            playerData.equippedBlueTools.Add(tool);
        }

        // Update crest
        equippedTalisman.UpdateSlotImage();
        OnTalismanChange();
    }

    public void EquipUnequipYellowTool(YellowTool.ToolName tool)
    {
        int nYellowTool = equippedTalisman.yellowSlots.Length;
        // Unequip
        if (playerData.equippedYellowTools.Contains(tool))
        {
            playerData.equippedYellowTools.Remove(tool);
        }
        // Equip
        else if (playerData.equippedYellowTools.Count < nYellowTool)
        {
            playerData.equippedYellowTools.Add(tool);
        }

        // Update crest
        equippedTalisman.UpdateSlotImage();
        OnTalismanChange();
    }

    public void AddVisitedRoom(string roomName)
    {
        if (!worldData.visitedRooms.Contains(roomName))
        {
            worldData.visitedRooms.Add(roomName);
        }
    }
}
