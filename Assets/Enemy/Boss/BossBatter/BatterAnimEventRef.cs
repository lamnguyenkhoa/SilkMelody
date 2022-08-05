using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatterAnimEventRef : MonoBehaviour
{
    private BossBatterAI batter;

    private void Start()
    {
        batter = transform.parent.GetComponent<BossBatterAI>();
    }

    public void BeginAttack()
    {
        batter.BeginAttack();
    }

    public void AttackDealDamage()
    {
        batter.AttackDealDamage();
    }

    public void EndAttack()
    {
        batter.EndAttack();
    }

    public void CreateBall()
    {
        batter.CreateBallAndHit();
    }
}