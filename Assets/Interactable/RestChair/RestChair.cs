using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestChair : MonoBehaviour
{
    public Transform sittingPos;
    [SerializeField] private bool playerInRange;
    [SerializeField] private bool playerSitting;
    [SerializeField] private Player player;
    public GameObject interactText;

    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.UpArrow))
        {
            SitOnChair();
        }

        if (playerSitting && Input.GetKeyDown(KeyCode.DownArrow))
        {
            GetOffChair();
        }
    }

    private void GetOffChair()
    {
        playerSitting = false;
        player.resting = false;
        player.rb.gravityScale = player.originalGravityScale;

        // Turn off invulnerable
        int playerLayerId = LayerMask.NameToLayer("Player");
        int EnemyLayerId = LayerMask.NameToLayer("Enemy");
        int EnemyAttackLayerId = LayerMask.NameToLayer("EnemyAttack");
        Physics2D.IgnoreLayerCollision(playerLayerId, EnemyLayerId, false);
        Physics2D.IgnoreLayerCollision(playerLayerId, EnemyAttackLayerId, false);

        interactText.SetActive(true);
    }

    public void SitOnChair()
    {
        playerSitting = true;
        player.resting = true;
        player.RestChairRecovery();
        player.rb.velocity = Vector2.zero;
        player.rb.gravityScale = 0f;
        player.transform.position = sittingPos.position;
        LevelLoader.instance.UpdateSpawnPoint(this.gameObject.name);

        // Invulnerable to enemy
        int playerLayerId = LayerMask.NameToLayer("Player");
        int EnemyLayerId = LayerMask.NameToLayer("Enemy");
        int EnemyAttackLayerId = LayerMask.NameToLayer("EnemyAttack");
        Physics2D.IgnoreLayerCollision(playerLayerId, EnemyLayerId, true);
        Physics2D.IgnoreLayerCollision(playerLayerId, EnemyAttackLayerId, true);

        interactText.SetActive(false);
    }

    public void RespawnAssignToChair(Player player)
    {
        this.player = player;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        player = collision.gameObject.GetComponent<Player>();
        if (player)
        {
            playerInRange = true;
            if (!playerSitting)
                interactText.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        player = collision.gameObject.GetComponent<Player>();
        if (player)
        {
            playerInRange = false;
            interactText.SetActive(false);
        }
    }
}