using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PPVolumeController : MonoBehaviour
{
    private float originalVignetteValue;
    private Volume volume;
    private Vignette vg;

    private void Start()
    {
        Volume volume = GetComponent<Volume>();
        volume.profile.TryGet<Vignette>(out vg);
        originalVignetteValue = vg.intensity.value;
    }

    public void DamagedPPEffect()
    {
        StopAllCoroutines();
        StartCoroutine(HurtVignette());
    }

    public IEnumerator HurtVignette()
    {
        vg.intensity.value = 0.6f;
        float duration = 2f;
        float timer = 0f;
        float startValue = 0.6f;
        while (timer < duration)
        {
            yield return null;
            timer += Time.deltaTime;
            vg.intensity.value = Mathf.Lerp(startValue, originalVignetteValue, timer / duration);
        }
        vg.intensity.value = originalVignetteValue;
        yield return null;
    }
}
