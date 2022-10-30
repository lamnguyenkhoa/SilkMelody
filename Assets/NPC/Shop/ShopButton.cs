using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;

public class ShopButton : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    private GameObject selectFrame;
    public ShopItem shopItemSO;
    public Image itemImage;
    public TextMeshProUGUI priceText;

    private void OnEnable()
    {
        selectFrame = transform.Find("SelectFrame").gameObject;
        itemImage.sprite = shopItemSO.image;
        priceText.text = shopItemSO.price.ToString();
        this.gameObject.name = shopItemSO.itemName;
        GetComponent<Button>().onClick.AddListener(PressButton);
        WorldData worldData = GameMaster.instance.worldData;
        //Temporary
        string placeName = "MossyTown";
        if (worldData.boughItemNameAndLocation.Contains(placeName + "_" + shopItemSO.itemName))
        {
            this.gameObject.SetActive(false);
        }

    }

    private void OnDisable()
    {
        selectFrame.SetActive(false);
    }

    public void OnSelect(BaseEventData eventData)
    {
        selectFrame.SetActive(true);
        //transform.root.GetComponent<InventoryMenu>().movingButtonSound.Play();

        ShopInfoBox.instance.nameText.text = shopItemSO.itemName;
        ShopInfoBox.instance.descText.text = shopItemSO.description;

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

    public void PressButton()
    {
        Debug.Log(shopItemSO.itemName + "|" + shopItemSO.price);
        AddItemToInventoryController();
    }

    private int NextChild()
    {
        // Check where we are
        int thisIndex = this.transform.GetSiblingIndex();

        // We have a few cases to rule out
        if (this.transform.parent == null)
            return -1;
        if (this.transform.parent.childCount == 1) // last item
            return -1;
        if (this.transform.parent.childCount <= thisIndex + 1) //last sequence
            return thisIndex - 1;
        return thisIndex + 1;
    }

    private void AddItemToInventoryController()
    {
        PlayerData playerData = GameMaster.instance.playerData;
        playerData.copperShard -= shopItemSO.price;

        switch (shopItemSO.itemEnum)
        {
            case ShopItem.ShopItemEnum.lifebloodNeedle:
                playerData.foundRedTools.Add(RedTool.ToolName.lifebloodNeedle);
                break;
            case ShopItem.ShopItemEnum.maskShard:
                playerData.maxHp += 1;
                break;
            case ShopItem.ShopItemEnum.sentinelTalisman:
                playerData.foundTalismans.Add(Talisman.TalismanName.sentinel);
                break;
            default:
                Debug.Log("ShopButton item not found");
                break;
        }

        // Temporary
        string placeName = "MossyTown";
        WorldData worldData = GameMaster.instance.worldData;
        worldData.boughItemNameAndLocation.Add(placeName + "_" + shopItemSO.itemName);
        this.gameObject.SetActive(false);
        int nextButtonId = NextChild();
        if (nextButtonId != -1)
        {
            EventSystem.current.SetSelectedGameObject(this.transform.parent.GetChild(nextButtonId).gameObject);
        }
    }
}
