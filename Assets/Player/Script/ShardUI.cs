using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShardUI : MonoBehaviour
{
    public TextMeshProUGUI copperText;
    public PlayerStatSO playerStat;

    private void Update()
    {
        copperText.text = playerStat.copperShard.ToString();
    }
}