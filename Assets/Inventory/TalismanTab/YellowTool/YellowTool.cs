using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "YellowTool", menuName = "ScriptableObjects/YellowTool")]
public class YellowTool : ScriptableObject
{
    public string displayName;
    [TextArea(3, 10)]
    public string description;
    public Sprite sprite;

    public enum ToolName
    { crabOrb }
    public ToolName thisToolName;
}
