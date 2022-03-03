using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class JournalPage : MonoBehaviour
{
    public GameObject firstSelectedObject;

    private void OnEnable()
    {
        SetFirstSelectedButton();
    }

    private void OnDisable()
    {
    }

    private void SetFirstSelectedButton()
    {
        EventSystem.current.SetSelectedGameObject(null);
        StartCoroutine(SetSelectedGameObject(firstSelectedObject));
    }

    private IEnumerator SetSelectedGameObject(GameObject selectedGameObject)
    {
        yield return new WaitForSeconds(0.01f);
        EventSystem.current.SetSelectedGameObject(selectedGameObject, new BaseEventData(EventSystem.current));
    }
}
