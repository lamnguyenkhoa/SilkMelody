using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CinemachineShake : MonoBehaviour
{
    private CinemachineVirtualCamera cmVirtualCamera;
    private float shakeTimer;
    private float shakeTimerTotal;
    public static CinemachineShake instance;
    private float startingIntensity;

    private void Start()
    {
        cmVirtualCamera = GetComponent<CinemachineVirtualCamera>();
        instance = this;
    }

    private void Update()
    {
        if (shakeTimer > 0f)
        {
            shakeTimer -= Time.deltaTime;
            if (shakeTimer <= 0f)
            {
                CinemachineBasicMultiChannelPerlin cmPerlin = cmVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
                cmPerlin.m_AmplitudeGain = Mathf.Lerp(startingIntensity, 0f, 1 - (shakeTimer / shakeTimerTotal));
            }
        }
    }

    public void ShakeCamera(float intensity, float time)
    {
        CinemachineBasicMultiChannelPerlin cmPerlin = cmVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        cmPerlin.m_AmplitudeGain = intensity;
        startingIntensity = intensity;
        shakeTimer = time;
        shakeTimerTotal = time;
    }
}