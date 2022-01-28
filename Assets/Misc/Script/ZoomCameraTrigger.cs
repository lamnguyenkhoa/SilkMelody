using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ZoomCameraTrigger : MonoBehaviour
{
    public CinemachineVirtualCamera vCam;
    public float targetLenSize;
    public float zoomTime;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (vCam.m_Lens.OrthographicSize != targetLenSize)
            StartCoroutine(ChangeLenSize());
    }

    private IEnumerator ChangeLenSize()
    {
        float timer = 0f;
        while (timer < zoomTime)
        {
            timer += Time.deltaTime;
            vCam.m_Lens.OrthographicSize = Mathf.Lerp(vCam.m_Lens.OrthographicSize, targetLenSize, timer / zoomTime);
            yield return null;
        }
    }
}