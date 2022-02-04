using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "BlueTool", menuName = "ScriptableObjects/BlueTool")]
public class BlueTool : ScriptableObject
{
    public string displayName;
    [TextArea(3, 10)]
    public string description;
    public Sprite sprite;
    public enum ToolName
    { acidOrb, weaverMask }
    public ToolName thisToolName;
}