using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomDisplay : MonoBehaviour
{
    private void OnEnable()
    {
        string roomName = transform.name;

        if (GameMaster.instance.worldData.visitedRooms.Contains(roomName))
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
