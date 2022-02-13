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
        yield return new WaitForSeconds(0.1f);
        EventSystem.current.SetSelectedGameObject(selectedGameObject, new BaseEventData(EventSystem.current));
    }

    private void AddSlotPrefabToGrid()
    {
        int[] inventoryItemAmount = GameMaster.instance.playerData.inventoryItemAmount;

        // Yes I know there are faster whay with for-loop
        // But we wanna use a bunch of if to control their appear order in grid
        if (inventoryItemAmount[(int)InventoryItem.ItemName.verdantMantle] > 0)
            Instantiate(GameMaster.instance.inventorySlotPrefabs[(int)InventoryItem.ItemName.verdantMantle], inventoryGrid.transform, false);

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
