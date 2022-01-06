using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** Boss design:
 * Moveset:
 * - Thow and hit a white ball, ranged attack
 * - Thow and hit a big red ball, ranged explosive attack, 2 dmg
 * - Normal attack
 * - Normal attack with a dash
 * - Charge toward player
 * - Jump and slam the ground, create shockwall and falling rock (2 dmg slam)
 * - Summon pet to attack, pet shoot laser beam
 * - Below 25% hp, move faster
 * Behaviour:
 * - Since I'm noob, most of his move will be selected randomly after he finished the previous one
 * - If player keep ledge grabbing, he will use ball attack
 * - Can only summon max 2 pet (pet can be killed in 1 hit)
 * - Can not use jump slam twice in a row
 */

public class BossBatterAI : MonoBehaviour
{
    private Player player;
    private Animator anim;
    private Transform spriteHolder;
    private Enemy stat;
    private Rigidbody2D rb;

    private bool inAttack;
    public float timeBetweenAttack;
    private float attackTimer;
    private bool isFacingLeft;

    public float dashForce;

    public enum Moveset
    { whiteBall, redBall, attack, attackDash, charge, jumpSlam, summon }
    public Moveset selectedMove;

    private void Start()
    {
        stat = GetComponent<Enemy>();
        player = GameObject.Find("Tenroh").GetComponent<Player>();
        spriteHolder = transform.Find("Sprite");
        anim = spriteHolder.GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (stat.isDead)
        {
            StopAllCoroutines();
            inAttack = false;
            this.enabled = false;
        }

        if (!inAttack)
        {
            FaceTowardPlayer();

            if (attackTimer < timeBetweenAttack)
            {
                attackTimer += Time.deltaTime;
            }
            else
            {
                SelectARandomMove();

                // Perform that attack
                switch (selectedMove)
                {
                    case Moveset.attack:
                        NormalAttack();
                        break;

                    case Moveset.attackDash:
                        DashAttack();
                        break;

                    case Moveset.charge:
                        break;

                    case Moveset.jumpSlam:
                        break;
                }
            }
        }
    }

    private void NormalAttack()
    {
        attackTimer = 0f;
        anim.SetTrigger("attack");
    }

    private void DashAttack()
    {
        attackTimer = 0f;
        anim.SetTrigger("attack");
        if (isFacingLeft)
            rb.AddForce(new Vector2(-dashForce, 10f), ForceMode2D.Impulse);
        else
            rb.AddForce(new Vector2(dashForce, 10f), ForceMode2D.Impulse);
    }

    private void FaceTowardPlayer()
    {
        if (player.transform.position.x < transform.position.x)
        {
            spriteHolder.localScale = new Vector3(1, 1, 1);
            isFacingLeft = true;
        }
        else
        {
            spriteHolder.localScale = new Vector3(-1, 1, 1);
            isFacingLeft = false;
        }
    }

    private void SelectARandomMove()
    {
        int nMove = System.Enum.GetValues(typeof(Moveset)).Length;
        selectedMove = (Moveset)Random.Range(0, nMove);
    }

    public void BeginNormalAttack()
    {
        inAttack = true;
    }

    public void EndNormalAttack()
    {
        inAttack = false;
    }

    public void AttackDealDamage()
    {
    }
}