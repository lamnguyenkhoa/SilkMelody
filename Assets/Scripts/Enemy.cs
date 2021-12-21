using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private SpriteRenderer sprite;
    private Material originalMat;
    public Material flashMat;

    private void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        originalMat = sprite.material;
    }

    // Update is called once per frame
    private void Update()
    {
    }

    public void Damaged()
    {
        StopAllCoroutines();
        StartCoroutine(SpriteFlash());
    }

    private IEnumerator SpriteFlash()
    {
        sprite.material = flashMat;
        yield return new WaitForSeconds(0.15f);
        sprite.material = originalMat;
    }
}