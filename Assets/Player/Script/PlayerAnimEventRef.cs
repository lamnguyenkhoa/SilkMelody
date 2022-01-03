using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimEventRef : MonoBehaviour
{
    private Player player;

    private void Start()
    {
        player = transform.parent.GetComponent<Player>();
    }

    public void BeginAttackAnim()
    {
        player.BeginAttackAnim();
    }

    public void EndAttackAnim()
    {
        player.EndAttackAnim();
    }
}