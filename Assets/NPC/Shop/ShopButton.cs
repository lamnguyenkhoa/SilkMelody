using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;

public class ShopButton : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    private GameObject selectFrame;

    private void OnEnable()
    {
        selectFrame = transform.Find("SelectFrame").gameObject;
    }

    private void OnDisable()
    {
        selectFrame.SetActive(false);
    }

    public void OnSelect(BaseEventData eventData)
    {
        selectFrame.SetActive(true);

        // Moving the scroll
        Canvas.ForceUpdateCanvases();

        RectTransform panel = transform.parent.GetComponent<RectTransform>();
        RectTransform scroll = panel.parent.GetComponent<RectTransform>();
        float endPosY = 0 - (scroll.sizeDelta.y / 2) - transform.GetComponent<RectTransform>().anchoredPosition.y;
        panel.anchoredPosition = new Vector2(panel.anchoredPosition.x, endPosY);

        // Remember the last selected entry (so when you comeback to Journal tab it dont go back to top)
        scroll.parent.GetComponent<JournalPage>().firstSelectedObject = this.gameObject;
    }

    public void OnDeselect(BaseEventData eventData)
    {
        selectFrame.SetActive(false);
    }
}
