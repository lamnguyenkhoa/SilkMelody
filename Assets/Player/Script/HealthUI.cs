using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    private PlayerData playerStat;
    public Sprite maskIcon;
    public Sprite emptyMaskIcon;
    private List<Image> maskList = new List<Image>();
    public Image maskPrefab;

    private int lastCurrentHp;
    private int lastMaxHp;
    private int lastLifebloodHp;


    private void Start()
    {
        playerStat = GameMaster.instance.playerData;
        lastCurrentHp = playerStat.currentHp;
        lastMaxHp = playerStat.maxHp;
        lastLifebloodHp = playerStat.lifebloodHp;

        InitNumberOfMask();
    }

    private void Update()
    {
        UpdateHealthUI();
    }

    public void UpdateHealthUI()
    {
        // Update current health with white mask / empty mask sprite
        if (lastCurrentHp != playerStat.currentHp)
        {
            RecalibrateCurrentMask();
        }

        // Update maximum number of permanent mask, used when acquired new mask
        // Currently there is no case of lowering max hp
        if (lastMaxHp < playerStat.maxHp)
        {
            InitNumberOfMask();
            lastMaxHp = playerStat.maxHp;
        }

        // Check for lifeblood hp (the blue temporary bonus masks)
        if (lastLifebloodHp != playerStat.lifebloodHp)
        {
            InitNumberOfMask();
            lastLifebloodHp = playerStat.lifebloodHp;
        }


        //// Update according to max hp
        //if (playerStat.maxHp + playerStat.lifebloodHp > maskList.Count)
        //    IncreaseNumberOfMask();
        //if (playerStat.maxHp + playerStat.lifebloodHp < maskList.Count)
        //    InitNumberOfMask();

        ////TODO: Possible room for optimization
        //// Update according to current hp
        //for (int i = 0; i < maskList.Count; i++)
        //{
        //    if (i < playerStat.currentHp)
        //        maskList[i].sprite = maskIcon;
        //    else
        //        maskList[i].sprite = emptyMaskIcon;
        //}
    }

    public void InitNumberOfMask()
    {
        // Delete all masks first
        foreach (Transform child in transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        maskList.Clear();

        for (int i = 0; i < playerStat.maxHp; i++)
        {
            Image newMask = Instantiate(maskPrefab, transform, false);
            maskList.Add(newMask);
        }

        for (int i = 0; i < playerStat.lifebloodHp; i++)
        {
            Image newLifebloodMask = Instantiate(maskPrefab, transform, false);
            newLifebloodMask.color = new Color(0f, 0.75f, 1f);
            maskList.Add(newLifebloodMask);
        }

        RecalibrateCurrentMask();
    }

    public void RecalibrateCurrentMask()
    {
        for (int i = 0; i < playerStat.maxHp; i++)
        {
            if (i < playerStat.currentHp)
                maskList[i].sprite = maskIcon;
            else
                maskList[i].sprite = emptyMaskIcon;
        }
        lastCurrentHp = playerStat.currentHp;
    }
}