using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float durability;
    private float currentDurability;
    public int damage;
    public bool knockbackAble = true;
    private Rigidbody2D rb;
    public Material flashMat;
    private Material originalMat;
    public SpriteRenderer sprite;
    private Color fadeColor;
    private float fadeSpeed = 2f;
    private bool isDead;
    public int maxBounce;
    private int bounceCounter;

    private void Start()
    {
        currentDurability = durability;
        rb = GetComponent<Rigidbody2D>();
        originalMat = sprite.material;
        fadeColor = sprite.color;
        bounceCounter = 0;
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

    public void Damaged(float amount, Vector3 knockbackForce)
    {
        if (knockbackAble)
        {
            Knockback(knockbackForce);
        }
        StartCoroutine(SpriteFlash());
        currentDurability -= amount;
        if (currentDurability <= 0)
            Death();
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
        Color deathColor = sprite.color;
        deathColor.r = 0.3f; deathColor.b = 0.3f; deathColor.g = 0.3f;
        sprite.color = deathColor;
        sprite.transform.localScale = new Vector3(sprite.transform.localScale.x, -1, sprite.transform.localScale.z);
        rb.gravityScale = 1;
        rb.drag = 1;
        rb.freezeRotation = false;
        isDead = true;
        Destroy(this.gameObject, 10f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        bounceCounter++;
        if (gameObject.layer == LayerMask.NameToLayer("EnemyAttack"))
        {
            Player player = collision.transform.GetComponent<Player>();
            if (player)
            {
                player.Damaged(damage, (player.transform.position - transform.position).normalized);
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
            Debug.Log("Environmental attack (deal damage to everyone). Not yet implemented");
        }

        if (bounceCounter > maxBounce)
            Death();
    }
}