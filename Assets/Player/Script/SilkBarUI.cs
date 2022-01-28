using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SilkBarUI : MonoBehaviour
{
    private PlayerData playerStat;
    public Sprite silkIcon;
    public Sprite emptyIcon;
    private List<Image> silkList = new List<Image>();
    public Image silkPrefab;

    private void Awake()
    {
        playerStat = GameMaster.instance.playerData;
        InitNumberOfSilk();
    }

    private void Update()
    {
        UpdateSilkUI();
    }

    public void UpdateSilkUI()
    {
        // Update according to max silk
        if (playerStat.maxSilk != silkList.Count)
            InitNumberOfSilk();

        // Update according to current silk
        for (int i = 0; i < silkList.Count; i++)
        {
            if (i < playerStat.currentSilk)
                silkList[i].sprite = silkIcon;
            else
                silkList[i].sprite = emptyIcon;
        }
    }

    public void InitNumberOfSilk()
    {
        // Delete all silks first
        foreach (Transform child in transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        silkList.Clear();

        // Added left side silk bar
        for (int i = 0; i < playerStat.maxSilk; i++)
        {
            Image newSilk = Instantiate(silkPrefab, transform, false);
            silkList.Add(newSilk);
        }
    }

    public void IncreaseNumberOfSilk()
    {
        while (silkList.Count < playerStat.maxSilk)
        {
            Image newSilk = Instantiate(silkPrefab, transform, false);
            silkList.Add(newSilk);
        }
    }
}