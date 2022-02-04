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

    public string talismanName;

    public void UpdateSlotImage()
    {
        // Red tools
        for (int i = 0; i < redSlots.Length; i++)
        {
            Image image = redSlots[i].transform.GetChild(0).GetComponent<Image>();
            if (i < GameMaster.instance.equippedTools.Count)
            {
                int toolId = (int)GameMaster.instance.equippedTools[GameMaster.instance.selectedId];
                image.sprite = GameMaster.instance.redToolData[toolId].sprite;
                image.gameObject.SetActive(true);
            }
            else
            {
                image.gameObject.SetActive(false);
            }
        }
    }
}