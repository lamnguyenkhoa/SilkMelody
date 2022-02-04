using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TabTalisman : MonoBehaviour
{
    public GameObject redGroup;
    public GameObject blueGroup;
    public GameObject yellowGroup;

    private void OnEnable()
    {
        SetFirstSelectedButton();
    }

    public void SetFirstSelectedButton()
    {
        var eventSystem = EventSystem.current;
        GameObject selectedGameObject = null;
        eventSystem.SetSelectedGameObject(null);
        if (redGroup.transform.childCount > 0)
        {
            selectedGameObject = redGroup.transform.GetChild(0).gameObject;
            eventSystem.SetSelectedGameObject(selectedGameObject, new BaseEventData(eventSystem));
        }
        else if (blueGroup.transform.childCount > 0)
        {
            selectedGameObject = blueGroup.transform.GetChild(0).gameObject;
            eventSystem.SetSelectedGameObject(selectedGameObject, new BaseEventData(eventSystem));
        }
        else if (yellowGroup.transform.childCount > 0)
        {
            selectedGameObject = yellowGroup.transform.GetChild(0).gameObject;
            eventSystem.SetSelectedGameObject(selectedGameObject, new BaseEventData(eventSystem));
        }

        // Display the select frame sprite
        if (selectedGameObject != null)
            selectedGameObject.transform.GetChild(0).gameObject.SetActive(true);
    }
}