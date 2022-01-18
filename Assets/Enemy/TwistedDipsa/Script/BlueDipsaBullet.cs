using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueDipsaBullet : MonoBehaviour
{
    public int damage = 1;
    public int drainSilk = 2;

    private void Start()
    {
        Destroy(this.gameObject, 10f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Player player = collision.transform.GetComponent<Player>();
        if (player)
        {
            player.Damaged(damage, (player.transform.position - transform.position).normalized);
            player.playerStat.currentSilk -= 2;
            player.playerStat.currentSilk = Mathf.Clamp(player.playerStat.currentSilk, 0, player.playerStat.maxSilk);
        }
        PlayDestroyEffect();
        Destroy(this.gameObject);
    }

    private void PlayDestroyEffect()
    {
        // Instantiate a ParticleEffect on World (so it doesnt stopped
        // by this object destruction).
    }
}