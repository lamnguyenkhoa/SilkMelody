using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public bool meleeHittable;
    public bool projectileHittable; // If at least 1 projectile has this when 2 projectile contact, they damaged each other
    public int durability = 1;
    private int currentDurability;
    public int damage;
    public bool knockbackAble = true;
    private Rigidbody2D rb;
    public Material flashMat;
    public AudioSource hitSound;
    public AudioSource collideSound;
    private Material originalMat;
    public SpriteRenderer sprite;
    private Color fadeColor;
    private float fadeSpeed = 10f;
    private bool isDead;
    public int maxBounce;
    private int bounceCounter;
    public Player.StatusEffect inflictStatus;

    private void Start()
    {
        currentDurability = durability;
        rb = GetComponent<Rigidbody2D>();
        originalMat = sprite.material;
        fadeColor = sprite.color;
        bounceCounter = 0;
        Destroy(this.gameObject, 10f);
    }

    private void Update()
    {
        if (isDead)
        {
            // Fade the sprite
            if (fadeColor.a > 0)
            {
                fadeColor.a -= fadeSpeed * Time.deltaTime;
                sprite.color = fadeColor;
            }
            else
            {
                Destroy(this.gameObject, 0.5f);
            }
        }
    }

    public void Damaged(int amount, Vector3 knockbackForce)
    {
        if (knockbackAble)
        {
            Knockback(knockbackForce);
        }
        PlayDamagedSound();
        StartCoroutine(SpriteFlash());
        currentDurability -= amount;
        if (currentDurability <= 0)
            Death();
    }

    private void PlayDamagedSound()
    {
        if (hitSound == null)
            return;

        float randomVolume = Random.Range(0.8f, 1f);
        float randomPitch = Random.Range(0.7f, 1.3f);

        hitSound.volume = randomVolume;
        hitSound.pitch = randomPitch;
        hitSound.Play();
    }

    private void PlayCollideSound()
    {
        if (collideSound == null)
            return;

        float randomVolume = Random.Range(0.8f, 1f);
        float randomPitch = Random.Range(0.7f, 1.3f);

        collideSound.volume = randomVolume;
        collideSound.pitch = randomPitch;
        collideSound.Play();
    }

    private void Knockback(Vector3 knockbackForce)
    {
        rb.velocity = Vector2.zero;
        rb.AddForce(knockbackForce, ForceMode2D.Impulse);
    }

    private IEnumerator SpriteFlash()
    {
        sprite.material = flashMat;
        yield return new WaitForSeconds(0.15f);
        sprite.material = originalMat;
    }

    public void Death()
    {
        // Change collision and sprite
        GetComponent<Collider2D>().enabled = false;
        //Color deathColor = sprite.color;
        //deathColor.r = 0.3f; deathColor.b = 0.3f; deathColor.g = 0.3f;
        //sprite.color = deathColor;
        //sprite.transform.localScale = new Vector3(sprite.transform.localScale.x, -1, sprite.transform.localScale.z);
        rb.gravityScale = 1;
        rb.drag = 1;
        rb.freezeRotation = false;
        isDead = true;
        Destroy(this.gameObject, 5f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        bounceCounter++;
        PlayCollideSound();

        // Hit another projectile
        Projectile projectile = collision.transform.GetComponent<Projectile>();
        if (projectile)
        {
            if (projectile.projectileHittable)
                projectile.Damaged(damage, (projectile.transform.position - transform.position).normalized);
            else
                return;
        }

        // Hit some entities (player, enemies, destructible...)
        else if (gameObject.layer == LayerMask.NameToLayer("EnemyAttack"))
        {
            Player player = collision.transform.GetComponent<Player>();
            if (player)
            {
                player.Damaged(damage, (player.transform.position - transform.position).normalized);
                player.InflictedStatusEffect(inflictStatus);
            }
        }
        else if (gameObject.layer == LayerMask.NameToLayer("PlayerAttack"))
        {
            Enemy enemy = collision.transform.GetComponent<Enemy>();
            if (enemy)
            {
                enemy.Damaged(damage, (enemy.transform.position - transform.position).normalized);
            }
        }
        else
        {
            Debug.Log("This projectile is an environmental attack (deal damage to everyone). Not yet implemented");
        }

        // Hit ground
        if (bounceCounter > maxBounce)
            Death();
    }
}
