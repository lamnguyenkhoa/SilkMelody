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

    private void Start()
    {
        playerStat = GameMaster.instance.playerData;
        InitNumberOfMask();
    }

    private void Update()
    {
        UpdateHealthUI();
    }

    public void UpdateHealthUI()
    {
        // Update according to max hp
        if (playerStat.maxHp > maskList.Count)
            IncreaseNumberOfMask();
        if (playerStat.maxHp < maskList.Count)
            InitNumberOfMask();

        // Update according to current hp
        for (int i = 0; i < maskList.Count; i++)
        {
            if (i < playerStat.currentHp)
                maskList[i].sprite = maskIcon;
            else
                maskList[i].sprite = emptyMaskIcon;
        }
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
    }

    public void IncreaseNumberOfMask()
    {
        while (maskList.Count < playerStat.maxHp)
        {
            Image newMask = Instantiate(maskPrefab, transform, false);
            maskList.Add(newMask);
        }
    }
}