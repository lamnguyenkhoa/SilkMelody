using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shockwave : MonoBehaviour
{
    public int damage = 1;
    public Transform wallChecker;
    public float checkRadius;
    public LayerMask wallMask;
    [HideInInspector] public Vector2 kinematicVelocity;
    private Rigidbody2D rb;

    private void Start()
    {
        Destroy(this.gameObject, 10f);
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        rb.velocity = kinematicVelocity;
        if (Physics2D.OverlapCircle(wallChecker.position, checkRadius, wallMask))
        {
            Destroy(this.gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Player player = collision.transform.GetComponent<Player>();
        if (player)
        {
            player.Damaged(damage, (player.transform.position - transform.position).normalized);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(wallChecker.position, checkRadius);
    }
}