using System;
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

    [DrawIf("toolType", ToolType.red)]
    public RedTool.ToolName redToolName;
    [DrawIf("toolType", ToolType.blue)]
    public BlueTool.ToolName blueToolName;
    [DrawIf("toolType", ToolType.yellow)]
    public YellowTool.ToolName yellowToolName;

    public Sprite emptyToolSprite;
    private bool foundItem;

    public void OnEnable()
    {
        selectFrame = transform.GetChild(0).gameObject;
        image = GetComponent<Image>();
        UpdateEquipState();
        GameMaster.instance.OnTalismanChange += UpdateEquipState;
        CheckIfToolFound();
    }

    private void OnDisable()
    {
        selectFrame.SetActive(false);
        GetComponent<Button>().onClick.RemoveAllListeners();
        GameMaster.instance.OnTalismanChange -= UpdateEquipState;
    }

    public void OnSelect(BaseEventData eventData)
    {
        selectFrame.SetActive(true);
        transform.root.GetComponent<InventoryMenu>().movingButtonSound.Play();

        if (foundItem)
        {
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
        else
        {
            ToolInfoBox.instance.nameText.text = "";
            ToolInfoBox.instance.descText.text = "";
            ToolInfoBox.instance.image.sprite = emptyToolSprite;
            ToolInfoBox.instance.image.enabled = false;
        }

        ScrollPanel();
    }

    public void OnDeselect(BaseEventData eventData)
    {
        selectFrame.SetActive(false);
    }

    private void ScrollPanel()
    {
        Canvas.ForceUpdateCanvases();

        RectTransform rect = GetComponent<RectTransform>();
        RectTransform groupRect = transform.parent.GetComponent<RectTransform>();
        RectTransform panelRect = groupRect.parent.GetComponent<RectTransform>();
        RectTransform scrollRect = panelRect.parent.GetComponent<RectTransform>();

        float itemPosY = rect.anchoredPosition.y + groupRect.anchoredPosition.y;
        float endPosY = 0 - (scrollRect.sizeDelta.y / 2) - itemPosY;
        panelRect.anchoredPosition = new Vector2(panelRect.anchoredPosition.x, endPosY);
    }

    private void CheckIfToolFound()
    {
        PlayerData playerData = GameMaster.instance.playerData;

        switch (toolType)
        {
            case ToolType.red:
                if (playerData.foundRedTools.Contains(redToolName))
                {
                    image.sprite = GameMaster.instance.redToolData[(int)redToolName].sprite;
                    GetComponent<Button>().onClick.AddListener(PressButton);
                    foundItem = true;
                }
                else
                {
                    image.sprite = emptyToolSprite;
                    GetComponent<Button>().onClick.RemoveAllListeners();
                    foundItem = false;
                }
                break;

            case ToolType.blue:
                if (playerData.foundBlueTools.Contains(blueToolName))
                {
                    image.sprite = GameMaster.instance.blueToolData[(int)blueToolName].sprite;
                    GetComponent<Button>().onClick.AddListener(PressButton);
                    foundItem = true;
                }
                else
                {
                    image.sprite = emptyToolSprite;
                    GetComponent<Button>().onClick.RemoveAllListeners();
                    foundItem = false;
                }
                break;

            case ToolType.yellow:
                if (playerData.foundYellowTools.Contains(yellowToolName))
                {
                    image.sprite = GameMaster.instance.yellowToolData[(int)yellowToolName].sprite;
                    GetComponent<Button>().onClick.AddListener(PressButton);
                    foundItem = true;
                }
                else
                {
                    image.sprite = emptyToolSprite;
                    GetComponent<Button>().onClick.RemoveAllListeners();
                    foundItem = false;
                }
                break;
        }
    }

    public void PressButton()
    {
        Player player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        if (!player.resting)
            return;

        transform.root.GetComponent<InventoryMenu>().pressedButtonSound.Play();
        switch (toolType)
        {
            case ToolType.red:
                GameMaster.instance.EquipUnequipRedTool(redToolName);
                break;

            case ToolType.blue:
                GameMaster.instance.EquipUnequipBlueTool(blueToolName);
                break;

            case ToolType.yellow:
                GameMaster.instance.EquipUnequipYellowTool(yellowToolName);
                break;
        }
    }

    private void UpdateEquipState()
    {
        switch (toolType)
        {
            case ToolType.red:
                if (image.color.a != 0.3f && GameMaster.instance.playerData.equippedRedTools.Contains(redToolName))
                    image.color = new Color(1, 1, 1, 0.3f);
                else if (image.color.a != 1f && !GameMaster.instance.playerData.equippedRedTools.Contains(redToolName))
                    image.color = new Color(1, 1, 1, 1);
                break;

            case ToolType.blue:
                if (image.color.a != 0.3f && GameMaster.instance.playerData.equippedBlueTools.Contains(blueToolName))
                    image.color = new Color(1, 1, 1, 0.3f);
                else if (image.color.a != 1f && !GameMaster.instance.playerData.equippedBlueTools.Contains(blueToolName))
                    image.color = new Color(1, 1, 1, 1);
                break;

            case ToolType.yellow:
                if (image.color.a != 0.3f && GameMaster.instance.playerData.equippedYellowTools.Contains(yellowToolName))
                    image.color = new Color(1, 1, 1, 0.3f);
                else if (image.color.a != 1f && !GameMaster.instance.playerData.equippedYellowTools.Contains(yellowToolName))
                    image.color = new Color(1, 1, 1, 1);
                break;
        }
    }
}
