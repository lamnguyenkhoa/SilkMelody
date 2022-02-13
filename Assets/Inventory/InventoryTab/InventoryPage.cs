using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryPage : MonoBehaviour
{
    public GameObject firstSelectedObject; //verdant mantle

    private void OnEnable()
    {
        SetFirstSelectedButton();
    }

    private void SetFirstSelectedButton()
    {
        EventSystem.current.SetSelectedGameObject(null);
        StartCoroutine(SetSelectedGameObject(firstSelectedObject));
    }

    private IEnumerator SetSelectedGameObject(GameObject selectedGameObject)
    {
        yield return new WaitForSeconds(0.1f);
        EventSystem.current.SetSelectedGameObject(selectedGameObject, new BaseEventData(EventSystem.current));
    }
}
