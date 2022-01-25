using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorSwitch : MonoBehaviour
{
    private bool activated;
    public Sprite activatedSprite;
    public GameObject[] deactivateObjs;
    public GameObject[] activateObjs;
    public AudioSource sfx;

    public void Activate()
    {
        if (activated)
            return;
        activated = true;
        sfx.Play();
        transform.GetComponent<SpriteRenderer>().sprite = activatedSprite;
        foreach (GameObject obj in deactivateObjs)
            obj.SetActive(false);
        foreach (GameObject obj in activateObjs)
            obj.SetActive(true);
    }
}