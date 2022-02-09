using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotifyBox : MonoBehaviour
{
    private float displayDuration = 4f;
    private float timer;
    private float fadeSpeed = 1f;
    private float resizeSpeed = 150f;

    private int stage = 0;
    private CanvasGroup canvasGroup;
    private RectTransform rectTransform;

    private void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        rectTransform = GetComponent<RectTransform>();
        canvasGroup.alpha = 0f;
    }

    private void Update()
    {
        switch (stage)
        {
            case 0:
                canvasGroup.alpha += fadeSpeed * 2 * Time.deltaTime;
                if (canvasGroup.alpha >= 1)
                    stage = 1;
                break;

            case 1:
                timer += Time.deltaTime;
                if (timer > displayDuration)
                    stage = 2;
                break;

            case 2:
                canvasGroup.alpha -= fadeSpeed * Time.deltaTime;
                if (canvasGroup.alpha <= 0)
                    stage = 3;
                break;

            case 3:
                float newHeight = rectTransform.rect.height;
                newHeight -= resizeSpeed * Time.deltaTime;
                rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, newHeight);

                if (rectTransform.sizeDelta.y <= 0)
                    Destroy(this.gameObject);
                break;
        }
    }
}
