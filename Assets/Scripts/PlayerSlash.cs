using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class PlayerSlash : MonoBehaviour
{
    [HideInInspector] public Transform player;
    public Light2D sparkLight;
    public float damage = 1f;
    private SpriteRenderer sprite;
    private Color fadeColor;
    private float fadeSpeed = 0f;
    public float fadeSpeedAccel = 0.1f;
    private float lightOriginalIntensitiy;

    private float timer;
    public float activeTime = 0.25f;

    private void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        fadeColor = sprite.color;
        timer = 0f;
        lightOriginalIntensitiy = sparkLight.intensity;
    }

    private void Update()
    {
        // Prevent late enemy get inside the hitbox
        timer += Time.deltaTime;
        if (timer > activeTime)
            transform.GetComponent<BoxCollider2D>().enabled = false;

        // Fade the sprite
        if (fadeColor.a > 0)
        {
            fadeColor.a -= fadeSpeed * Time.deltaTime;
            sprite.color = fadeColor;
            fadeSpeed += fadeSpeedAccel * Time.deltaTime;
            sparkLight.intensity = lightOriginalIntensitiy * fadeColor.a;
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
            sparkLight.enabled = true;
            // Push enemy backward slightly
            Vector2 knockbackDir = (Vector2)(enemy.transform.position - player.position).normalized;
            enemy.Damaged(damage, knockbackDir);
        }
    }
}