using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStat", menuName = "ScriptableObjects/PlayerStat")]
public class PlayerStatSO : ScriptableObject
{
    public int maxHp = 5;
    public int currentHp = 5;
    public float damage = 1f;
    public float moveSpeed = 7f;
    public float jumpForce = 15f;
    public float enemyKnockbackPower = 2f;
    public float selfKnockbackPower = 1f;

    public float stunTime = 0.3f;
    public float iFrameTime = 3f;

    /** Note: If player get damaged, they will be stunned for a short time and has
     * i-frame for a long time.
     */
}