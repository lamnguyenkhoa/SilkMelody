using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WorldData
{
    // Note: DO NOT CHANGE THE ORDER OF ENUM. MAY FUCK THINGS UP.
    [Header("Collectibles")]
    public bool[] maskUpgrades = new bool[3];
    public bool[] silkUpgrades = new bool[3];

    [Header("Upgrades")]
    public bool doubleJump;
    public bool wallJump;

    [Header("DoorSwitch")]
    public bool[] doorSwitches = new bool[3];

    public enum BossEnum
    { Batter, Cela, GreatCrawler }

    [Header("Boss")]
    public bool[] bossDefeated = new bool[3]; // For conditional trigger

    [Header("Map")]
    public List<string> visitedRooms = new List<string>();

    [Header("Other")]
    public int[] enemyKillCount;

    [Header("Shop")]
    public List<string> boughItemNameAndLocation = new List<string>();
    // Example: MossyTown_MaskShard02, MossyTown_LifebloodNeedle, Kingdom_BombBeetle


    public void ResetToNewGameState()
    {
        for (int i = 0; i < maskUpgrades.Length; i++)
            maskUpgrades[i] = false;
        for (int i = 0; i < silkUpgrades.Length; i++)
            silkUpgrades[i] = false;
        for (int i = 0; i < doorSwitches.Length; i++)
            doorSwitches[i] = false;
        for (int i = 0; i < bossDefeated.Length; i++)
            bossDefeated[i] = false;

        doubleJump = false;
        wallJump = false;

        visitedRooms.Clear();
        boughItemNameAndLocation.Clear();
    }
}
