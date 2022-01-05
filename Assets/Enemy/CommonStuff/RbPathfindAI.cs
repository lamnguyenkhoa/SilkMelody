using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

#pragma warning disable 0168 // variable declared but not used.
#pragma warning disable 0219 // variable assigned but not used.
#pragma warning disable 0414 // private field assigned but not used.

/// <summary>
/// Support script for integrate A* Pathfinding script
/// with Unity Rigidbody system.
/// </summary>
public class RbPathfindAI : MonoBehaviour
{
    private Transform target;
    public Transform spriteHolder;
    public bool canFly = true;

    public float speed = 200f;
    public float nextWaypointDistance = 3f;

    private Path path;
    [SerializeField] private int currentWaypoint;
    private bool reachedEndOfPath = false;

    private Seeker seeker;
    private Rigidbody2D rb;
    private Enemy stat;

    public Vector2 direction;

    // Start is called before the first frame update
    private void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        stat = GetComponent<Enemy>();
        target = GameObject.Find("Tenroh").transform;

        InvokeRepeating("UpdatePath", 0f, 0.5f);
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (stat.isDead)
        {
            seeker.enabled = false;
            this.enabled = false;
        }

        if (path == null)
            return;

        if (currentWaypoint >= path.vectorPath.Count)
        {
            reachedEndOfPath = true;
            return;
        }
        else
        {
            reachedEndOfPath = false;
        }

        direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        Vector2 force = direction * speed * Time.fixedDeltaTime;
        if (!canFly)
            force.y = 1f;

        if (!stat.shouldStopMoving)
            rb.AddForce(force);

        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);
        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }

        // Flip sprite based on target position
        if (force.x >= 0.01f) // right
        {
            spriteHolder.localScale = new Vector2(-1, spriteHolder.localScale.y);
        }
        else if (force.x <= -0.01f) // left
        {
            spriteHolder.localScale = new Vector2(1, spriteHolder.localScale.y);
        }
    }

    private void UpdatePath()
    {
        if (seeker.IsDone())
            seeker.StartPath(rb.position, target.position, OnPathComplete);
    }

    private void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }
}