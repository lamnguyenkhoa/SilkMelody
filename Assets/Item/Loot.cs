using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loot : MonoBehaviour
{
    public enum LootEnum
    { copperShard, scaleShard };
    public LootEnum lootType;
    public int value;
    public float destroyTime = 0f; // Set to 0f if you dont want them to be destroy after some time
    public AudioSource lootSound;
    private bool pickedUp;

    private void Start()
    {
        if (destroyTime > 0)
            Destroy(this.gameObject, destroyTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.transform.GetComponent<Player>();
        if (player && !pickedUp)
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
            pickedUp = true;
            this.transform.GetComponent<SpriteRenderer>().enabled = false;
            this.transform.GetComponent<Collider2D>().enabled = false;
            lootSound.Play();
            Destroy(this.gameObject, 1f);
        }
    }
}