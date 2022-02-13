using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "BlueTool", menuName = "ScriptableObjects/BlueTool")]
public class BlueTool : ItemData
{
    public enum ToolName
    { acidOrb, weaverMask }
    public ToolName thisToolName;
}
