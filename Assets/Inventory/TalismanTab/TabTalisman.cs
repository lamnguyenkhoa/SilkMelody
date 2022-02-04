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
        GameObject selectedGameObject = null;
        EventSystem.current.SetSelectedGameObject(null);
        if (redGroup.transform.childCount > 0)
        {
            selectedGameObject = redGroup.transform.GetChild(0).gameObject;
            StartCoroutine(SetSelectedGameObject(selectedGameObject));
        }
        else if (blueGroup.transform.childCount > 0)
        {
            selectedGameObject = blueGroup.transform.GetChild(0).gameObject;
            StartCoroutine(SetSelectedGameObject(selectedGameObject));
        }
        else if (yellowGroup.transform.childCount > 0)
        {
            selectedGameObject = yellowGroup.transform.GetChild(0).gameObject;
            StartCoroutine(SetSelectedGameObject(selectedGameObject));
        }

        // Display the select frame sprite
        if (selectedGameObject != null)
            selectedGameObject.transform.GetChild(0).gameObject.SetActive(true);
    }

    private IEnumerator SetSelectedGameObject(GameObject selectedGameObject)
    {
        yield return new WaitForSeconds(0.1f);
        EventSystem.current.SetSelectedGameObject(selectedGameObject, new BaseEventData(EventSystem.current));
    }
}
