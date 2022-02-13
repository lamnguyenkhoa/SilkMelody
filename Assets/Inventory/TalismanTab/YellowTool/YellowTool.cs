using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "YellowTool", menuName = "ScriptableObjects/YellowTool")]
public class YellowTool : ItemData
{
    public enum ToolName
    { crabOrb }
    public ToolName thisToolName;
}
