using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gossamer : MonoBehaviour
{
    public float damageInterval = 0.1f;
    public int damage;
    private float timer;
    public LayerMask enemyLayer;
    private Vector2 center;
    private float radius;
    [HideInInspector] public Vector3 playerPos;

    private void Start()
    {
        center = GetComponent<CircleCollider2D>().offset + (Vector2)transform.position;
        radius = GetComponent<CircleCollider2D>().radius;
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer > damageInterval)
        {
            timer = 0f;

            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(center, radius * transform.localScale.x, enemyLayer);
            foreach (Collider2D hitEnemy in hitEnemies)
            {
                Enemy enemy = hitEnemy.GetComponent<Enemy>();
                if (enemy == null && hitEnemy.transform.parent != null)
                    enemy = hitEnemy.transform.parent.GetComponent<Enemy>();
                Vector2 knockbackForce = (Vector2)(enemy.transform.position - playerPos).normalized;
                knockbackForce = knockbackForce * 0.01f;
                enemy.Damaged(damage, knockbackForce);
            }
        }
    }
}
