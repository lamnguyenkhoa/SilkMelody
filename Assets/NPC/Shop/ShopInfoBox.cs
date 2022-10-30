using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShopInfoBox : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descText;

    public static ShopInfoBox instance;

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
