using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryItemButton : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    private GameObject selectFrame;
    public InventoryItem itemData;

    private void OnEnable()
    {
        selectFrame = transform.GetChild(0).gameObject;
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
