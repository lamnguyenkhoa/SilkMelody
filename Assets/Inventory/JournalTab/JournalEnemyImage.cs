using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JournalEnemyImage : MonoBehaviour
{
    public Image image;

    public static JournalEnemyImage instance;

    private void Start()
    {
        if (!instance)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}
