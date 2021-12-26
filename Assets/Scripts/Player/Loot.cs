using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loot : MonoBehaviour
{
    public enum LootEnum
    { copperShard, scaleShard };
    public LootEnum lootType;
    public int value;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Player player = collision.transform.GetComponent<Player>();
        if (player)
        {
            switch (lootType)
            {
                case LootEnum.copperShard:
                    player.playerStat.copperShard += value;
                    break;

                case LootEnum.scaleShard:
                    player.playerStat.scaleShard += value;
                    break;

                default:
                    Debug.Log("Unexpected default case");
                    break;
            }
            Destroy(this.gameObject);
        }
    }
}