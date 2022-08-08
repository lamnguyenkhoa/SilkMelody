using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LapisLazerAI : MonoBehaviour
{
    public float detectionRadius = 12f;
    public float attackRange = 18f;
    private bool alerted;
    private float attackTimer;
    public float aimDuration = 2f;
    public float damageDuration = 0.25f;
    public float attackCooldown = 2f;
    public LayerMask playerMask;
    public LayerMask groundMask;
    public LayerMask shootTargetMask;
    public GameObject pistilLight;
    public AudioSource shootAudioSource;
    public AudioClip chargeClip;
    public AudioClip laserClip;

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
        shootAudioSource.Stop();
    }

    private void Update()
    {
        if (stat.isDead)
        {
            StopAllCoroutines();
            pistilLight.SetActive(false);
            lineRenderer.positionCount = 0;
            shootAudioSource.Stop();
            this.enabled = false;
        }

        if (alerted)
        {
            if (!isShooting)
                attackTimer += Time.deltaTime;
            if (attackTimer >= attackCooldown)
            {
                if (Vector2.Distance(transform.position, player.position) <= attackRange && !isShooting)
                {
                    StartCoroutine(ShootLaserBeam());
                    attackTimer = 0f;
                }
            }
        }
        else
        {
            attackTimer = 0f;
            if (Physics2D.OverlapCircle(transform.position, detectionRadius, playerMask))
            {
                alerted = true;
            }
        }
    }

    private IEnumerator ShootLaserBeam()
    {
        // Aiming
        isShooting = true;
        shootAudioSource.Stop();
        shootAudioSource.clip = chargeClip;
        shootAudioSource.Play();
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
        shootAudioSource.Stop();
        shootAudioSource.clip = laserClip;
        shootAudioSource.Play();
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

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
