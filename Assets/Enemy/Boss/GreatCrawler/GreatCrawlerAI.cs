using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** Great Crawler Boss design:
 * Moveset:
 * - Bite attack, if player out range then it run toward
 * - Charge toward the player
 * - Jump and slam the ground, create shockwall and falling rock (2 dmg if slammed)
 * - Below 25% hp, enraged -> +25% animation speed
 * Behaviour:
 * - Move will be selected randomly after it finished the previous one
 */

public class GreatCrawlerAI : MonoBehaviour
{
    [Header("Components")]
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
    { attack, charge, jumpSlam }
    public Moveset selectedMove;
    [SerializeField] private bool isEnraged;

    [Header("Bite attack")]
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

    [Header("JumpSlam")]
    public Shockwave shockwavePrefab;
    public float shockwaveSpeed;
    public float jumpForce;
    public float slamForce;
    private float originalGravityScale;
    public Transform groundChecker; // can use wallMask to check too
    public AudioSource groundCrashSound;

    // Start is called before the first frame update
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
        UpdateSpeedOverride();
    }

    // Update is called once per frame
    private void Update()
    {
        if (stat.isDead)
        {
            StopAllCoroutines();
            worldState.bossDefeated[(int)WorldData.BossEnum.GreatCrawler] = true;
            inAttack = false;
            this.enabled = false;
        }

        // Low hp, enraged
        if ((float)stat.currentHp / stat.maxHp <= 0.25f && !isEnraged)
        {
            isEnraged = true;
            speedOverdrive += 0.25f;
            UpdateSpeedOverride();
            spriteHolder.GetComponent<SpriteRenderer>().color = Color.red;
            //Color enragedColor = new Color(255f, 120f, 120f);
            //spriteHolder.GetComponent<SpriteRenderer>().color = enragedColor;
        }

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

                    case Moveset.jumpSlam:
                        JumpSlam();
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

    private void JumpSlam()
    {
        attackTimer = 0f;
        StartCoroutine(JumpSlamController());
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
        attackSound.Play();
    }

    public void EndAttack()
    {
        inAttack = false;
        attackTimer = 0f;
    }

    public void AttackDealDamage()
    {
        // Short dash toward player just before bite
        if (isFacingLeft)
            rb.AddForce(new Vector2(-dashForce, 10f), ForceMode2D.Impulse);
        else if (!isFacingLeft)
            rb.AddForce(new Vector2(dashForce, 10f), ForceMode2D.Impulse);
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
            chargeTimer += Time.deltaTime * speedOverdrive; // This for prevent bug that Boss stuck in charge
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
        Shockwave shockwaveRight = Instantiate(shockwavePrefab, transform.position + new Vector3(2f, 0f), Quaternion.identity);
        shockwaveRight.kinematicVelocity = new Vector2(shockwaveSpeed, 0f);
        Shockwave shockwaveLeft = Instantiate(shockwavePrefab, transform.position - new Vector3(2f, 0f), Quaternion.identity);
        shockwaveLeft.transform.localScale = new Vector3(-1, 1, 1);
        shockwaveLeft.kinematicVelocity = new Vector2(-shockwaveSpeed, 0f);
        yield return new WaitForSeconds(1f / speedOverdrive);

        // Recovery time
        anim.SetInteger("slamState", 0);
        inAttack = false;
        yield return new WaitForSeconds(0.5f / speedOverdrive);
    }

    private void UpdateSpeedOverride()
    {
        maxChargeSpeed *= speedOverdrive;
        shockwaveSpeed *= speedOverdrive;
        maxChargeSpeed *= speedOverdrive;
        chargeAccel *= speedOverdrive;
        chargePrepTime /= speedOverdrive;
        dashForce *= speedOverdrive;
        runSpeed *= speedOverdrive;
        anim.speed = speedOverdrive;
    }
}
