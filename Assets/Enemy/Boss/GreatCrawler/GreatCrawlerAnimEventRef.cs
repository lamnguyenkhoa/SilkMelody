using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreatCrawlerAnimEventRef : MonoBehaviour
{
    private GreatCrawlerAI boss;

    private void Start()
    {
        boss = transform.parent.GetComponent<GreatCrawlerAI>();
    }

    public void BeginAttack()
    {
        boss.BeginAttack();
    }

    public void AttackDealDamage()
    {
        boss.AttackDealDamage();
    }

    public void EndAttack()
    {
        boss.EndAttack();
    }
}
