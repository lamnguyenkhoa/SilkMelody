using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraFollowTarget : MonoBehaviour
{
    public string targetName;

    private void Start()
    {
        Transform target = GameObject.Find(targetName).transform;
        GetComponent<CinemachineVirtualCamera>().m_Follow = target;
    }
}