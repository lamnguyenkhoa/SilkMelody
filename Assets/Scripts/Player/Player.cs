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
    public bool disableControl = false;
    public bool inIFrame = false;

    private Transform slashPos;
    public PlayerSlash slashPrefab;

    // FSM
    private enum State
    { idle, running, jumping, falling, hurt }
    [SerializeField] private State state = State.idle;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        slashPos = transform.Find("SlashSpawnPos");
        sprite = GetComponent<SpriteRenderer>();
        soundEffect = GetComponent<PlayerSoundEffect>();
    }

    private void Update()
    {
        if (!disableControl && !inAttack)
        {
            verInput = Input.GetAxis("Vertical");
            horInput = Input.GetAxis("Horizontal");

            // Move left
            if (horInput < 0)
            {
                Flip(true);
            }
            // Move right
            else if (horInput > 0)
            {
                Flip(false);
            }
            rb.velocity = new Vector2(horInput * playerStat.moveSpeed, rb.velocity.y);

            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.X))
            {
                // If on ground
                if (Mathf.Abs(rb.velocity.y) < 0.01f)
                {
                    Jump();
                }
            }

            if (Input.GetKeyDown(KeyCode.C))
            {
                Attack();
            }
        }

        AnimationControl();

        // Prevent bug
        Mathf.Clamp(playerStat.currentHp, 0, playerStat.maxHp);
    }

    private void AnimationControl()
    {
        if (isHurt)
        {
            state = State.hurt;
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
        rb.velocity = Vector2.zero;
        rb.gravityScale = 0f;
    }

    public void AttackDealDamage()
    {
        PlayerSlash slash = Instantiate(slashPrefab, slashPos);
        slash.player = this.transform;
        slash.transform.localPosition = Vector3.zero;
        slash.damage = playerStat.damage;
        slash.knockbackPower = playerStat.enemyKnockbackPower;
        slash.transform.parent = null;
    }

    public void EndAttackAnim()
    {
        inAttack = false;
        rb.gravityScale = 3f;
    }

    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, playerStat.jumpForce);
        // Play some dust particle when jump
    }

    // Flip the character sprite horizontally
    private void Flip(bool facingLeft)
    {
        if (facingLeft)
        {
            if (transform.localScale.x != -1)
            {
                transform.localScale = new Vector2(-1, 1);
                //SwitchDirectionDust();
            }
        }
        else
        {
            if (transform.localScale.x != 1)
            {
                transform.localScale = new Vector2(1, 1);
                //SwitchDirectionDust();
            }
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
}