using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InventoryItem", menuName = "ScriptableObjects/InventoryItem")]
public class InventoryItem : ItemData
{
    public enum ItemName
    {
        verdantMantle, silkSpool, mask, needle, coolKey, notebookQuill, carapaceBackpack, silkbindSandal
    }
    public ItemName thisItemName;
}
