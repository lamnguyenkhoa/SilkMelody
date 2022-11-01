using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class Player : MonoBehaviour
{
    #region Variables

    [Header("Components")]
    public Animator anim;
    public Rigidbody2D rb;
    private PlayerSoundEffect soundEffect;
    public SpriteRenderer sprite;
    public PlayerSlash slashPrefab;
    public PlayerSlash dashSlashPrefab;
    public PlayerSlash pogoSlashPrefab;
    public ParticleSystem dustPE;
    public Transform groundCheck;
    public Collider2D hurtBox;
    public GameObject gossamerPrefab;
    private GameObject gossamerInstance;
    public GameObject silkBurstPrefab;
    private GameObject silkBurstInstance;
    public GameObject hurtPE;
    public GameObject hurtSpark;
    public PlayerData playerStat;

    [Header("Movement")]
    [SerializeField] private int dashCount;
    [SerializeField] private int jumpCount;
    [SerializeField] private float verInput;
    [SerializeField] private float horInput;
    private float jumpTimer;
    public float maxJumpTime = 0.5f;
    [HideInInspector] public float originalGravityScale;
    private float airTime;
    public bool isGrounded;
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
     * Using integer instead of bool allow us to easy keep tracking of how many NON-INTERRUPTABLE
    * effects are disabling the player control. For example, player get stunned and paralyzed
    * meaning the counter = 2. If one effect come off first, the counter is 1 so they still
    * cant move. If we use bool the player will be able to move after 1 effect wear off.
    */

    public bool inIFrame = false;
    private float attackTimer;
    public Transform dashSlashPos;
    public Transform pogoSlashPos;
    public Vector2 dashRecoil;
    private float parryTimer;
    private float toolTimer;

    [Header("SilkAbility")]
    public GameObject silkBindVFXPrefab;

    [Header("Status Effects")]
    [SerializeField] private bool isParalyzed;
    [SerializeField] private bool isSlowed;

    [Header("DebugWatch")]
    [SerializeField] private bool inAttack;
    public bool isHurt;
    [SerializeField] private bool isDashing;
    public bool isFacingLeft;
    [SerializeField] private bool isJumping;
    [SerializeField] private bool isLedgeGrabbing;
    [SerializeField] private bool isDead;
    [SerializeField] private bool inSilkSkill;
    [SerializeField] private bool toolAnimLock;
    [SerializeField] private bool isParrying;
    public bool resting;

    public bool inMenu;

    [Header("Misc")]
    public State state = State.idle;
    private Coroutine dashCoroutine;
    private Coroutine parryCoroutine;
    public Material flashMat;
    private Material originalMaterial;
    public bool disableInventoryMenu;

    public enum State
    { idle, running, jumping, falling, hurt, dashing, ledgeGrabbing }
    public enum StatusEffect
    { none, paralyzed, slowed, drained };

    public static Player instance;
    public InputMaster inputMaster;

    #endregion Variables

    #region Unity Callbacks

    private void Awake()
    {
        if (!instance)
        {
            instance = this;
            inputMaster = new InputMaster();
            playerStat = GameMaster.instance.playerData;
            DontDestroyOnLoad(this.gameObject);
        }
        else
            Destroy(this.gameObject);
    }

    private void OnEnable()
    {
        if (inputMaster != null)
            inputMaster.Enable();
    }

    private void OnDisable()
    {
        if (inputMaster != null)
            inputMaster.Disable();
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        soundEffect = GetComponent<PlayerSoundEffect>();
        originalGravityScale = rb.gravityScale;
        dashCount = playerStat.maxDash;
        jumpCount = playerStat.extraJump;
        groundBox = hurtBox.bounds.size;
        groundBox.x -= 0.05f;
        groundBox.y = 0.1f;
        originalMaterial = sprite.material;
    }

    private void Update()
    {
        CheckGrounded();

        if (!inAttack)
            attackTimer += Time.deltaTime;

        if (!isParrying)
            parryTimer += Time.deltaTime;

        toolTimer += Time.deltaTime;

        if (!resting && disableControlCounter == 0 && !isDashing && !isParalyzed && !inSilkSkill && !toolAnimLock && !isParrying && !inMenu)
        {
            HandleMovement();

            HandleJump();

            HandleAttack();

            HandleParry();

            HandleDash();

            if (canLedgeGrab)
                HandleLedgeGrab();

            HandleDashAttack();

            HandleCoyoteTime();

            HandleSilkSkill();

            HandleToolUsage();
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
                Damaged(1, knockbackDir);
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
        Vector2 direction = inputMaster.Gameplay.Movement.ReadValue<Vector2>();
        verInput = direction.y;
        horInput = direction.x;

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
        InputAction jumpAction = inputMaster.Gameplay.Jump;
        if (jumpAction.WasPressedThisFrame())
        {
            // Jump from ground
            if (isGrounded || isLedgeGrabbing || coyoteAirTimer <= coyoteTime)
            {
                Jump();
            }
            else if (jumpCount > 0)
            {
                Jump();
                jumpCount--;
            }
        }
        if (jumpAction.IsPressed() && isJumping)
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
        if (jumpAction.WasReleasedThisFrame() && isJumping)
        {
            jumpTimer = 0f;
            isJumping = false;
            rb.velocity = new Vector2(rb.velocity.x, 0f);
            coyoteAirTimer = coyoteTime + 1f; // Prevent coyote double jump bug
        }
    }

    private void HandleDash()
    {
        InputAction dashAction = inputMaster.Gameplay.Dash;
        if (dashAction.WasPressedThisFrame() && !isDashing && dashCount > 0)
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
        InputAction dashAttackAction = inputMaster.Gameplay.DashAttack;
        if (dashAttackAction.WasPressedThisFrame() && !isDashing && !inAttack && dashCount > 0)
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
            soundEffect.PlaySoundEffect(PlayerSoundEffect.SoundEnum.attack);
            anim.SetBool("dashAttack", true);
            PlayerSlash dashSlash = Instantiate(dashSlashPrefab, dashSlashPos, false);
            dashSlash.transform.localPosition = Vector3.zero;
            dashSlash.disappearTime = playerStat.dashTime;
            dashSlash.player = this;
        }
    }

    private void HandleAttack()
    {
        InputAction attackAction = inputMaster.Gameplay.Attack;
        if (attackAction.WasPressedThisFrame() && attackTimer > playerStat.attackCooldown && !inAttack)
        {
            // Up attack
            if (verInput >= 0.1f)
            {
                anim.SetTrigger("attackUp");
                AttackDealDamage(true);
            }
            // Down attack
            else if (verInput <= -0.1f && !isGrounded)
            {
                PogoAttack();
            }
            // Normal attack
            else
            {
                anim.SetTrigger("attack");
                AttackDealDamage(false);
            }

            soundEffect.PlaySoundEffect(PlayerSoundEffect.SoundEnum.attack);
            attackTimer = 0f;
        }
    }

    private void HandleParry()
    {
        InputAction parryAction = inputMaster.Gameplay.Parry;
        if (parryAction.WasPressedThisFrame() && !inAttack && isGrounded)
        {
            if (parryTimer >= playerStat.parryCooldown)
            {
                parryTimer = 0f;
                rb.velocity = Vector2.zero;
                parryCoroutine = StartCoroutine(Parrying());
            }
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
            dashCount = playerStat.maxDash;
            jumpCount = playerStat.extraJump;
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
            {
                dustPE.Play();
                soundEffect.PlaySoundEffect(PlayerSoundEffect.SoundEnum.landing);
            }
            airTime = 0f;
            dashCount = playerStat.maxDash;
            jumpCount = playerStat.extraJump;
        }
    }

    /// <summary>
    /// Mainly used for state or looped animation
    /// </summary>
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

    /// <summary>
    /// Amount must be positive for dealing damage.
    /// </summary>
    /// <param name="amount"></param>
    /// <param name="knockbackDir"></param>
    public void Damaged(int amount, Vector3 knockbackDir)
    {
        int originalAmount = amount;
        if (inIFrame || isDead) return;
        if (isParrying)
        {
            if (parryCoroutine != null)
                StopCoroutine(parryCoroutine);
            StartCoroutine(ParryRipose());
            return;
        }

        // Disable all active silk skills
        if (gossamerInstance != null)
            Destroy(gossamerInstance);
        if (silkBurstInstance != null)
            Destroy(silkBurstInstance);

        if (amount == 0)
            Debug.Log("This attack deal 0 damage!");

        // Damage calculation
        soundEffect.PlaySoundEffect(PlayerSoundEffect.SoundEnum.damaged);
        if (amount <= playerStat.lifebloodHp) // Lifeblood hp absorb some damage
        {
            playerStat.lifebloodHp -= amount; // Should not reach 0
            if (playerStat.lifebloodHp < 0)
            {
                playerStat.lifebloodHp = 0;
                Debug.LogWarning("Lifeblood go negative! This should not happened");
            }
            amount = 0;
        }
        else
        {
            amount -= playerStat.lifebloodHp;
            playerStat.lifebloodHp = 0;
        }
        playerStat.currentHp -= amount;
        playerStat.currentHp = Mathf.Clamp(playerStat.currentHp, 0, playerStat.maxHp);

        // Damaged after effect
        rb.gravityScale = originalGravityScale;
        isDashing = false;
        inAttack = false;
        isParalyzed = false;
        inSilkSkill = false;
        toolAnimLock = false;
        isParrying = false;
        sprite.material = originalMaterial;
        anim.SetBool("dashAttack", false);
        anim.SetBool("pogoAttack", false);

        if (GameObject.Find("Global Volume"))
        {
            GameObject.Find("Global Volume").GetComponent<PPVolumeController>().DamagedPPEffect();
        }
        CinemachineShake.instance.ShakeCamera(5f, 0.3f, true);
        float randomRotateValue = Random.Range(-15f, 15f);
        GameObject tmp = Instantiate(hurtSpark, transform.position, Quaternion.identity);
        tmp.transform.localRotation = Quaternion.Euler(0, 0, randomRotateValue);
        Instantiate(hurtPE, transform.position, Quaternion.identity);

        if (playerStat.currentHp <= 0)
        {
            StartCoroutine(Death());
        }
        else
        {
            StartCoroutine(DamagedFreezeTime(originalAmount));
            StartCoroutine(IFrame());
            StartCoroutine(Stunned());
            rb.AddForce(knockbackDir * 5f + Vector3.up * 5f, ForceMode2D.Impulse);
        }
    }

    public void HandleSilkSkill()
    {
        InputAction silkAction = inputMaster.Gameplay.SilkSkill;
        if (silkAction.WasPressedThisFrame() && !inAttack)
        {
            // Gossamer
            if (verInput >= 0.1f && playerStat.currentSilk >= 6)
            {
                playerStat.currentSilk -= 6;
                playerStat.currentSilk = Mathf.Clamp(playerStat.currentSilk, 0, playerStat.maxSilk);
                anim.SetTrigger("gossamer");
            }
            // Silk burst
            else if (verInput <= -0.1f && playerStat.currentSilk >= 4)
            {
                playerStat.currentSilk -= 4;
                playerStat.currentSilk = Mathf.Clamp(playerStat.currentSilk, 0, playerStat.maxSilk);
                anim.SetTrigger("silkBurst");
            }
            // Heal
            else if (playerStat.currentSilk >= 8 && playerStat.currentHp < playerStat.maxHp)
            {
                playerStat.currentSilk -= 8;
                playerStat.currentSilk = Mathf.Clamp(playerStat.currentSilk, 0, playerStat.maxSilk);
                anim.SetTrigger("heal");
            }
        }
    }

    public void HandleToolUsage()
    {
        int nTool = playerStat.equippedRedTools.Count;
        if (nTool == 0)
            return;

        InputAction swapToolRight = inputMaster.Gameplay.SwapToolRight;
        InputAction useTool = inputMaster.Gameplay.UseRedTool;

        if (swapToolRight.WasPressedThisFrame())
        {
            GameMaster.instance.SwapTool(true);
        }

        if (useTool.WasPressedThisFrame() && !inAttack && toolTimer > playerStat.toolCooldown)
        {
            toolTimer = 0f;
            RedTool.ToolName usedTool = playerStat.equippedRedTools[playerStat.selectedRedToolId];
            GetComponent<RedToolController>().UseRedTool(usedTool);
        }
    }

    #endregion Main Functions

    #region Sub Functions

    private void Jump()
    {
        jumpTimer = maxJumpTime;
        isJumping = true;
        rb.velocity = new Vector2(rb.velocity.x, playerStat.jumpForce);
        coyoteAirTimer = coyoteTime + 1f; // Prevent coyote double jump bug
        soundEffect.PlaySoundEffect(PlayerSoundEffect.SoundEnum.jump);
        dustPE.Play();
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
        PlayerSlash pogoSlash = Instantiate(pogoSlashPrefab, pogoSlashPos, false);
        pogoSlash.transform.localPosition = Vector3.zero;
        pogoSlash.disappearTime = playerStat.dashTime;
        pogoSlash.player = this;
    }

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
        if (isParrying)
        {
            return;
        }

        switch (status)
        {
            case StatusEffect.paralyzed:
                StartCoroutine(GotParalyzed());
                break;

            case StatusEffect.slowed:
                StartCoroutine(GotSlowed());
                break;

            case StatusEffect.drained:
                playerStat.currentSilk -= 2;
                playerStat.currentSilk = Mathf.Clamp(playerStat.currentSilk, 0, playerStat.maxSilk);
                break;
        }
    }

    public void AttackDealDamage(bool isUpAttack)
    {
        PlayerSlash slash = Instantiate(slashPrefab, transform, false);
        slash.transform.localPosition = Vector3.zero;
        if (isUpAttack)
        {
            slash.transform.localPosition += Vector3.up * 0.5f;
            slash.transform.localRotation = Quaternion.Euler(0, 0, 90);
        }
        else
        {
            slash.transform.localPosition += Vector3.right * 1f;
        }
        slash.player = this;
        slash.knockbackPower = playerStat.enemyKnockbackPower;
    }

    public void AttackRecoil(bool resetDash)
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
        if (resetDash)
        {
            dashCount = playerStat.maxDash;
            jumpCount = playerStat.extraJump;
        }
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

    public void DisableGameplayControl(bool disable)
    {
        disableInventoryMenu = disable;
        if (inputMaster != null)
        {
            if (disable)
                inputMaster.Gameplay.Disable();
            else
                inputMaster.Gameplay.Enable();
        }
    }

    public void Heal(int amount)
    {
        playerStat.currentHp += amount;
        playerStat.currentHp = Mathf.Clamp(playerStat.currentHp, 0, playerStat.maxHp);
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

    public void BeginToolAnimLock()
    {
        toolAnimLock = true;
        rb.velocity = Vector2.zero;
        rb.gravityScale = 0f;
    }

    public void EndToolAnimLock()
    {
        toolAnimLock = false;
        rb.gravityScale = originalGravityScale;
    }

    public void LifebloodNeedleEffect()
    {
        playerStat.lifebloodHp += 2;
        soundEffect.PlaySoundEffect(PlayerSoundEffect.SoundEnum.syringe);
        StartCoroutine(FlashBlue());
        GameObject effect = GetComponent<RedToolController>().lifebloodNeedlePE;
        // Create a splash of lifeblood effect from the back and slightly upward
        if (isFacingLeft)
            Instantiate(effect, transform.position, Quaternion.LookRotation(new Vector3(1, 0.2f, 0), Vector3.up));
        else
            Instantiate(effect, transform.position, Quaternion.LookRotation(new Vector3(-1, 0.2f, 0), Vector3.up));
    }

    public void BeginHeal()
    {
        soundEffect.PlaySoundEffect(PlayerSoundEffect.SoundEnum.silkbind);
        inSilkSkill = true;
        rb.velocity = Vector2.zero;
        rb.gravityScale = 0f;
        GameObject vfx = Instantiate(silkBindVFXPrefab, transform, false);
        vfx.transform.localPosition = new Vector3(0.25f, 0, 0);
    }

    public void ActualHeal()
    {
        Heal(playerStat.silkHeal);
        StartCoroutine(FlashWhite());
    }

    public void EndHeal()
    {
        inSilkSkill = false;
        rb.gravityScale = originalGravityScale;
    }

    public void BeginGossamer()
    {
        // If player get hit during gossamer, the gossamer object will be destroy prematurely
        inSilkSkill = true;
        rb.velocity = Vector2.zero;
        rb.gravityScale = 0f;
        gossamerInstance = Instantiate(gossamerPrefab, transform, false);
        gossamerInstance.transform.localPosition = new Vector3(0.25f, 0, 0);
        gossamerInstance.GetComponent<Gossamer>().playerPos = transform.position;
    }

    public void BeginSilkBurst()
    {
        inSilkSkill = true;
        rb.velocity = Vector2.zero;
        rb.gravityScale = 0f;
    }

    public void ThrowSilkBurst()
    {
        silkBurstInstance = Instantiate(silkBurstPrefab, transform, false);
        silkBurstInstance.transform.localPosition = new Vector3(4, 0, 0);
        silkBurstInstance.GetComponent<SilkBurst>().playerPos = transform.position;
    }

    public void CatchSilkBurstRecoil()
    {
        if (isFacingLeft)
            rb.AddForce(new Vector2(10f, 2f), ForceMode2D.Impulse);
        else
            rb.AddForce(new Vector2(-10f, 2f), ForceMode2D.Impulse);
    }

    #endregion Animation Events

    #region Coroutines

    private IEnumerator Dash(bool alsoAttack, Vector2 dashDirection)
    {
        soundEffect.PlaySoundEffect(PlayerSoundEffect.SoundEnum.dash);
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
        disableControlCounter += 1;
        rb.velocity = Vector2.zero;
        yield return new WaitForSeconds(playerStat.stunTime);
        disableControlCounter -= 1;
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

    private IEnumerator Death()
    {
        isDead = true;
        disableControlCounter += 1;
        rb.isKinematic = true;
        rb.velocity = Vector2.zero;
        LevelLoader.instance.Respawn();
        yield return new WaitForSeconds(LevelLoader.instance.transitionTime);
        isDead = false;
        playerStat.currentHp = playerStat.maxHp;
        disableControlCounter -= 1;
    }

    private IEnumerator Parrying()
    {
        isParrying = true;
        anim.SetInteger("parryState", 1);
        yield return new WaitForSeconds(playerStat.parryWindow);
        anim.SetInteger("parryState", 0);
        isParrying = false;
    }

    private IEnumerator ParryRipose()
    {
        //Get hit -> freeze time
        anim.SetInteger("parryState", 2);
        soundEffect.PlaySoundEffect(PlayerSoundEffect.SoundEnum.parry);
        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(0.35f);
        Time.timeScale = 1f;

        // I-frame
        inIFrame = true;
        int playerLayerId = LayerMask.NameToLayer("Player");
        int EnemyLayerId = LayerMask.NameToLayer("Enemy");
        int EnemyAttackLayerId = LayerMask.NameToLayer("EnemyAttack");
        Physics2D.IgnoreLayerCollision(playerLayerId, EnemyLayerId, true);
        Physics2D.IgnoreLayerCollision(playerLayerId, EnemyAttackLayerId, true);

        // while dash forward and attack
        soundEffect.PlaySoundEffect(PlayerSoundEffect.SoundEnum.attack);
        anim.SetInteger("parryState", 3);
        PlayerSlash dashSlash = Instantiate(dashSlashPrefab, dashSlashPos, false);
        dashSlash.transform.localPosition = Vector3.zero;
        dashSlash.disappearTime = playerStat.dashTime;
        dashSlash.player = this;
        dashSlash.hasRecoil = false;
        dashSlash.piercing = true;
        dustPE.Play();
        Vector2 dashDirection;
        if (isFacingLeft)
            dashDirection = new Vector2(-1f, 0f);
        else
            dashDirection = new Vector2(1f, 0f);
        isDashing = true;
        inAttack = true;
        rb.gravityScale = 0f;
        rb.velocity = Vector2.zero;
        rb.AddForce(dashDirection * playerStat.dashForce * 1.5f, ForceMode2D.Impulse);

        // Finish dash n attack
        yield return new WaitForSeconds(playerStat.dashTime);
        isDashing = false;
        inAttack = false;
        isParrying = false;
        rb.gravityScale = originalGravityScale;
        anim.SetInteger("parryState", 0);

        // Bonus iFrame time
        yield return new WaitForSeconds(0.1f);

        // Return to normal
        inIFrame = false;
        Physics2D.IgnoreLayerCollision(playerLayerId, EnemyLayerId, false);
        Physics2D.IgnoreLayerCollision(playerLayerId, EnemyAttackLayerId, false);

        // Reset parry cooldown
        parryTimer = playerStat.parryCooldown;
    }

    private IEnumerator FlashWhite()
    {
        sprite.color = new Color(1, 1, 1);
        sprite.material = flashMat;
        yield return new WaitForSeconds(0.25f);
        sprite.material = originalMaterial;
    }

    private IEnumerator FlashBlue()
    {
        sprite.material = flashMat;
        sprite.color = new Color(0, 0.5f, 1);
        yield return new WaitForSeconds(0.15f);
        sprite.material = originalMaterial;
        sprite.color = new Color(1, 1, 1);
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
