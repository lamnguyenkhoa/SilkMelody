using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ToolButton : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    private GameObject selectFrame;
    private Image image;
    public enum ToolType
    { red, blue, yellow };
    public ToolType toolType;

    public RedTool.ToolName redToolName;
    public BlueTool.ToolName blueToolName;
    public YellowTool.ToolName yellowToolName;

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
        switch (toolType)
        {
            case ToolType.red:
                ToolInfoBox.instance.nameText.text = GameMaster.instance.redToolData[(int)redToolName].displayName;
                ToolInfoBox.instance.descText.text = GameMaster.instance.redToolData[(int)redToolName].description;
                ToolInfoBox.instance.image.sprite = GameMaster.instance.redToolData[(int)redToolName].sprite;

                break;

            case ToolType.blue:
                ToolInfoBox.instance.nameText.text = GameMaster.instance.blueToolData[(int)blueToolName].displayName;
                ToolInfoBox.instance.descText.text = GameMaster.instance.blueToolData[(int)blueToolName].description;
                ToolInfoBox.instance.image.sprite = GameMaster.instance.blueToolData[(int)blueToolName].sprite;
                break;

            case ToolType.yellow:
                ToolInfoBox.instance.nameText.text = GameMaster.instance.yellowToolData[(int)yellowToolName].displayName;
                ToolInfoBox.instance.descText.text = GameMaster.instance.yellowToolData[(int)yellowToolName].description;
                ToolInfoBox.instance.image.sprite = GameMaster.instance.yellowToolData[(int)yellowToolName].sprite;
                break;
        }
        ToolInfoBox.instance.image.enabled = true;
    }

    public void OnDeselect(BaseEventData eventData)
    {
        selectFrame.SetActive(false);
    }

    public void PressButton()
    {
        if (toolType == ToolType.red)
            GameMaster.instance.EquipUnequipRedTool(redToolName);
        if (toolType == ToolType.blue)
            GameMaster.instance.EquipUnequipBlueTool(blueToolName);
        if (toolType == ToolType.yellow)
            GameMaster.instance.EquipUnequipYellowTool(yellowToolName);

        UpdateEquipState();
    }

    private void UpdateEquipState()
    {
        // Red tools
        if (toolType == ToolType.red)
        {
            if (GameMaster.instance.equippedRedTools.Contains(redToolName))
            {
                image.color = new Color(1, 1, 1, 0.3f);
            }
            else
            {
                image.color = new Color(1, 1, 1, 1);
            }
        }
        // Blue tools
        if (toolType == ToolType.blue)
        {
            if (GameMaster.instance.equippedBlueTools.Contains(blueToolName))
            {
                image.color = new Color(1, 1, 1, 0.3f);
            }
            else
            {
                image.color = new Color(1, 1, 1, 1);
            }
        }
        // Yellow tools
        if (toolType == ToolType.yellow)
        {
            if (GameMaster.instance.equippedYellowTools.Contains(yellowToolName))
            {
                image.color = new Color(1, 1, 1, 0.3f);
            }
            else
            {
                image.color = new Color(1, 1, 1, 1);
            }
        }
    }
}
