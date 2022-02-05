using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ToolInfoBox : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public Image image;
    public TextMeshProUGUI descText;

    public static ToolInfoBox instance;

    private void Start()
    {
        if (!instance)
        {
            instance = this;
            nameText.text = "";
            descText.text = "";
            image.enabled = false;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}
