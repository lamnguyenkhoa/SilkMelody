using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "RedTool", menuName = "ScriptableObjects/RedTool")]
public class RedTool : ScriptableObject
{
    public float maxCharge;
    public int fixCostPerCharge;
    public Sprite sprite;

    public enum ToolName
    { throwBlade, trippleKnife, lifebloodNeedle }
    public ToolName thisToolName;
}