using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Stat")]
    public float maxHp;
    private float hp;
    public int damage;
    public float knockbackForce = 1f;
    public bool knockbackAble = true;
    public bool isInvulnerable = false;

    [Header("Other")]
    private SpriteRenderer sprite;
    private Material originalMat;
    public Material flashMat;
    private Rigidbody2D rb;

    private void Start()
    {
        hp = maxHp;
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        originalMat = sprite.material;
    }

    public void Damaged(float amount, Vector3 knockbackDir)
    {
        if (knockbackAble)
        {
            Knockback(knockbackDir);
        }
        if (!isInvulnerable)
        {
            StopAllCoroutines();
            StartCoroutine(SpriteFlash());
            hp -= amount;
            if (hp <= 0)
                Death();
        }
    }

    private void Death()
    {
        Destroy(this.gameObject);
    }

    private void Knockback(Vector3 knockbackDir)
    {
        rb.AddForce(knockbackDir * knockbackForce, ForceMode2D.Impulse);
    }

    private IEnumerator SpriteFlash()
    {
        sprite.material = flashMat;
        yield return new WaitForSeconds(0.15f);
        sprite.material = originalMat;
    }
}