using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwistedDipsaAI : DipsaAI
{
    public GameObject[] bulletPrefabs;
    public float spreadAngle = 15f;

    protected override void Attack()
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
}