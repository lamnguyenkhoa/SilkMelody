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

    public float minIFrame = 0.3f;
    public float damagedFreezeTime = 0.2f;

    /** Note: If player get damaged, they will go into i-frame till they hit the ground (or 0.3f if
     * they are already on the ground. During the i-frame period, if they attack or dash, they will
     * cancel out of it.
     */
}