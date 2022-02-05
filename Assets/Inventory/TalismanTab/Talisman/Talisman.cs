using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Talisman : MonoBehaviour
{
    public GameObject[] redSlots;
    public GameObject[] blueSlots;
    public GameObject[] yellowSlots;

    public enum TalismanName
    {
        wanderer, sentinel, toolmaster
    }
    public TalismanName thisTalismanName;

    public void UpdateSlotImage()
    {
        // Red tools
        for (int i = 0; i < redSlots.Length; i++)
        {
            Image image = redSlots[i].transform.GetChild(0).GetComponent<Image>();
            if (i < GameMaster.instance.playerData.equippedRedTools.Count)
            {
                int toolId = (int)GameMaster.instance.playerData.equippedRedTools[i];
                image.sprite = GameMaster.instance.redToolData[toolId].sprite;
                image.gameObject.SetActive(true);
            }
            else
            {
                image.gameObject.SetActive(false);
            }
        }

        // Blue tools
        for (int i = 0; i < blueSlots.Length; i++)
        {
            Image image = blueSlots[i].transform.GetChild(0).GetComponent<Image>();
            if (i < GameMaster.instance.playerData.equippedBlueTools.Count)
            {
                int toolId = (int)GameMaster.instance.playerData.equippedBlueTools[i];
                image.sprite = GameMaster.instance.blueToolData[toolId].sprite;
                image.gameObject.SetActive(true);
            }
            else
            {
                image.gameObject.SetActive(false);
            }
        }

        // Yellow tools
        for (int i = 0; i < yellowSlots.Length; i++)
        {
            Image image = yellowSlots[i].transform.GetChild(0).GetComponent<Image>();
            if (i < GameMaster.instance.playerData.equippedYellowTools.Count)
            {
                int toolId = (int)GameMaster.instance.playerData.equippedYellowTools[i];
                image.sprite = GameMaster.instance.yellowToolData[toolId].sprite;
                image.gameObject.SetActive(true);
            }
            else
            {
                image.gameObject.SetActive(false);
            }
        }
    }
}
