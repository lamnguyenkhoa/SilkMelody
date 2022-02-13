using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class InventoryItemButton : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    private GameObject selectFrame;
    public InventoryItem itemData;

    private void OnEnable()
    {
        selectFrame = transform.Find("SelectFrame").gameObject;

        // Update amount / quantity text
        if (transform.Find("Amount"))
        {
            TextMeshProUGUI amountText = transform.Find("Amount").GetComponent<TextMeshProUGUI>();
            int amount = GameMaster.instance.playerData.inventoryItemAmount[(int)itemData.thisItemName];
            if (amount > 1)
            {
                amountText.gameObject.SetActive(true);
                amountText.text = amount.ToString();
            }
            else
            {
                amountText.gameObject.SetActive(false);
            }
        }
        else
        {
            // Does not contain "Amount" gameObject
            return;
        }
    }

    private void OnDisable()
    {
        selectFrame.SetActive(false);
    }

    public void OnSelect(BaseEventData eventData)
    {
        selectFrame.SetActive(true);
        transform.root.GetComponent<InventoryMenu>().movingButtonSound.Play();
        if (itemData != null)
        {
            InventoryInfoBox.instance.nameText.text = itemData.displayName;
            InventoryInfoBox.instance.descText.text = itemData.description;
        }
        else
        {
            Debug.Log("Inventory item data null " + transform.name);
        }
    }

    public void OnDeselect(BaseEventData eventData)
    {
        selectFrame.SetActive(false);
    }
}
