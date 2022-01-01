using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    private PlayerSoundEffect soundEffect;
    private SpriteRenderer sprite;
    public float verInput;
    public float horInput;
    public PlayerStatSO playerStat;
    public bool inAttack;
    public bool isHurt;
    public bool isDashing;
    public bool isFacingLeft;
    public bool isJumping;
    private float originalGravityScale;
    private float airTime;
    public bool isGrounded;
    public int dashCount;
    public LayerMask groundLayer;

    public bool disableControl = false;
    public bool inIFrame = false;

    private float attackTimer;

    private Transform slashPos;
    public PlayerSlash slashPrefab;
    public ParticleSystem dustPE;
    public Transform groundCheck;
    public Collider2D hurtBox;
    private Vector3 groundBox;

    public float jumpTimer;
    public float maxJumpTime = 0.5f;

    // FSM
    private enum State
    { idle, running, jumping, falling, hurt, dashing }
    [SerializeField] private State state = State.idle;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        slashPos = transform.Find("SlashSpawnPos");
        sprite = GetComponent<SpriteRenderer>();
        soundEffect = GetComponent<PlayerSoundEffect>();
        originalGravityScale = rb.gravityScale;
        dashCount = playerStat.maxDashCount;
        groundBox = hurtBox.bounds.size;
        groundBox.y = 0.1f;
    }

    private void Update()
    {
        //isGrounded = Mathf.Abs(rb.velocity.y) < 0.01f;
        if (Physics2D.OverlapBox(groundCheck.position, groundBox, 0f, groundLayer))
            isGrounded = true;
        else
            isGrounded = false;

        if (!inAttack)
            attackTimer += Time.deltaTime;

        if (!disableControl && !isDashing)
        {
            verInput = Input.GetAxis("Vertical");
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
            rb.velocity = new Vector2(horInput * playerStat.moveSpeed, rb.velocity.y);

            if (Input.GetKeyDown(KeyCode.X) && isGrounded)
            {
                jumpTimer = maxJumpTime;
                isJumping = true;
                rb.velocity = new Vector2(rb.velocity.x, playerStat.jumpForce);
                dustPE.Play();
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
            if (Input.GetKeyUp(KeyCode.X))
            {
                isJumping = false;
            }

            if (Input.GetKeyDown(KeyCode.C) && attackTimer > playerStat.attackCooldown)
            {
                Attack();
                attackTimer = 0f;
            }

            if (Input.GetKeyDown(KeyCode.Z) && !isDashing && dashCount > 0)
            {
                dashCount--;
                StartCoroutine(Dash());
            }
        }

        AnimationControl();

        // Prevent bug
        Mathf.Clamp(playerStat.currentHp, 0, playerStat.maxHp);

        // Landing dust
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
        if (isHurt)
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

    private void Attack()
    {
        anim.SetTrigger("attack");
        soundEffect.PlayAttackSound();
    }

    public void BeginAttackAnim()
    {
        inAttack = true;
    }

    public void EndAttackAnim()
    {
        inAttack = false;
    }

    public void AttackDealDamage()
    {
        PlayerSlash slash = Instantiate(slashPrefab, slashPos);
        slash.player = this.transform;
        slash.transform.localPosition = Vector3.zero;
        slash.damage = playerStat.damage;
        slash.knockbackPower = playerStat.enemyKnockbackPower;
    }

    // Flip the character sprite horizontally
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
        disableControl = true;
        rb.velocity = Vector2.zero;
        yield return new WaitForSeconds(playerStat.stunTime);
        disableControl = false;
        isHurt = false;
    }

    private IEnumerator DamagedFreezeTime(int damageAmount)
    {
        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(0.35f * damageAmount);
        Time.timeScale = 1f;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Enemy enemy = collision.transform.GetComponent<Enemy>();
        if (enemy)
        {
            Vector2 knockbackDir = (Vector2)(transform.position - enemy.transform.position).normalized;
            Damaged(enemy.damage, knockbackDir);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireCube(groundCheck.position, groundBox);
    }
}