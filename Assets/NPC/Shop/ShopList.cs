using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ShopList", menuName = "ScriptableObjects/ShopList")]
public class ShopList : ScriptableObject
{

    public ShopItem[] shopItems;
}
