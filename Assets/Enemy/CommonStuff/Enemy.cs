using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Stat")]
    public float maxHp;
    [SerializeField] private float currentHp;
    public int damage;
    public bool knockbackAble = true;
    public bool isInvulnerable = false;
    public bool shouldStopMoving = false;
    public bool isDead;
    public float iFrame = 0.1f;

    [Header("Loot")]
    public GameObject dropLoot;
    public int dropAmount;
    public Transform lootPos;

    [Header("Other")]
    public Material flashMat;
    public SpriteRenderer sprite;
    private Animator anim;
    private Material originalMat;
    private Rigidbody2D rb;
    private Coroutine spriteFlashCoroutine;
    private Coroutine stopMovingCoroutine;

    private void Start()
    {
        currentHp = maxHp;
        rb = GetComponent<Rigidbody2D>();
        originalMat = sprite.material;
        anim = sprite.transform.GetComponent<Animator>();
    }

    public void Damaged(float amount, Vector3 knockbackForce)
    {
        if (knockbackAble)
        {
            Knockback(knockbackForce);
        }
        if (!isInvulnerable)
        {
            StartCoroutine(InvincibleFrame());
            if (spriteFlashCoroutine != null)
                StopCoroutine(spriteFlashCoroutine);
            spriteFlashCoroutine = StartCoroutine(SpriteFlash());
            currentHp -= amount;
            if (currentHp <= 0)
                Death();
        }
    }

    private void Death()
    {
        // Change collision and sprite
        Color deathColor = sprite.color;
        deathColor.r = 0.3f; deathColor.b = 0.3f; deathColor.g = 0.3f;
        sprite.color = deathColor;
        sprite.transform.localScale = new Vector3(sprite.transform.localScale.x, -1, sprite.transform.localScale.z);
        transform.gameObject.layer = LayerMask.NameToLayer("DeadEnemy");
        if (anim)
        {
            anim.Play("Idle"); // Should be replaced with death sprite when asset available
            anim.speed = 0f;
        }
        rb.gravityScale = 1;
        rb.drag = 1;
        rb.freezeRotation = false;
        isDead = true;

        // Loot
        if (dropLoot)
        {
            for (int i = 0; i < dropAmount; i++)
            {
                Vector2 explodeForce = new Vector2(Random.Range(-5f, 5f), Random.Range(5f, 10f));
                GameObject spawnedLoot = Instantiate(dropLoot, lootPos.position, Quaternion.identity);
                spawnedLoot.GetComponent<Rigidbody2D>().AddForce(explodeForce, ForceMode2D.Impulse);
            }
        }

        this.enabled = false;
    }

    private void Knockback(Vector3 knockbackForce)
    {
        rb.velocity = Vector2.zero;
        rb.AddForce(knockbackForce, ForceMode2D.Impulse);
        if (stopMovingCoroutine != null)
            StopCoroutine(stopMovingCoroutine);
        stopMovingCoroutine = StartCoroutine(StopMoving());
    }

    public IEnumerator StopMoving()
    {
        shouldStopMoving = true;
        yield return new WaitForSeconds(0.25f);
        shouldStopMoving = false;
    }

    private IEnumerator SpriteFlash()
    {
        sprite.material = flashMat;
        yield return new WaitForSeconds(0.15f);
        sprite.material = originalMat;
    }

    private IEnumerator InvincibleFrame()
    {
        isInvulnerable = true;
        yield return new WaitForSeconds(iFrame);
        isInvulnerable = false;
    }
}