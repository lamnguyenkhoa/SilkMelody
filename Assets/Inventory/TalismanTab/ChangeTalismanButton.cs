using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ChangeTalismanButton : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    public GameObject selectFrame;

    public void OnEnable()
    {
        selectFrame = transform.GetChild(0).gameObject;
    }

    public void PressedButton()
    {
        GameMaster.instance.ChangeToNextTalisman();
    }

    private void OnDisable()
    {
        selectFrame.SetActive(false);
    }

    public void OnSelect(BaseEventData eventData)
    {
        selectFrame.SetActive(true);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        selectFrame.SetActive(false);
    }
}
