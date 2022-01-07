using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class EnemySlash : MonoBehaviour
{
    public int damage;
    public float knockbackPower = 1f;
    private SpriteRenderer sprite;
    private Color fadeColor;
    private float fadeSpeed = 0f;
    public float fadeSpeedAccel = 0.1f;

    private float timer;
    public float activeTime = 0.15f;

    private void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        fadeColor = sprite.color;
        timer = 0f;
    }

    private void Update()
    {
        // Prevent long lasting hitbox
        timer += Time.deltaTime;
        if (timer > activeTime)
            transform.GetComponent<BoxCollider2D>().enabled = false;

        // Fade the sprite
        if (fadeColor.a > 0)
        {
            fadeColor.a -= fadeSpeed * Time.deltaTime;
            sprite.color = fadeColor;
            fadeSpeed += fadeSpeedAccel * Time.deltaTime;
        }
        else
        {
            Destroy(this.gameObject, 1f);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.GetComponent<Player>();
        if (player)
        {
            player.Damaged(damage, (player.transform.position - transform.position).normalized);
        }
    }
}