using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShardUI : MonoBehaviour
{
    public TextMeshProUGUI copperText;
    public PlayerData playerStat;

    private void Start()
    {
        playerStat = GameMaster.instance.playerData;
    }

    private void Update()
    {
        copperText.text = playerStat.copperShard.ToString();
    }
}