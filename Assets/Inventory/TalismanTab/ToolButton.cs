using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ToolButton : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    private GameObject selectFrame;
    private Image image;
    public RedTool.ToolName toolName;

    public void OnEnable()
    {
        selectFrame = transform.GetChild(0).gameObject;
        image = GetComponent<Image>();
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

    public void PressButton()
    {
        GameMaster.instance.EquipUnequipRedTool(toolName);
        UpdateEquipState();
    }

    private void UpdateEquipState()
    {
        if (GameMaster.instance.equippedTools.Contains(toolName))
        {
            image.color = new Color(1, 1, 1, 0.3f);
        }
        else
        {
            image.color = new Color(1, 1, 1, 1);
        }
    }
}