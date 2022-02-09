using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaskUpgrade : MonoBehaviour
{
    public float floatingAmout;
    private Vector3 originalPosition;
    private bool used;
    public int maskId;
    public WorldData worldState;

    private void Awake()
    {
        worldState = GameMaster.instance.worldData;
        if (worldState.maskUpgrades[maskId])
        {
            Destroy(this.gameObject);
        }
    }

    private void Start()
    {
        originalPosition = transform.position;
    }

    private void Update()
    {
        transform.position = originalPosition + new Vector3(0, Mathf.Sin(Time.time) * floatingAmout, 0);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.GetComponent<Player>();
        if (player && !used)
        {
            worldState.maskUpgrades[maskId] = true;
            used = true;
            player.playerStat.maxHp += 1;
            player.playerStat.currentHp = player.playerStat.maxHp;
            NotifyCanvas.instance.AddItemNotifyBox(GetComponent<SpriteRenderer>().sprite, "Mask upgrade");
            Destroy(this.gameObject);
        }
    }
}
