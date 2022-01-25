using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class PlayerSlash : MonoBehaviour
{
    [HideInInspector] public Player player;
    public Light2D sparkLight;
    public PlayerStatSO playerStat;
    public float knockbackPower = 1f;
    private SpriteRenderer sprite;
    private Color fadeColor;
    private float fadeSpeed = 0f;
    private float fadeSpeedAccel = 100f;
    private float lightOriginalIntensitiy;
    [SerializeField] private AudioSource hitEnemySound;
    private bool impacted = false;
    private Collider2D attackCollider;

    [Header("SpecialAttack")]
    public bool canResetDash;
    [HideInInspector] public bool piercing; // Used for parry ripose
    public bool hasRecoil; // Stop the player after the attack touch an enemy
    public bool isDashAttack;

    public float disappearTime = 0.15f; // Normal attack 0.15s
    private float timer;

    private void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        fadeColor = sprite.color;
        lightOriginalIntensitiy = sparkLight.intensity;
        attackCollider = transform.GetComponent<Collider2D>();
        if (piercing && hasRecoil)
            Debug.Log("Mutually exclusive! Cannot be both!");
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (impacted || timer >= disappearTime)
        {
            if (attackCollider.enabled)
                attackCollider.enabled = false;

            // Fade the sprite
            if (fadeColor.a > 0)
            {
                fadeColor.a -= fadeSpeed * Time.deltaTime;
                fadeColor.a = Mathf.Clamp(fadeColor.a, 0, 1);
                sprite.color = fadeColor;
                fadeSpeed += fadeSpeedAccel * Time.deltaTime;
                sparkLight.intensity = lightOriginalIntensitiy * fadeColor.a;
            }
            else
            {
                Destroy(this.gameObject, 1f);
            }
        }
    }

    public void PlayHitEnemySound()
    {
        float randomVolume = Random.Range(0.8f, 1f);
        float randomPitch = Random.Range(0.7f, 1.3f);

        hitEnemySound.volume = randomVolume;
        hitEnemySound.pitch = randomPitch;
        hitEnemySound.Play();
    }

    private void EnemyImpactEffect()
    {
        sparkLight.enabled = true;
        PlayHitEnemySound();
        impacted = true;
        // If piercing (attack multiple enemy) then no recoil
        if (!piercing && hasRecoil)
            player.AttackRecoil(canResetDash);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Enemy enemy = collision.GetComponent<Enemy>();
        // Failsafe
        if (!enemy && collision.transform.parent != null)
            enemy = collision.transform.parent.GetComponent<Enemy>(); // When collider are put in SpriteHolder

        Projectile projectile = collision.GetComponent<Projectile>();
        DoorSwitch doorSwitch = collision.GetComponent<DoorSwitch>();
        if (enemy)
        {
            // Play effect that only happened ONCE (if player hit enemy) unless attack is piercing
            if (!impacted)
            {
                EnemyImpactEffect();
            }
            // Push enemy backward slightly
            Vector2 knockbackForce = (Vector2)(enemy.transform.position - player.transform.position).normalized * knockbackPower;
            enemy.Damaged(playerStat.damage, knockbackForce);
            playerStat.currentSilk += 1;
            playerStat.currentSilk = Mathf.Clamp(playerStat.currentSilk, 0, playerStat.maxSilk);
            // Play hit/damaged effect on enemy (if there are multiple enemies within attack)
        }
        if (projectile && projectile.gameObject.layer == LayerMask.NameToLayer("EnemyAttack"))
        {
            if (!projectile.unhittable)
            {
                if (!impacted)
                {
                    EnemyImpactEffect();
                }
                Vector2 knockbackForce = (Vector2)(projectile.transform.position - player.transform.position).normalized * knockbackPower;
                projectile.Damaged(playerStat.damage, knockbackForce);
            }
        }
        if (doorSwitch)
        {
            doorSwitch.Activate();
        }
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            if (isDashAttack)
            {
                player.DashAttackTouchGround();
                if (!piercing)
                    impacted = true;
            }
        }
    }
}