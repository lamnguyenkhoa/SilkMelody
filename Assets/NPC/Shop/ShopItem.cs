using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ShopItem", menuName = "ScriptableObjects/ShopItem")]
public class ShopItem : ScriptableObject
{
    public string itemName;
    public int price;
    public Sprite image;

    [TextArea(5, 10)]
    public string description;

    public enum ShopItemEnum
    {
        lifebloodNeedle, maskShard, sentinelTalisman
    }
    public ShopItemEnum itemEnum;

}
