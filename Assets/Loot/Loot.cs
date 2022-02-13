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
    public float animSpeed = 1;
    public Transform spriteHolder;
    private float randomTmp;

    private void Start()
    {
        if (destroyTime > 0)
            Destroy(this.gameObject, destroyTime);
        randomTmp = Random.Range(90, 270);
    }

    private void Update()
    {
        // Spinning animation
        float tmp = Mathf.Sin(randomTmp + Time.time * animSpeed);
        spriteHolder.localScale = new Vector3(tmp, 1, 1);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer != LayerMask.NameToLayer("PlayerTrigger"))
        {
            return;
        }

        Player player = collision.transform.parent.GetComponent<Player>();
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
            spriteHolder.gameObject.SetActive(false);
            this.transform.GetComponent<Collider2D>().enabled = false;
            lootSound.Play();
            Destroy(this.gameObject, 1f);
        }
    }
}