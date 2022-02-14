using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MainMenuVersionText : MonoBehaviour
{
    private void Start()
    {
        GetComponent<TextMeshProUGUI>().text = Application.version;
    }
}
