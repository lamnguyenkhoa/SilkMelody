using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrabberAI : MonoBehaviour
{
    public float timeBetweenAttack = 3f;
    private float attackTimer = 0f;
    private Transform player;
    public float spreadAngle = 15f;
    private Enemy stat;
    private bool alerted;
    private bool inAttack;
    private RbPathfindAI pathfindAI;
    public float detectionRadius = 4f;
    public float attackRadius = 2f;

    public LayerMask playerMask;
    public Animator anim;
    private Rigidbody2D rb;
    public float dashAttackForce = 5f;

    private void Start()
    {
        stat = GetComponent<Enemy>();
        pathfindAI = GetComponent<RbPathfindAI>();
        player = GameObject.Find("Tenroh").transform;
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (stat.isDead)
            this.enabled = false;

        if (alerted)
        {
            if (!pathfindAI.enabled)
                pathfindAI.enabled = true;

            if (inAttack)
                return;

            if (Vector2.Distance(transform.position, player.position) <= attackRadius)
                stat.shouldStopMoving = true;
            else
                stat.shouldStopMoving = false;

            attackTimer += Time.deltaTime;
            if (attackTimer >= timeBetweenAttack)
            {
                if (Vector2.Distance(transform.position, player.position) <= attackRadius)
                {
                    anim.SetTrigger("attack");
                    attackTimer = 0f;
                }
            }
        }
        else
        {
            // Patrol behaviour

            // Detect
            attackTimer = 0f;
            if (pathfindAI.enabled)
                pathfindAI.enabled = false;
            if (Physics2D.OverlapCircle(transform.position, detectionRadius, playerMask))
            {
                alerted = true;
            }
        }
    }

    public void BeginAttack()
    {
        inAttack = true;
        stat.shouldStopMoving = true;
    }

    public void EndAttack()
    {
        inAttack = false;
        stat.shouldStopMoving = false;
    }

    public void DashForward()
    {
        Vector2 force = pathfindAI.direction;
        rb.AddForce(force * dashAttackForce);
    }
}