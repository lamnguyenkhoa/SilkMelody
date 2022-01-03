using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaskUpgrade : MonoBehaviour
{
    public float floatingAmout;
    private Vector3 originalPosition;

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
        if (player)
        {
            player.playerStat.maxHp += 1;
            player.playerStat.currentHp = player.playerStat.maxHp;
            Destroy(this.gameObject);
        }
    }
}