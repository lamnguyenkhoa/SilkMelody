using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SilkBarUI : MonoBehaviour
{
    public PlayerStatSO playerStat;
    private Image silkBarImage;
    public Sprite[] silkBarSprites; // There should be 9

    private void Start()
    {
        InitSilkBar();
    }

    private void Update()
    {
        UpdateSilkBar();
    }

    private void InitSilkBar()
    {
        silkBarImage = GetComponent<Image>();
        if (silkBarSprites.Length != 9)
            Debug.Log("ERROR SILKBAR SPRITES != 0");
    }

    private void UpdateSilkBar()
    {
        int nSilk = playerStat.currentSilk;
        nSilk = Mathf.Clamp(nSilk, 0, 8);
        silkBarImage.sprite = silkBarSprites[nSilk];
    }
}