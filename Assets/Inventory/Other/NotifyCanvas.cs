using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NotifyCanvas : MonoBehaviour
{
    public static NotifyCanvas instance;
    public GameObject notifyBox;
    public Transform notifyGroup;
    //public List<string> currentDisplayItemNames = new List<string>();

    private void Awake()
    {
        if (!instance)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void AddItemNotifyBox(Sprite sprite, string itemName)
    {
        GameObject newNotifyBox = Instantiate(notifyBox, notifyGroup, false);
        newNotifyBox.transform.Find("ItemImage").GetComponent<Image>().sprite = sprite;
        newNotifyBox.transform.Find("ItemName").GetComponent<TextMeshProUGUI>().text = itemName;
    }
}
