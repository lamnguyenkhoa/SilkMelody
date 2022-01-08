using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    #region Variables

    [Header("Components")]
    public Animator anim;
    private Rigidbody2D rb;
    private PlayerSoundEffect soundEffect;
    public SpriteRenderer sprite;
    public PlayerSlash slashPrefab;
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
    private float originalGravityScale;
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
    public bool disableControl = false;
    public bool inIFrame = false;
    private float attackTimer;

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

    [Header("Misc")]
    [SerializeField] private State state = State.idle;
    private enum State
    { idle, running, jumping, falling, hurt, dashing, ledgeGrabbing }
    public enum StatusEffect
    { paralyzed, slowed };

    #endregion Variables

    #region Unity Callbacks

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

        if (!disableControl && !isDashing && !isParalyzed)
        {
            HandleMovement();

            HandleJump();

            HandleAttack();

            HandleDash();

            HandleLedgeGrab();

            HandleCoyoteTime();
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
            StartCoroutine(Dash());
        }
    }

    private void HandleAttack()
    {
        if (Input.GetKeyDown(KeyCode.C) && attackTimer > playerStat.attackCooldown && !inAttack)
        {
            if (verInput >= 0.1f)
            {
                anim.SetTrigger("attackUp");
                AttackDealDamage(true);
            }
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
        if (isHurt || isParalyzed)
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
        StartCoroutine(DamagedFreezeTime(amount));
        StartCoroutine(IFrame());
        StartCoroutine(Stunned());
        rb.AddForce(knockbackDir * 5f + Vector3.up * 5f, ForceMode2D.Impulse);
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

    private IEnumerator Dash()
    {
        isDashing = true;
        rb.gravityScale = 0f;
        rb.velocity = Vector2.zero;
        if (isFacingLeft)
            rb.AddForce(new Vector2(-playerStat.dashForce, 0f), ForceMode2D.Impulse);
        else
            rb.AddForce(new Vector2(playerStat.dashForce, 0f), ForceMode2D.Impulse);

        yield return new WaitForSeconds(playerStat.dashTime);
        isDashing = false;
        rb.gravityScale = originalGravityScale;
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
        disableControl = true;
        rb.velocity = Vector2.zero;
        yield return new WaitForSeconds(playerStat.stunTime);
        disableControl = false;
        isHurt = false;
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