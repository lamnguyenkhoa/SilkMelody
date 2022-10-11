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

    public void BeginToolAnimLock()
    {
        player.BeginToolAnimLock();
    }

    public void EndToolAnimLock()
    {
        player.EndToolAnimLock();
    }

    public void LifebloodNeedleEffect()
    {
        player.LifebloodNeedleEffect();
    }

    public void BeginHeal()
    {
        player.BeginHeal();
    }

    public void ActualHeal()
    {
        player.ActualHeal();
    }

    public void EndHeal()
    {
        player.EndHeal();
    }

    public void BeginGossamer()
    {
        player.BeginGossamer();
    }

    public void EndGossamer()
    {
        // Temp
        player.EndHeal();
    }

    public void BeginSilkBurst()
    {
        player.BeginSilkBurst();
    }

    public void ThrowSilkBurst()
    {
        player.ThrowSilkBurst();
    }

    public void CatchSilkBurstRecoil()
    {
        player.CatchSilkBurstRecoil();
    }

    public void EndSilkBurst()
    {
        // Temp
        player.EndHeal();
    }
}