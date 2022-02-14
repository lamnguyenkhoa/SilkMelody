using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class JournalButton : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    private GameObject selectFrame;
    public EnemyLore enemyLore;

    private int killCount;

    private void OnEnable()
    {
        selectFrame = transform.Find("SelectFrame").gameObject;
        killCount = GameMaster.instance.worldData.enemyKillCount[(int)enemyLore.thisEnemyType];
        if (killCount == 0)
            transform.Find("Name").GetComponent<TextMeshProUGUI>().text = "???";
        else
            transform.Find("Name").GetComponent<TextMeshProUGUI>().text = enemyLore.displayName;
    }

    private void OnDisable()
    {
        selectFrame.SetActive(false);
    }

    public void OnSelect(BaseEventData eventData)
    {
        selectFrame.SetActive(true);
        transform.root.GetComponent<InventoryMenu>().movingButtonSound.Play();

        JournalEnemyImage.instance.image.sprite = enemyLore.sprite;
        JournalEnemyImage.instance.image.SetNativeSize();
        JournalInfoBox.instance.killCount.text = "Defeated: " + killCount.ToString();

        // Discovered enemy
        if (killCount > 0)
        {
            JournalEnemyImage.instance.image.color = new Color(1, 1, 1);
            JournalInfoBox.instance.nameText.text = enemyLore.displayName;
            JournalInfoBox.instance.descText.text = enemyLore.enemyDescription;
            JournalInfoBox.instance.flavorText.text = enemyLore.flavourText;
        }
        else
        {
            JournalEnemyImage.instance.image.color = new Color(0, 0, 0); // Black silhouette
            JournalInfoBox.instance.nameText.text = "";
            JournalInfoBox.instance.descText.text = "";
            JournalInfoBox.instance.flavorText.text = "";
        }
    }

    public void OnDeselect(BaseEventData eventData)
    {
        selectFrame.SetActive(false);
    }
}
