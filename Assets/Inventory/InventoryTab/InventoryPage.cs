using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryPage : MonoBehaviour
{
    public GameObject firstSelectedObject; //mask
    public GameObject inventoryGrid;

    private void OnEnable()
    {
        SetFirstSelectedButton();
        AddSlotPrefabToGrid();
    }

    private void OnDisable()
    {
        DeleteEverySlotInGrid();
    }

    private void SetFirstSelectedButton()
    {
        EventSystem.current.SetSelectedGameObject(null);
        StartCoroutine(SetSelectedGameObject(firstSelectedObject));
    }

    private IEnumerator SetSelectedGameObject(GameObject selectedGameObject)
    {
        yield return new WaitForSeconds(0.01f);
        EventSystem.current.SetSelectedGameObject(selectedGameObject, new BaseEventData(EventSystem.current));
    }

    private void AddSlotPrefabToGrid()
    {
        int[] inventoryItemAmount = GameMaster.instance.playerData.inventoryItemAmount;

        // Yes I know there are faster whay with for-loop
        // But we wanna use a bunch of if to control their appearance order in grid
        // Need to "if" for ALL item
        if (inventoryItemAmount[(int)InventoryItem.ItemName.verdantMantle] > 0)
            Instantiate(GameMaster.instance.inventorySlotPrefabs[(int)InventoryItem.ItemName.verdantMantle], inventoryGrid.transform, false);

        if (inventoryItemAmount[(int)InventoryItem.ItemName.notebookQuill] > 0)
            Instantiate(GameMaster.instance.inventorySlotPrefabs[(int)InventoryItem.ItemName.notebookQuill], inventoryGrid.transform, false);

        if (inventoryItemAmount[(int)InventoryItem.ItemName.carapaceBackpack] > 0)
            Instantiate(GameMaster.instance.inventorySlotPrefabs[(int)InventoryItem.ItemName.carapaceBackpack], inventoryGrid.transform, false);

        if (inventoryItemAmount[(int)InventoryItem.ItemName.silkbindSandal] > 0)
            Instantiate(GameMaster.instance.inventorySlotPrefabs[(int)InventoryItem.ItemName.silkbindSandal], inventoryGrid.transform, false);

        if (inventoryItemAmount[(int)InventoryItem.ItemName.coolKey] > 0)
            Instantiate(GameMaster.instance.inventorySlotPrefabs[(int)InventoryItem.ItemName.coolKey], inventoryGrid.transform, false);
    }

    private void DeleteEverySlotInGrid()
    {
        foreach (Transform child in inventoryGrid.transform)
        {
            Destroy(child.gameObject);
        }
    }
}
