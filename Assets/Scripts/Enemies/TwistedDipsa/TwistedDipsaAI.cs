using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwistedDipsaAI : MonoBehaviour
{
    public float timeBetweenAttack = 3f;
    public float bulletSpeed = 3f;
    private float attackTimer = 0f;
    public GameObject bulletPrefab;
    public Transform shootPos;
    public Transform player;

    private void Start()
    {
    }

    private void Update()
    {
        attackTimer += Time.deltaTime;
        if (attackTimer >= timeBetweenAttack)
        {
            Attack();
            attackTimer = 0f;
        }
    }

    private void Attack()
    {
        // Shoot bullet here
        GameObject newBullet = Instantiate(bulletPrefab, shootPos.position, Quaternion.identity);
        newBullet.GetComponent<Rigidbody2D>().velocity = (player.position - transform.position).normalized * bulletSpeed;
    }
}