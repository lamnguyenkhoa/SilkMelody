using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatterBall : MonoBehaviour
{
    private Enemy stat;
    public int maxBounce;
    private int bounceCounter;

    private void Start()
    {
        stat = GetComponent<Enemy>();
        bounceCounter = 0;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        bounceCounter++;
        Player player = collision.transform.GetComponent<Player>();
        if (player || bounceCounter > maxBounce)
        {
            stat.Death();
            return;
        }

        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
        }
        else
        {
            // Should never reach here. This is just for failsafe
            stat.Death();
        }
    }
}