using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSlash : MonoBehaviour
{
    public Transform player;
    private SpriteRenderer sprite;
    private Color originalColor;
    public float fadeSpeed = 0.2f;
    private float timer;
    public float activeTime = 0.25f;

    private void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        originalColor = sprite.color;
        timer = 0f;
    }

    private void Update()
    {
        // Prevent late enemy get inside the hitbox
        timer += Time.deltaTime;
        if (timer > activeTime)
            transform.GetComponent<BoxCollider2D>().enabled = false;

        // Fade the sprite
        if (originalColor.a > 0)
        {
            originalColor.a -= fadeSpeed;
            sprite.color = originalColor;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Enemy enemy = collision.GetComponent<Enemy>();
        if (enemy)
        {
            // Push enemy backward slightly
            Vector2 knockbackForce = (Vector2)(enemy.transform.position - player.position);
            enemy.Damaged(1, knockbackForce.normalized);
        }
    }
}