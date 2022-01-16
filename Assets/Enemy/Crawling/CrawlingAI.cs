using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrawlingAI : MonoBehaviour
{
    private Rigidbody2D rb;
    private Enemy stat;
    public Transform checkObstaclePos;
    public bool isMovingRight = false;
    public float speed = 1f;
    private Vector2 moveVelocity;
    [SerializeField] private bool nearWall = false;
    [SerializeField] private bool nearFall = false;
    public LayerMask notWallMask;
    [SerializeField] private bool stabilized;

    private void Start()
    {
        stat = GetComponent<Enemy>();
        rb = GetComponent<Rigidbody2D>();
        if (isMovingRight)
            moveVelocity = new Vector2(speed, 0);
        else
            moveVelocity = new Vector2(-speed, 0);
        rb.velocity = moveVelocity;
    }

    private void FixedUpdate()
    {
        if (stat.isDead)
            this.enabled = false;

        Patrol();
    }

    private void Patrol()
    {
        // Check if end of platform or hit wall (exclude player's layer)
        RaycastHit2D groundInfo = Physics2D.Raycast(checkObstaclePos.position, Vector2.down, 1f, ~notWallMask);
        if (groundInfo.collider)
            nearFall = false;
        else
            nearFall = true;

        Collider2D tmp = Physics2D.OverlapCircle(checkObstaclePos.position, 0.1f, ~notWallMask);
        if (tmp)
            nearWall = true;
        else
            nearWall = false;

        if (!nearFall && !nearWall)
            stabilized = true;

        if (stabilized && (nearFall || nearWall))
        {
            stabilized = false;
            ChangeDirection();
        }
        if (stabilized && !stat.shouldStopMoving)
            rb.velocity = Vector2.Lerp(rb.velocity, moveVelocity, 0.1f);
    }

    private void ChangeDirection()
    {
        rb.velocity = Vector2.zero;
        if (isMovingRight)
        {
            isMovingRight = false;
            moveVelocity.x = -speed;
            transform.localScale = new Vector2(1, 1);
        }
        else
        {
            isMovingRight = true;
            moveVelocity.x = speed;
            transform.localScale = new Vector2(-1, 1);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawRay(checkObstaclePos.position, Vector2.down);
        Gizmos.DrawWireSphere(checkObstaclePos.position, 0.1f);
    }
}