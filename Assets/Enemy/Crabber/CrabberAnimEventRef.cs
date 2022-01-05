using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrabberAnimEventRef : MonoBehaviour
{
    private CrabberAI crabber;

    private void Start()
    {
        crabber = transform.parent.GetComponent<CrabberAI>();
    }

    public void BeginAttack()
    {
        crabber.BeginAttack();
    }

    public void EndAttack()
    {
        crabber.EndAttack();
    }

    public void DashForward()
    {
        crabber.DashForward();
    }
}