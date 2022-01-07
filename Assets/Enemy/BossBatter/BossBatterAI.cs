using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** Boss design:
 * Moveset:
 * - Thow and hit a white ball, ranged attack
 * - Thow and hit a big red ball, ranged explosive attack, 2 dmg
 * - Normal attack, if player out range then he has a dash
 * - Charge toward player
 * - Jump and slam the ground, create shockwall and falling rock (2 dmg if slammed)
 * - Summon pet to attack, pet shoot laser beam
 * - Below 25% hp, move faster
 * Behaviour:
 * - Since I'm noob, most of his move will be selected randomly after he finished the previous one
 * - If player keep ledge grabbing, he will use ball attack
 * - Can only summon max 2 pet (pet can be killed in 1 hit)
 * - Can not use jump slam twice in a row
 */

public class BossBatterAI : MonoBehaviour
{
    [Header("Components")]
    public EnemySlash slashPrefab;
    private Player player;
    private Animator anim;
    private Transform spriteHolder;
    private Enemy stat;
    private Rigidbody2D rb;

    [Header("Moveset control")]
    public float timeBetweenAttack;
    [SerializeField] private bool inAttack;
    private float attackTimer;
    [SerializeField] private bool isFacingLeft;
    public enum Moveset
    { whiteBall, redBall, attack, charge, jumpSlam, summon }
    public Moveset selectedMove;

    [Header("Normal attack")]
    public float dashForce;
    public float runSpeed;
    public float meleeRange;

    [Header("Charge")]
    public float chargePrepTime;
    public float chargeAccel;
    public float maxChargeSpeed;
    public LayerMask wallMask;
    public Transform wallChecker;

    private void Start()
    {
        stat = GetComponent<Enemy>();
        player = GameObject.Find("Tenroh").GetComponent<Player>();
        spriteHolder = transform.Find("Sprite");
        anim = spriteHolder.GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (stat.isDead)
        {
            StopAllCoroutines();
            inAttack = false;
            this.enabled = false;
        }

        if (!inAttack)
        {
            FaceTowardPlayer();

            if (attackTimer < timeBetweenAttack)
            {
                attackTimer += Time.deltaTime;
            }
            else
            {
                SelectARandomMove();

                // Perform that attack
                switch (selectedMove)
                {
                    case Moveset.attack:
                        NormalAttack();
                        break;

                    case Moveset.charge:
                        Charge();
                        break;

                    case Moveset.jumpSlam:
                        break;
                }
            }
        }
    }

    private void NormalAttack()
    {
        attackTimer = 0f;
        StartCoroutine(RunAndAttack());
    }

    private void Charge()
    {
        attackTimer = 0f;
        StartCoroutine(ChargeController());
    }

    private void FaceTowardPlayer()
    {
        if (player.transform.position.x < transform.position.x)
        {
            spriteHolder.localScale = new Vector3(1, 1, 1);
            isFacingLeft = true;
        }
        else
        {
            spriteHolder.localScale = new Vector3(-1, 1, 1);
            isFacingLeft = false;
        }
    }

    private void SelectARandomMove()
    {
        int nMove = System.Enum.GetValues(typeof(Moveset)).Length;
        selectedMove = (Moveset)Random.Range(0, nMove);
    }

    /// <summary>
    /// Deprecated.
    /// </summary>
    public void BeginNormalAttack()
    {
        inAttack = true;
    }

    public void EndNormalAttack()
    {
        inAttack = false;
    }

    public void AttackDealDamage()
    {
        // Dash if player in front of batterbut still out of melee range
        float targetPosX = player.transform.position.x;
        if (isFacingLeft && (transform.position.x - targetPosX > meleeRange))
            rb.AddForce(new Vector2(-dashForce, 10f), ForceMode2D.Impulse);
        else if (!isFacingLeft && (targetPosX - transform.position.x > meleeRange))
            rb.AddForce(new Vector2(dashForce, 10f), ForceMode2D.Impulse);

        // Actual damage deal
        EnemySlash slash = Instantiate(slashPrefab, transform, false);
        slash.transform.localPosition = Vector3.zero;
        if (isFacingLeft)
        {
            slash.transform.localPosition += Vector3.right * -1f;
        }
        else
        {
            slash.transform.localPosition += Vector3.right * 1f;
            slash.transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    private IEnumerator RunAndAttack()
    {
        inAttack = true;

        anim.SetTrigger("run");
        while (Mathf.Abs(player.transform.position.x - transform.position.x) > meleeRange)
        {
            if (isFacingLeft)
                rb.velocity = new Vector2(-runSpeed, 0f);
            else
                rb.velocity = new Vector2(runSpeed, 0f);
            yield return null;
        }

        anim.SetTrigger("attack");
        anim.ResetTrigger("run");
        yield return null;
    }

    private IEnumerator ChargeController()
    {
        inAttack = true;
        anim.SetInteger("chargeState", 1);
        // Prepare to charge
        float prepChargeTimer = 0f;
        while (prepChargeTimer < chargePrepTime)
        {
            prepChargeTimer += Time.deltaTime;
            FaceTowardPlayer();
            yield return null;
        }

        // Charge toward the opposite wall
        anim.SetInteger("chargeState", 2);
        bool collideWall = false;
        float chargeTimer = 0f;
        float chargeSpeed = 0.1f;
        if (isFacingLeft)
            wallChecker.localPosition = new Vector3(-1.2f, 0f);
        else
            wallChecker.localPosition = new Vector3(1.2f, 0f);

        while (!collideWall && chargeTimer < 5f)
        {
            chargeTimer += Time.deltaTime; // This for prevent bug that Batter stuck in charge
            chargeSpeed += chargeAccel;
            chargeSpeed = Mathf.Clamp(chargeSpeed, 0.1f, maxChargeSpeed);
            if (isFacingLeft)
                rb.velocity = new Vector2(-chargeSpeed, 0f);
            else
                rb.velocity = new Vector2(chargeSpeed, 0f);

            if (Physics2D.OverlapCircle(wallChecker.position, 1f, wallMask))
            {
                collideWall = true;
                CinemachineShake.instance.ShakeCamera(5f, 0.5f);
            }

            yield return null;
        }

        // Recovery time
        rb.velocity = Vector2.zero;
        yield return new WaitForSeconds(0.5f);
        anim.SetInteger("chargeState", 0);
        yield return new WaitForSeconds(0.5f);

        // Finish
        inAttack = false;
        yield return null;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(wallChecker.position, 1f);
    }
}