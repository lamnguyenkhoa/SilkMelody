using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryInfoBox : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descText;

    public static InventoryInfoBox instance;

    private void Start()
    {
        if (!instance)
        {
            instance = this;
            nameText.text = "";
            descText.text = "";
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}
