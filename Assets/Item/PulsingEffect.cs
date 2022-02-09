using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class PulsingEffect : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private float pulseScale;
    public float minPulse, maxPulse;
    public float pulseSpeed;

    private float timer;
    public float restTime;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        pulseScale = minPulse;
    }

    private void Update()
    {
        float velocity = 0f;
        transform.localScale = new Vector2(pulseScale, pulseScale);
        pulseScale = Mathf.SmoothDamp(pulseScale, maxPulse, ref velocity, 1 / pulseSpeed, 100, Time.deltaTime);

        Color color = spriteRenderer.color;
        color.a = 1 - ((pulseScale - minPulse) / (maxPulse - minPulse));
        spriteRenderer.color = color;

        if (pulseScale >= (maxPulse - 0.05f))
        {
            if (timer < restTime)
                timer += Time.deltaTime;
            else
            {
                timer = 0f;
                pulseScale = minPulse;
            }
        }
    }
}
