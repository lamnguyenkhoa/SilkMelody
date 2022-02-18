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
    private bool unscaled;

    private void Start()
    {
        cmVirtualCamera = GetComponent<CinemachineVirtualCamera>();
        instance = this;
    }

    private void Update()
    {
        if (shakeTimer > 0f)
        {
            if (unscaled)
                shakeTimer -= Time.unscaledDeltaTime;
            else
                shakeTimer -= Time.deltaTime;

            if (shakeTimer <= 0f)
            {
                CinemachineBasicMultiChannelPerlin cmPerlin = cmVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
                cmPerlin.m_AmplitudeGain = Mathf.Lerp(startingIntensity, 0f, 1 - (shakeTimer / shakeTimerTotal));
                GameObject.Find("Main Camera").GetComponent<CinemachineBrain>().m_IgnoreTimeScale = false;
            }
        }
    }

    public void ShakeCamera(float intensity, float time, bool unscaled)
    {
        CinemachineBasicMultiChannelPerlin cmPerlin = cmVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        cmPerlin.m_AmplitudeGain = intensity;
        startingIntensity = intensity;
        shakeTimer = time;
        shakeTimerTotal = time;
        this.unscaled = unscaled;

        GameObject.Find("Main Camera").GetComponent<CinemachineBrain>().m_IgnoreTimeScale = unscaled;
    }
}
