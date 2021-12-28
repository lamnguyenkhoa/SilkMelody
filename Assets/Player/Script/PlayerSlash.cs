using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class PlayerSlash : MonoBehaviour
{
    [HideInInspector] public Transform player;
    public Light2D sparkLight;
    public float damage = 1f;
    public float knockbackPower = 1f;
    private SpriteRenderer sprite;
    private Color fadeColor;
    private float fadeSpeed = 0f;
    public float fadeSpeedAccel = 0.1f;
    private float lightOriginalIntensitiy;
    [SerializeField] private AudioSource hitEnemySound;
    private bool playedImpact = false;

    private float timer;
    public float activeTime = 0.15f;

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
            Destroy(this.gameObject, 1f);
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
        if (enemy)
        {
            // Play effect that only happened ONCE (if player hit enemy)
            if (!playedImpact)
            {
                sparkLight.enabled = true;
                PlayHitEnemySound();
                playedImpact = true;
            }
            // Push enemy backward slightly
            Vector2 knockbackForce = (Vector2)(enemy.transform.position - player.position).normalized * knockbackPower;
            enemy.Damaged(damage, knockbackForce);
            // Play hit/damaged effect on enemy (if there are multiple enemies within attack)
        }
    }
}