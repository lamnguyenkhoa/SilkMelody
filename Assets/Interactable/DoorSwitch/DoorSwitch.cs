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
    public int doorSwitchId;
    private WorldData worldState;

    private void Awake()
    {
        worldState = GameMaster.instance.worldData;

        if (worldState.doorSwitches[doorSwitchId])
        {
            activated = true;
            transform.GetComponent<SpriteRenderer>().sprite = activatedSprite;
            foreach (GameObject obj in deactivateObjs)
                obj.SetActive(false);
            foreach (GameObject obj in activateObjs)
                obj.SetActive(true); ;
        }
    }

    public void Activate()
    {
        if (activated)
            return;
        worldState.doorSwitches[doorSwitchId] = true;
        activated = true;
        sfx.Play();
        transform.GetComponent<SpriteRenderer>().sprite = activatedSprite;
        foreach (GameObject obj in deactivateObjs)
            obj.SetActive(false);
        foreach (GameObject obj in activateObjs)
            obj.SetActive(true);
    }
}