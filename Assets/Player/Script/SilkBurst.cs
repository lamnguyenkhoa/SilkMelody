using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SilkBurst : MonoBehaviour
{
    public int damage;
    public float knockbackPower;
    [HideInInspector] public Vector3 playerPos;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Enemy enemy = collision.GetComponent<Enemy>();
        if (enemy == null && collision.transform.parent != null)
            enemy = collision.transform.parent.GetComponent<Enemy>();

        if (enemy)
        {
            Vector2 knockbackForce = (Vector2)(enemy.transform.position - playerPos).normalized;
            knockbackForce = new Vector2(knockbackForce.x * knockbackPower, 5f);
            enemy.Damaged(damage, knockbackForce);
        }
    }
}
