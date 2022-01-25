using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStat", menuName = "ScriptableObjects/PlayerStat")]
[System.Serializable]
public class PlayerStatSO : ScriptableObject
{
    [Header("Stat")]
    public int maxHp = 5;
    public int currentHp = 5;
    public int currentSilk = 0;
    public int maxSilk = 8;
    public int silkHeal = 3;
    public float damage = 1f;
    public float moveSpeed = 7f;
    public float jumpForce = 12f;
    public float dashForce = 20f;
    public int maxDashCount = 1;
    public float dashTime = 0.25f;
    public float enemyKnockbackPower = 5f;
    public float selfKnockbackPower = 1f;
    public float attackCooldown = 0.15f;
    public float parryWindow = 0.25f;
    public float parryCooldown = 1f;

    [Header("Location")]
    public string respawnScene = "";
    public string respawnChairName = "";
    public Vector3 respawnPos = Vector3.zero; // Use when there is no chair

    [Header("Inventory")]
    public int copperShard = 0;
    public int scaleShard = 0;

    [Header("Misc")]
    public float stunTime = 0.4f;
    public float iFrameTime = 1.5f;

    /** Note: If player get damaged, they will be stunned for a short time and has
     * i-frame for a long time.
     */

    public void ResetToNewGameState()
    {
        maxHp = 5;
        currentHp = 5;
        currentSilk = 0;
        maxSilk = 8;
        silkHeal = 3;
        damage = 1f;
        moveSpeed = 7f;
        jumpForce = 12f;
        dashForce = 20f;
        maxDashCount = 1;
        dashTime = 0.25f;
        enemyKnockbackPower = 5f;
        selfKnockbackPower = 1f;
        attackCooldown = 0.15f;
        parryWindow = 0.25f;
        parryCooldown = 1f;
        copperShard = 0;
        scaleShard = 0;
        stunTime = 0.4f;
        iFrameTime = 1.5f;
        respawnChairName = "";
        respawnScene = "DirtCave0";
        respawnPos = Vector3.zero;
    }

    //public void CopyStatFromThis(PlayerStatSO statToCopy)
    //{
    //    maxHp = statToCopy.maxHp;
    //    currentHp = statToCopy.currentHp;
    //    currentSilk = statToCopy.currentSilk;
    //    maxSilk = statToCopy.maxSilk;
    //    silkHeal = statToCopy.silkHeal;
    //    damage = statToCopy.damage;
    //    moveSpeed = statToCopy.moveSpeed;
    //    jumpForce = statToCopy.jumpForce;
    //    dashForce = statToCopy.dashForce;
    //    maxDashCount = statToCopy.maxDashCount;
    //    dashTime = statToCopy.dashTime;
    //    enemyKnockbackPower = statToCopy.enemyKnockbackPower;
    //    selfKnockbackPower = statToCopy.selfKnockbackPower;
    //    attackCooldown = statToCopy.attackCooldown;
    //    parryWindow = statToCopy.parryWindow;
    //    parryCooldown = statToCopy.parryCooldown;
    //    copperShard = statToCopy.copperShard;
    //    scaleShard = statToCopy.scaleShard;
    //    stunTime = statToCopy.stunTime;
    //    iFrameTime = statToCopy.iFrameTime;
    //}
}