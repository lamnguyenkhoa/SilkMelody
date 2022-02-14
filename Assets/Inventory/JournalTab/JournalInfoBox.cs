using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class JournalInfoBox : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descText;
    public TextMeshProUGUI flavorText;
    public TextMeshProUGUI killCount;

    public static JournalInfoBox instance;

    private void Start()
    {
        if (!instance)
        {
            instance = this;
            nameText.text = "";
            descText.text = "";
            flavorText.text = "";
            killCount.text = "Defeated: 0";
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}
