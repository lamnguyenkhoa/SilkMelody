using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    #region Variables

    [Header("Components")]
    public Animator anim;
    public Rigidbody2D rb;
    private PlayerSoundEffect soundEffect;
    public SpriteRenderer sprite;
    public PlayerSlash slashPrefab;
    public PlayerDashSlash dashSlashPrefab;
    public PlayerDashSlash pogoSlashPrefab;
    public ParticleSystem dustPE;
    public Transform groundCheck;
    public Collider2D hurtBox;
    public PlayerStatSO playerStat;

    [Header("Movement")]
    [SerializeField] private int dashCount;
    [SerializeField] private float verInput;
    [SerializeField] private float horInput;
    private float jumpTimer;
    public float maxJumpTime = 0.5f;
    [HideInInspector] public float originalGravityScale;
    private float airTime;
    [SerializeField] private bool isGrounded;
    public LayerMask groundMask;
    private Vector3 groundBox;
    private float coyoteAirTimer;
    private float coyoteTime = 0.2f;

    [Header("LedgeGrab")]
    public bool canLedgeGrab = true;
    [SerializeField] private Vector2 redOffset, redSize, greenOffset, greenSize;
    private bool redBox, greenBox;

    [Header("Combat")]
    public int disableControlCounter = 0;
    /**
     * Using integer instead of bool allow us to easy keep tracking of how many effect are
    * disabling the player control. For example, player get stunned and paralyzed meaning
    * the counter = 2. If one effect come off first, the counter is 1 so they still
    * cant move. If we use bool the player will be able to move after 1 effect wear off.
    */

    public bool inIFrame = false;
    private float attackTimer;
    public Transform dashSlashPos;
    public Transform pogoSlashPos;
    public Vector2 dashRecoil;

    [Header("Status Effects")]
    [SerializeField] private bool isParalyzed;
    [SerializeField] private bool isSlowed;

    [Header("DebugWatch")]
    [SerializeField] private bool inAttack;
    [SerializeField] private bool isHurt;
    [SerializeField] private bool isDashing;
    [SerializeField] private bool isFacingLeft;
    [SerializeField] private bool isJumping;
    [SerializeField] private bool isLedgeGrabbing;
    [SerializeField] private bool isDead;

    [Header("Misc")]
    [SerializeField] private State state = State.idle;
    public bool resting;
    private Coroutine dashCoroutine;

    private enum State
    { idle, running, jumping, falling, hurt, dashing, ledgeGrabbing }
    public enum StatusEffect
    { paralyzed, slowed };

    public static Player instance;

    #endregion Variables

    #region Unity Callbacks

    private void Awake()
    {
        if (!instance)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
            Destroy(this.gameObject);
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        soundEffect = GetComponent<PlayerSoundEffect>();
        originalGravityScale = rb.gravityScale;
        dashCount = playerStat.maxDashCount;
        groundBox = hurtBox.bounds.size;
        groundBox.x -= 0.05f;
        groundBox.y = 0.1f;
    }

    private void Update()
    {
        CheckGrounded();

        if (!inAttack)
            attackTimer += Time.deltaTime;

        if (!resting && disableControlCounter == 0 && !isDashing && !isParalyzed)
        {
            HandleMovement();

            HandleJump();

            HandleAttack();

            HandleDash();

            HandleLedgeGrab();

            HandleCoyoteTime();

            HandleDashAttack();
        }

        AnimationControl();

        // Prevent bug
        Mathf.Clamp(playerStat.currentHp, 0, playerStat.maxHp);

        LandingDust();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Enemy enemy = collision.transform.GetComponent<Enemy>();
        if (enemy)
        {
            if (!enemy.noContactDamage)
            {
                Vector2 knockbackDir = (Vector2)(transform.position - enemy.transform.position).normalized;
                Damaged(enemy.damage, knockbackDir);
            }
        }
    }

    #endregion Unity Callbacks

    #region Main Functions

    private void CheckGrounded()
    {
        if (Physics2D.OverlapBox(groundCheck.position, groundBox, 0f, groundMask))
            isGrounded = true;
        else
            isGrounded = false;
    }

    private void HandleMovement()
    {
        verInput = Input.GetAxisRaw("Vertical");
        horInput = Input.GetAxis("Horizontal");

        // Move left
        if (horInput < 0 && !inAttack)
        {
            isFacingLeft = true;
            Flip();
        }
        // Move right
        else if (horInput > 0 && !inAttack)
        {
            isFacingLeft = false;
            Flip();
        }
        float speedModifier;
        if (!isSlowed)
            speedModifier = 1;
        else
            speedModifier = 0.5f;
        rb.velocity = new Vector2(horInput * playerStat.moveSpeed * speedModifier, rb.velocity.y);
    }

    private void HandleJump()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            if (isGrounded || isLedgeGrabbing || coyoteAirTimer <= coyoteTime)
            {
                jumpTimer = maxJumpTime;
                isJumping = true;
                rb.velocity = new Vector2(rb.velocity.x, playerStat.jumpForce);
                dustPE.Play();
            }
        }
        if (Input.GetKey(KeyCode.X) && isJumping)
        {
            if (jumpTimer > 0)
            {
                rb.velocity = new Vector2(rb.velocity.x, playerStat.jumpForce);
                jumpTimer -= Time.deltaTime;
            }
            else
            {
                isJumping = false;
            }
        }
        if (Input.GetKeyUp(KeyCode.X) && isJumping)
        {
            isJumping = false;
            rb.velocity = new Vector2(rb.velocity.x, 0f);
        }
    }

    private void HandleDash()
    {
        if (Input.GetKeyDown(KeyCode.Z) && !isDashing && dashCount > 0)
        {
            dashCount--;
            dustPE.Play();
            Vector2 dashDirection;
            if (isFacingLeft)
                dashDirection = new Vector2(-1f, 0f);
            else
                dashDirection = new Vector2(1f, 0f);

            if (dashCoroutine != null)
                StopCoroutine(dashCoroutine);
            dashCoroutine = StartCoroutine(Dash(false, dashDirection));
        }
    }

    private void HandleDashAttack()
    {
        if (Input.GetKeyDown(KeyCode.V) && !isDashing && !inAttack && dashCount > 0)
        {
            dashCount--;
            dustPE.Play();
            Vector2 dashDirection;
            if (isFacingLeft)
                dashDirection = new Vector2(-1f, 0f);
            else
                dashDirection = new Vector2(1f, 0f);

            if (dashCoroutine != null)
                StopCoroutine(dashCoroutine);
            dashCoroutine = StartCoroutine(Dash(true, dashDirection));

            // Attack
            soundEffect.PlayAttackSound();
            anim.SetBool("dashAttack", true);
            PlayerDashSlash dashSlash = Instantiate(dashSlashPrefab, dashSlashPos, false);
            dashSlash.transform.localPosition = Vector3.zero;
            dashSlash.disappearTime = playerStat.dashTime;
            dashSlash.player = this;
        }
    }

    private void PogoAttack()
    {
        dustPE.Play();
        Vector2 dashDirection;
        if (isFacingLeft)
            dashDirection = new Vector2(-1f, -1f);
        else
            dashDirection = new Vector2(1f, -1f);

        if (dashCoroutine != null)
            StopCoroutine(dashCoroutine);
        dashCoroutine = StartCoroutine(Dash(true, dashDirection));

        // Attack
        anim.SetBool("pogoAttack", true);
        PlayerDashSlash pogoSlash = Instantiate(pogoSlashPrefab, pogoSlashPos, false);
        pogoSlash.transform.localPosition = Vector3.zero;
        pogoSlash.disappearTime = playerStat.dashTime;
        pogoSlash.player = this;
    }

    private void HandleAttack()
    {
        if (Input.GetKeyDown(KeyCode.C) && attackTimer > playerStat.attackCooldown && !inAttack)
        {
            // Up attack
            if (verInput >= 0.1f)
            {
                anim.SetTrigger("attackUp");
                AttackDealDamage(true);
            }
            // Down attack
            else if (verInput <= -0.1f)
            {
                PogoAttack();
            }
            // Normal attack
            else
            {
                anim.SetTrigger("attack");
                AttackDealDamage(false);
            }

            soundEffect.PlayAttackSound();
            attackTimer = 0f;
        }
    }

    private void HandleLedgeGrab()
    {
        if (isDashing)
            return;

        greenBox = Physics2D.OverlapBox(new Vector2(transform.position.x + (greenOffset.x * transform.localScale.x), transform.position.y + greenOffset.y),
            new Vector2(greenSize.x, greenSize.y), 0f, groundMask);
        redBox = Physics2D.OverlapBox(new Vector2(transform.position.x + (redOffset.x * transform.localScale.x), transform.position.y + redOffset.y),
            new Vector2(redSize.x, redSize.y), 0f, groundMask);

        if (greenBox && !redBox && !isJumping && !isGrounded)
            isLedgeGrabbing = true;
        else
            isLedgeGrabbing = false;

        if (isLedgeGrabbing)
        {
            rb.velocity = Vector2.zero;
            rb.gravityScale = 0f;
            dashCount = playerStat.maxDashCount;
        }
        else
        {
            rb.gravityScale = originalGravityScale;
        }
    }

    private void HandleCoyoteTime()
    {
        if (!isGrounded && !isLedgeGrabbing)
        {
            coyoteAirTimer += Time.deltaTime;
        }
        else
        {
            coyoteAirTimer = 0f;
        }
    }

    /// <summary>
    /// Create dust particle when player land on the ground after falling
    /// from a sufficient height.
    /// </summary>
    private void LandingDust()
    {
        if (!isGrounded)
        {
            airTime += Time.deltaTime;
        }
        else
        {
            if (airTime > 0.2f)
                dustPE.Play();
            airTime = 0f;
            dashCount = playerStat.maxDashCount;
        }
    }

    private void AnimationControl()
    {
        if (isDead || isHurt || isParalyzed)
        {
            state = State.hurt;
        }
        else if (isDashing)
        {
            state = State.dashing;
        }
        else if (rb.velocity.y > 0.1f)
        {
            state = State.jumping;
        }
        else if (rb.velocity.y < -0.1f)
        {
            state = State.falling;
        }
        else if (Mathf.Abs(rb.velocity.x) > 0.1f)
        {
            state = State.running;
        }
        else if (isLedgeGrabbing)
        {
            state = State.ledgeGrabbing;
        }
        else
        {
            state = State.idle;
        }

        anim.SetInteger("state", (int)state);
    }

    public void Damaged(int amount, Vector3 knockbackDir)
    {
        if (inIFrame) return;
        if (amount == 0)
            Debug.Log("This attack deal 0 damage!");
        soundEffect.PlayDamagedSound();
        playerStat.currentHp -= amount;
        playerStat.currentHp = Mathf.Clamp(playerStat.currentHp, 0, playerStat.maxHp);
        rb.gravityScale = originalGravityScale;
        isDashing = false;
        inAttack = false;
        anim.SetBool("dashAttack", false);
        anim.SetBool("pogoAttack", false);
        if (playerStat.currentHp <= 0)
        {
            StartCoroutine(Death());
        }
        else
        {
            StartCoroutine(DamagedFreezeTime(amount));
            StartCoroutine(IFrame());
            StartCoroutine(Stunned());
            rb.AddForce(knockbackDir * 5f + Vector3.up * 5f, ForceMode2D.Impulse);
        }
    }

    #endregion Main Functions

    #region Sub Functions

    private void Flip()
    {
        if (isFacingLeft)
        {
            if (transform.localScale.x != -1)
            {
                transform.localScale = new Vector2(-1, 1);
                if (isGrounded)
                    dustPE.Play();
            }
        }
        else
        {
            if (transform.localScale.x != 1)
            {
                transform.localScale = new Vector2(1, 1);
                if (isGrounded)
                    dustPE.Play();
            }
        }
    }

    public void InflictedStatusEffect(StatusEffect status)
    {
        switch (status)
        {
            case StatusEffect.paralyzed:
                StartCoroutine(GotParalyzed());
                break;

            case StatusEffect.slowed:
                StartCoroutine(GotSlowed());
                break;
        }
    }

    public void AttackDealDamage(bool isUpAttack)
    {
        PlayerSlash slash = Instantiate(slashPrefab);
        slash.transform.position = transform.position;
        if (isUpAttack)
        {
            slash.transform.position = transform.position + Vector3.up * 0.5f;
            slash.transform.rotation = Quaternion.Euler(0, 0, 90);
            if (isFacingLeft)
                slash.transform.localScale = new Vector3(1, -1, 1);
        }
        else
        {
            if (isFacingLeft)
            {
                slash.transform.position += Vector3.right * -0.5f;
                slash.transform.localScale = new Vector3(-1, 1, 1);
            }
            else
                slash.transform.position += Vector3.right * 0.5f;
        }
        slash.player = this.transform;
        slash.damage = playerStat.damage;
        slash.knockbackPower = playerStat.enemyKnockbackPower;
    }

    public void AttackRecoil()
    {
        if (dashCoroutine != null)
            StopCoroutine(dashCoroutine);
        isDashing = false;
        rb.gravityScale = originalGravityScale;
        inAttack = false;
        anim.SetBool("dashAttack", false);
        anim.SetBool("pogoAttack", false);
        rb.velocity = Vector2.zero;
        rb.AddForce(dashRecoil, ForceMode2D.Impulse);
    }

    public void DashAttackTouchGround()
    {
        if (dashCoroutine != null)
            StopCoroutine(dashCoroutine);
        isDashing = false;
        rb.gravityScale = originalGravityScale;
        inAttack = false;
        anim.SetBool("dashAttack", false);
        anim.SetBool("pogoAttack", false);
        rb.velocity = Vector2.zero;
    }

    public void RestChairRecovery()
    {
        StopAllCoroutines();
        playerStat.currentHp = playerStat.maxHp;
        isDashing = false;
        inAttack = false;
        anim.SetBool("dashAttack", false);
        anim.SetBool("pogoAttack", false);
    }

    #endregion Sub Functions

    #region Animation Events

    public void BeginAttackAnim()
    {
        inAttack = true;
    }

    public void EndAttackAnim()
    {
        inAttack = false;
    }

    #endregion Animation Events

    #region Coroutines

    private IEnumerator Dash(bool alsoAttack, Vector2 dashDirection)
    {
        isDashing = true;
        if (alsoAttack)
            inAttack = true;

        rb.gravityScale = 0f;
        rb.velocity = Vector2.zero;
        rb.AddForce(dashDirection * playerStat.dashForce, ForceMode2D.Impulse);

        yield return new WaitForSeconds(playerStat.dashTime);
        isDashing = false;
        rb.gravityScale = originalGravityScale;
        if (alsoAttack)
        {
            inAttack = false;
            anim.SetBool("dashAttack", false);
            anim.SetBool("pogoAttack", false);
        }
    }

    private IEnumerator IFrame()
    {
        int playerLayerId = LayerMask.NameToLayer("Player");
        int EnemyLayerId = LayerMask.NameToLayer("Enemy");
        int EnemyAttackLayerId = LayerMask.NameToLayer("EnemyAttack");

        inIFrame = true;
        Physics2D.IgnoreLayerCollision(playerLayerId, EnemyLayerId, true);
        Physics2D.IgnoreLayerCollision(playerLayerId, EnemyAttackLayerId, true);

        Color originalColor = sprite.color;
        Color iFrameColor = sprite.color;
        iFrameColor.a = 0.5f;
        sprite.color = iFrameColor;

        yield return new WaitForSeconds(playerStat.iFrameTime);

        inIFrame = false;
        Physics2D.IgnoreLayerCollision(playerLayerId, EnemyLayerId, false);
        Physics2D.IgnoreLayerCollision(playerLayerId, EnemyAttackLayerId, false);
        sprite.color = originalColor;
    }

    private IEnumerator Stunned()
    {
        isHurt = true;
        isParalyzed = false;
        disableControlCounter += 1;
        rb.velocity = Vector2.zero;
        yield return new WaitForSeconds(playerStat.stunTime);
        disableControlCounter -= 1;
        isHurt = false;
        inAttack = false;
    }

    private IEnumerator GotParalyzed()
    {
        rb.velocity = Vector2.zero;
        isParalyzed = true;
        yield return new WaitForSeconds(1f);
        isParalyzed = false;
    }

    private IEnumerator GotSlowed()
    {
        isSlowed = true;
        yield return new WaitForSeconds(2f);
        isSlowed = false;
    }

    private IEnumerator DamagedFreezeTime(int damageAmount)
    {
        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(0.35f * damageAmount);
        Time.timeScale = 1f;
    }

    private IEnumerator Death()
    {
        isDead = true;
        disableControlCounter += 1;
        rb.gravityScale = 0f;
        rb.velocity = Vector2.zero;
        LevelLoader.instance.Respawn();
        yield return new WaitForSeconds(LevelLoader.instance.transitionTime);
        isDead = false;
        playerStat.currentHp = playerStat.maxHp;
        disableControlCounter -= 1;
    }

    #endregion Coroutines

    #region Misc

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireCube(groundCheck.position, groundBox);
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(new Vector2(transform.position.x + (greenOffset.x * transform.localScale.x), transform.position.y + greenOffset.y),
            new Vector2(greenSize.x, greenSize.y));
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(new Vector2(transform.position.x + (redOffset.x * transform.localScale.x), transform.position.y + redOffset.y),
            new Vector2(redSize.x, redSize.y));
    }

    #endregion Misc
}