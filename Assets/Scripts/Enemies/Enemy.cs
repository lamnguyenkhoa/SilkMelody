using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Stat")]
    public float maxHp;
    private float currentHp;
    public int damage;
    public bool knockbackAble = true;
    public bool isInvulnerable = false;
    public bool shouldStopMoving = false;

    [Header("Loot")]
    public GameObject dropLoot;
    public int dropAmount;
    public Transform lootPos;

    [Header("Other")]
    public Material flashMat;
    private SpriteRenderer sprite;
    private Material originalMat;
    private Rigidbody2D rb;
    private Coroutine spriteFlashCoroutine;
    private Coroutine stopMovingCoroutine;

    private void Start()
    {
        currentHp = maxHp;
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        originalMat = sprite.material;
    }

    public void Damaged(float amount, Vector3 knockbackForce)
    {
        if (knockbackAble)
        {
            Knockback(knockbackForce);
        }
        if (!isInvulnerable)
        {
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
        if (dropLoot)
        {
            for (int i = 0; i < dropAmount; i++)
            {
                Vector2 explodeForce = new Vector2(Random.Range(-2f, 2f), 5f);
                GameObject spawnedLoot = Instantiate(dropLoot, lootPos.position, Quaternion.identity);
                spawnedLoot.GetComponent<Rigidbody2D>().AddForce(explodeForce, ForceMode2D.Impulse);
            }
        }

        Destroy(this.gameObject);
    }

    private void Knockback(Vector3 knockbackForce)
    {
        rb.velocity = Vector2.zero;
        rb.AddForce(knockbackForce, ForceMode2D.Impulse);
        if (stopMovingCoroutine != null)
            StopCoroutine(stopMovingCoroutine);
        stopMovingCoroutine = StartCoroutine(StopMoving());
    }

    private IEnumerator StopMoving()
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
}