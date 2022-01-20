using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class PlayerDashSlash : MonoBehaviour
{
    [HideInInspector] public Player player;
    public Light2D sparkLight;
    public PlayerStatSO playerStat;
    public float knockbackPower = 1f;
    private SpriteRenderer sprite;
    private Color fadeColor;
    private float fadeSpeed = 0f;
    public float fadeSpeedAccel = 0.1f;
    private float lightOriginalIntensitiy;
    [SerializeField] private AudioSource hitEnemySound;
    private bool playedImpact = false;
    private Collider2D attackCollider;
    public bool canResetDash;
    public bool piercing;

    public float disappearTime;
    private float timer;

    private void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        fadeColor = sprite.color;
        lightOriginalIntensitiy = sparkLight.intensity;
        attackCollider = transform.GetComponent<Collider2D>();
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (playedImpact || timer >= disappearTime)
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Enemy enemy = collision.GetComponent<Enemy>();
        Projectile projectile = collision.GetComponent<Projectile>();
        if (enemy)
        {
            // Play effect that only happened ONCE (if player hit enemy) unless attack is piercing
            if (!playedImpact)
            {
                sparkLight.enabled = true;
                PlayHitEnemySound();
                playedImpact = true;
                // If piercing (attack multiple enemy) then no recoil
                if (!piercing)
                    player.AttackRecoil(canResetDash);
            }
            // Push enemy backward slightly
            Vector2 knockbackForce = (Vector2)(enemy.transform.position - player.transform.position).normalized * knockbackPower;
            enemy.Damaged(playerStat.damage, knockbackForce);
            playerStat.currentSilk += 1;
            playerStat.currentSilk = Mathf.Clamp(playerStat.currentSilk, 0, playerStat.maxSilk);
            // Play hit/damaged effect on enemy (if there are multiple enemies within attack)
        }
        else if (projectile && projectile.gameObject.layer == LayerMask.NameToLayer("EnemyAttack"))
        {
            if (!playedImpact)
            {
                sparkLight.enabled = true;
                PlayHitEnemySound();
                playedImpact = true;
                if (!piercing)
                    player.AttackRecoil(canResetDash);
            }
            Vector2 knockbackForce = (Vector2)(projectile.transform.position - player.transform.position).normalized * knockbackPower;
            projectile.Damaged(playerStat.damage, knockbackForce);
            playerStat.currentSilk += 1;
            playerStat.currentSilk = Mathf.Clamp(playerStat.currentSilk, 0, playerStat.maxSilk);
        }
        else
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
            {
                player.DashAttackTouchGround();
                if (!piercing)
                    playedImpact = true;
            }
        }
    }
}