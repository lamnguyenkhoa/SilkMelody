using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeLevelTrigger : MonoBehaviour
{
    public string levelName;
    private bool activated;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!activated)
        {
            Player player = collision.gameObject.GetComponent<Player>();
            if (player)
            {
                LevelLoader.instance.LoadLevel(levelName);
            }
        }
    }
}