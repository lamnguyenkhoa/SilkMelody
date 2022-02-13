using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemData : ScriptableObject
{
    public string displayName;
    [TextArea(3, 10)]
    public string description;
    public Sprite sprite;
}
