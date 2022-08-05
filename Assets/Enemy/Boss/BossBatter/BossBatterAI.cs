using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** Boss design:
 * Moveset:
 * - Thow and hit a white ball, ranged attack, can be deflected back to Batter
 * - Thow and hit a big red ball, ranged explosive attack, 2 dmg
 * - Normal attack, if player out range then he has a dash
 * - Charge toward player
 * - Jump and slam the ground, create shockwall and falling rock (2 dmg if slammed)
 * - Summon pet to attack, pet shoot laser beam
 * - Below 25% hp, move faster
 * Behaviour:
 * - Since I'm noob, most of his move will be selected randomly after he finished the previous one
 */

public class BossBatterAI : MonoBehaviour
{
    [Header("Components")]
    public EnemySlash slashPrefab;
    private Player player;
    private Animator anim;
    private Transform spriteHolder;
    [HideInInspector] public Enemy stat;
    private Rigidbody2D rb;
    private WorldData worldState;

    [Header("Moveset control")]
    public float beginWait = 1f;
    public float timeBetweenAttack;
    [SerializeField] private bool inAttack;
    private float attackTimer;
    [SerializeField] private bool isFacingLeft;
    public float speedOverdrive = 1f;
    public enum Moveset
    { whiteBall, redBall, attack, charge, jumpSlam, summon }
    public Moveset selectedMove;

    [Header("Normal attack")]
    public float dashForce;
    public float runSpeed;
    public float meleeRange;
    public AudioSource attackSound;

    [Header("Charge")]
    public float chargePrepTime;
    public float chargeAccel;
    public float maxChargeSpeed;
    public LayerMask wallMask;
    public Transform wallChecker;
    public int numBoulderSpawn;
    public GameObject boulderPrefab;
    public Transform boulderSpawnZoneLeft;
    public Transform boulderSpawnZoneRight;
    public AudioSource wallCrashSound;

    [Header("Summon")]
    public BatterPetAI petPrefab;
    public Transform[] spawnSpots; // length of this array is max number of pet
    public int currentPetCounter;
    public float timeBetweenSummon;
    private float summonTimer;
    public AudioSource summonSound;

    [Header("JumpSlam")]
    public Shockwave shockwavePrefab;
    public float shockwaveSpeed;
    public float jumpForce;
    public float slamForce;
    private float originalGravityScale;
    public Transform groundChecker; // can use wallMask to check too
    public AudioSource groundCrashSound;

    [Header("ThrowBall")]
    public Projectile ballPrefab;
    public float hitForce;
    public float throwMinRange; // should larger than meleeRange
    public AudioSource ballHitSound;

    private void Start()
    {
        worldState = GameMaster.instance.worldData;
        stat = GetComponent<Enemy>();
        player = GameObject.Find("Tenroh").GetComponent<Player>();
        spriteHolder = transform.Find("Sprite");
        anim = spriteHolder.GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        originalGravityScale = rb.gravityScale;
        attackTimer = -beginWait;

        // Speed overdrive
        maxChargeSpeed *= speedOverdrive;
        shockwaveSpeed *= speedOverdrive;
        maxChargeSpeed *= speedOverdrive;
        chargeAccel *= speedOverdrive;
        chargePrepTime /= speedOverdrive;
        dashForce *= speedOverdrive;
        runSpeed *= speedOverdrive;
        anim.speed = speedOverdrive;
    }

    private void Update()
    {
        if (stat.isDead)
        {
            StopAllCoroutines();
            worldState.bossDefeated[(int)WorldData.BossEnum.Batter] = true;
            inAttack = false;
            this.enabled = false;
        }

        summonTimer += Time.deltaTime * speedOverdrive;

        if (!inAttack)
        {
            FaceTowardPlayer();

            if (attackTimer < timeBetweenAttack)
            {
                attackTimer += Time.deltaTime * speedOverdrive;
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

                    case Moveset.summon:
                        Summon();
                        break;

                    case Moveset.jumpSlam:
                        JumpSlam();
                        break;

                    case Moveset.whiteBall:
                        ThrowBall();
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

    private void Summon()
    {
        if (summonTimer >= timeBetweenSummon)
        {
            if (currentPetCounter < spawnSpots.Length)
            {
                attackTimer = 0f;
                summonTimer = 0f;
                StartCoroutine(SummonPet());
            }
        }
    }

    private void JumpSlam()
    {
        attackTimer = 0f;
        StartCoroutine(JumpSlamController());
    }

    private void ThrowBall()
    {
        if (Vector3.Distance(player.transform.position, transform.position) > throwMinRange)
        {
            attackTimer = 0f;
            anim.SetTrigger("throw");
        }
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

    public void BeginAttack()
    {
        inAttack = true;
    }

    public void EndAttack()
    {
        inAttack = false;
    }

    public void CreateBallAndHit()
    {
        Vector3 spawnBallPos = new Vector3(-2, 0.3f, 0f);
        if (!isFacingLeft)
            spawnBallPos.x = 2f;

        spawnBallPos = transform.position + spawnBallPos;

        anim.ResetTrigger("throw"); // failsafe to prevent bug
        Projectile newBall = Instantiate(ballPrefab, spawnBallPos, Quaternion.identity);
        Vector2 ballDirection = (player.transform.position - spawnBallPos).normalized;
        newBall.GetComponent<Rigidbody2D>().AddForce(ballDirection * hitForce, ForceMode2D.Impulse);
        ballHitSound.Play();
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
        attackSound.Play();
    }

    private void SpawnBoulder()
    {
        for (int i = 0; i < numBoulderSpawn; i++)
        {
            float randomX = Random.Range(boulderSpawnZoneLeft.position.x, boulderSpawnZoneRight.position.x);
            float randomY = Random.Range(boulderSpawnZoneLeft.position.y, boulderSpawnZoneRight.position.y);

            Instantiate(boulderPrefab, new Vector2(randomX, randomY), Quaternion.identity);
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
            prepChargeTimer += Time.deltaTime * speedOverdrive;
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
            chargeTimer += Time.deltaTime * speedOverdrive; // This for prevent bug that Batter stuck in charge
            chargeSpeed += chargeAccel * Time.deltaTime;
            chargeSpeed = Mathf.Clamp(chargeSpeed, 0.1f, maxChargeSpeed);
            if (isFacingLeft)
                rb.velocity = new Vector2(-chargeSpeed, 0f);
            else
                rb.velocity = new Vector2(chargeSpeed, 0f);

            if (Physics2D.OverlapCircle(wallChecker.position, 1f, wallMask))
            {
                collideWall = true;
                CinemachineShake.instance.ShakeCamera(5f, 0.5f, false);
                wallCrashSound.Play();
                SpawnBoulder();
            }

            yield return null;
        }

        // Recovery time
        rb.velocity = Vector2.zero;
        yield return new WaitForSeconds(0.5f / speedOverdrive);
        anim.SetInteger("chargeState", 0);
        yield return new WaitForSeconds(0.5f / speedOverdrive);

        // Finish
        inAttack = false;
        yield return null;
    }

    private IEnumerator SummonPet()
    {
        inAttack = true;

        anim.SetBool("summoning", true);
        summonSound.Play();
        yield return new WaitForSeconds(0.75f / speedOverdrive);

        // Spawn a pet in the first empty slot
        foreach (Transform spawnSpot in spawnSpots)
        {
            if (spawnSpot.childCount == 0)
            {
                BatterPetAI newPet = Instantiate(petPrefab, spawnSpot, false);
                newPet.transform.localPosition = Vector3.zero;
                newPet.owner = this;
                currentPetCounter++;
                break;
            }
        }

        // Recovery time
        anim.SetBool("summoning", false);
        yield return new WaitForSeconds(0.25f / speedOverdrive);
        inAttack = false;
    }

    private IEnumerator JumpSlamController()
    {
        inAttack = true;

        // Jump
        rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
        anim.SetInteger("slamState", 1);
        // Wait until he stop moving up
        while (rb.velocity.y > 0.1f)
        {
            yield return null;
        }

        // Hover
        rb.gravityScale = 0f;
        rb.velocity = Vector2.zero;
        bool isGrounded = false;
        anim.SetInteger("slamState", 2);
        yield return new WaitForSeconds(0.25f / speedOverdrive);

        // Fall
        rb.gravityScale = originalGravityScale;
        rb.AddForce(new Vector2(0, -slamForce), ForceMode2D.Impulse);
        // Wait until near the ground
        while (!isGrounded)
        {
            isGrounded = Physics2D.OverlapCircle(groundChecker.position, 0.5f, wallMask);
            yield return null;
        }

        // Slam
        anim.SetInteger("slamState", 3);
        CinemachineShake.instance.ShakeCamera(5f, 0.5f, false);
        groundCrashSound.Play();
        // Create shockwave (default sprite direction is right)
        Shockwave shockwaveRight = Instantiate(shockwavePrefab, transform.position + new Vector3(1f, 0f), Quaternion.identity);
        shockwaveRight.kinematicVelocity = new Vector2(shockwaveSpeed, 0f);
        Shockwave shockwaveLeft = Instantiate(shockwavePrefab, transform.position - new Vector3(1f, 0f), Quaternion.identity);
        shockwaveLeft.transform.localScale = new Vector3(-1, 1, 1);
        shockwaveLeft.kinematicVelocity = new Vector2(-shockwaveSpeed, 0f);
        yield return new WaitForSeconds(1f / speedOverdrive);

        // Recovery time
        anim.SetInteger("slamState", 0);
        inAttack = false;
        yield return new WaitForSeconds(0.5f / speedOverdrive);
    }
}
