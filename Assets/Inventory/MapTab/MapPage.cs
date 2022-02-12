using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapPage : MonoBehaviour
{
    public float moveSpeed;
    public float zoomSpeed;
    private InputMaster inputMaster;
    public GameObject playerMarker;

    public GameObject mapContent;

    private void OnEnable()
    {
        inputMaster = new InputMaster();
        inputMaster.Enable();
        UpdatePlayerPosOnMap();
    }

    private void OnDisable()
    {
        if (inputMaster != null)
            inputMaster.Disable();
    }

    private void Update()
    {
        HandleMapControl();
    }

    private void HandleMapControl()
    {
        Vector2 moveDirection = inputMaster.Inventory.Movement2.ReadValue<Vector2>();
        mapContent.transform.localPosition += -(Vector3)moveDirection * moveSpeed * Time.deltaTime;

        float zoomDirection = inputMaster.Inventory.Zoom.ReadValue<float>();
        mapContent.transform.localScale += Time.deltaTime * zoomDirection * new Vector3(zoomSpeed, zoomSpeed);

        if (mapContent.transform.localScale.x < 0.2f)
            mapContent.transform.localScale = new Vector3(0.2f, 0.2f);
        else if (mapContent.transform.localScale.x > 3)
            mapContent.transform.localScale = new Vector3(3f, 3f);
    }

    public void UpdatePlayerPosOnMap()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;

        int areaCount = mapContent.transform.childCount;
        for (int i = 0; i < areaCount; i++)
        {
            Transform area = mapContent.transform.GetChild(i);
            int roomCount = area.childCount;
            for (int j = 0; j < roomCount; j++)
            {
                Transform room = area.GetChild(j);
                if (room.name == currentSceneName)
                {
                    // The marker will automatically destroyed on Disable
                    Instantiate(playerMarker, room, false);

                    // Auto centering toward that room
                    mapContent.transform.localScale = new Vector3(1f, 1f);
                    mapContent.transform.localPosition = -area.transform.localPosition;
                    mapContent.transform.localPosition -= room.transform.localPosition;
                    return;
                }
            }
        }
    }
}
