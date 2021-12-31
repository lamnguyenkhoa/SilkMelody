using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwistedDipsaAI : MonoBehaviour
{
    public float timeBetweenAttack = 3f;
    public float bulletSpeed = 3f;
    private float attackTimer = 0f;
    public GameObject[] bulletPrefabs;
    public Transform shootPos;
    public Transform player;
    public float spreadAngle = 15f;
    private Enemy stat;
    private bool alerted;
    private RbPathfindAI pathfindAI;
    public float detectionRadius = 4f;
    public LayerMask detectionMask;

    private void Start()
    {
        stat = GetComponent<Enemy>();
        pathfindAI = GetComponent<RbPathfindAI>();
    }

    private void Update()
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
            if (Physics2D.OverlapCircle(transform.position, detectionRadius, detectionMask))
            {
                alerted = true;
            }
        }
    }

    private void Attack()
    {
        StartCoroutine(stat.StopMoving());
        ShuffleBulletArray();
        Vector2 playerDirection = (player.position - transform.position).normalized;
        Vector2 spreadDirection1 = Quaternion.Euler(0, 0, spreadAngle) * playerDirection;
        Vector2 spreadDirection2 = Quaternion.Euler(0, 0, -spreadAngle) * playerDirection;

        // Shoot bullet here
        GameObject newBullet1 = Instantiate(bulletPrefabs[0], shootPos.position, Quaternion.identity);
        newBullet1.GetComponent<Rigidbody2D>().velocity = playerDirection * bulletSpeed;
        GameObject newBullet2 = Instantiate(bulletPrefabs[1], shootPos.position, Quaternion.identity);
        newBullet2.GetComponent<Rigidbody2D>().velocity = spreadDirection1 * bulletSpeed;
        GameObject newBullet3 = Instantiate(bulletPrefabs[2], shootPos.position, Quaternion.identity);
        newBullet3.GetComponent<Rigidbody2D>().velocity = spreadDirection2 * bulletSpeed;
    }

    private void ShuffleBulletArray()
    {
        GameObject tmp;
        for (int i = 0; i < bulletPrefabs.Length - 1; i++)
        {
            int random = Random.Range(i, bulletPrefabs.Length);
            tmp = bulletPrefabs[random];
            bulletPrefabs[random] = bulletPrefabs[i];
            bulletPrefabs[i] = tmp;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}