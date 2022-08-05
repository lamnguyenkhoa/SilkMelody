using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Currently just a copy-and-modified version of LapisLazerAI.
/// Should refactor for better reusability.
/// </summary>
public class BatterPetAI : MonoBehaviour
{
    private float attackTimer;
    public float aimDuration = 2f;
    public float damageDuration = 0.25f;
    public float attackCooldown = 2f;
    public LayerMask playerMask;
    public LayerMask groundMask;
    public LayerMask shootTargetMask;
    public BossBatterAI owner;

    private Enemy stat;
    private Transform player;
    public LineRenderer lineRenderer;
    [SerializeField] private bool isShooting;
    [SerializeField] private float beamAimWidth = 0.05f;
    [SerializeField] private float beamDamageWidth = 0.05f;

    private void Start()
    {
        stat = GetComponent<Enemy>();
        player = GameObject.Find("Tenroh").transform;
        attackTimer = attackTimer * 0.75f; // fast first time attack
    }

    private void Update()
    {
        if (owner.stat.isDead)
            stat.Death();

        if (stat.isDead)
        {
            StopAllCoroutines();
            lineRenderer.positionCount = 0;
            this.enabled = false;
            owner.currentPetCounter--;
        }

        if (!isShooting)
        {
            LookAtPlayer();
            attackTimer += Time.deltaTime;
        }
        if (attackTimer >= attackCooldown)
        {
            if (!isShooting)
            {
                StartCoroutine(ShootLaserBeam());
                attackTimer = 0f;
            }
        }
    }

    private void LookAtPlayer()
    {
        Vector3 dir = player.position - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        angle += 180f;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    private IEnumerator ShootLaserBeam()
    {
        // Aiming
        isShooting = true;
        Vector2 playerDirection = (player.position - transform.position).normalized;
        RaycastHit2D laserDestination = Physics2D.Raycast(transform.position, playerDirection, 100f, groundMask);
        lineRenderer.startWidth = beamAimWidth;
        lineRenderer.endWidth = beamAimWidth;
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, transform.position);
        if (laserDestination)
            lineRenderer.SetPosition(1, laserDestination.point);
        else
            lineRenderer.SetPosition(1, (Vector2)transform.position + playerDirection * 100f); // Raycast not hit ground
        yield return new WaitForSeconds(aimDuration);

        // Shoot
        lineRenderer.startWidth = beamDamageWidth;
        lineRenderer.endWidth = beamDamageWidth;
        RaycastHit2D hitCast = Physics2D.Raycast(transform.position, playerDirection, 100f, shootTargetMask);
        if (hitCast) // Hit something
            if (hitCast.collider.gameObject.GetComponent<Player>()) // That something is a player
                player.GetComponent<Player>().Damaged(stat.damage, playerDirection);
        yield return new WaitForSeconds(damageDuration);

        lineRenderer.positionCount = 0;
        isShooting = false;
    }
}