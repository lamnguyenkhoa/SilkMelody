using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PathDisplay : MonoBehaviour
{
    public string room0;
    public string room1;

    private void OnEnable()
    {
        if (GameMaster.instance.worldData.visitedRooms.Contains(room0) || GameMaster.instance.worldData.visitedRooms.Contains(room1))
        {
            if (!GetComponent<Image>().enabled)
            {
                GetComponent<Image>().enabled = true;
                for (int i = 0; i < transform.childCount; i++)
                {
                    transform.GetChild(0).gameObject.SetActive(true);
                }
            }
        }
        else
        {
            if (GetComponent<Image>().enabled)
            {
                GetComponent<Image>().enabled = false;
                for (int i = 0; i < transform.childCount; i++)
                {
                    transform.GetChild(0).gameObject.SetActive(false);
                }
            }
        }
    }
}
