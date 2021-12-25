using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueDipsaBullet : MonoBehaviour
{
    public int damage = 1;

    private void Start()
    {
        Destroy(this.gameObject, 10f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Player player = collision.transform.GetComponent<Player>();
        if (player)
        {
            // Effect here
            player.Damaged(damage, (player.transform.position - transform.position).normalized);
            Debug.Log("Player hit by blue dipsa bullet! Drain mana now!");
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