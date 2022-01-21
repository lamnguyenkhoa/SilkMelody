using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DipsaAI : MonoBehaviour
{
    public float timeBetweenAttack = 3f;
    public float bulletSpeed = 3f;
    protected float attackTimer = 0f;
    public GameObject bulletPrefab;
    public Transform shootPos;
    protected Transform player;
    protected Enemy stat;
    protected bool alerted;
    protected RbPathfindAI pathfindAI;
    public float detectionRadius;
    public LayerMask playerMask;

    protected virtual void Start()
    {
        stat = GetComponent<Enemy>();
        pathfindAI = GetComponent<RbPathfindAI>();
        player = GameObject.Find("Tenroh").transform;
    }

    protected virtual void Update()
    {
        if (stat.isDead)
            this.enabled = false;

        if (alerted)
        {
            if (!pathfindAI.enabled)
                pathfindAI.enabled = true;

            if (Vector2.Distance(transform.position, player.position) <= detectionRadius)
                stat.shouldStopMoving = true;
            else
                stat.shouldStopMoving = false;

            attackTimer += Time.deltaTime;
            if (attackTimer >= timeBetweenAttack)
            {
                if (Vector2.Distance(transform.position, player.position) <= detectionRadius)
                {
                    Attack();
                    attackTimer = 0f;
                }
            }
        }
        else
        {
            attackTimer = 0f;
            if (pathfindAI.enabled)
                pathfindAI.enabled = false;
            if (Physics2D.OverlapCircle(transform.position, detectionRadius, playerMask))
            {
                alerted = true;
            }
        }
    }

    protected virtual void Attack()
    {
        StartCoroutine(stat.StopMoving());
        Vector2 playerDirection = (player.position - transform.position).normalized;

        // Shoot bullet here
        GameObject newBullet = Instantiate(bulletPrefab, shootPos.position, Quaternion.identity);
        newBullet.GetComponent<Rigidbody2D>().velocity = playerDirection * bulletSpeed;
    }

    protected void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}