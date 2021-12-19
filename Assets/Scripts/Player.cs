using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    public float verInput;
    public float horInput;
    public float moveSpeed = 7f;
    public float jumpForce = 15f;
    public bool inAttack;

    // FSM
    private enum State
    { idle, running, jumping, falling, hurt }
    [SerializeField] private State state = State.idle;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (inAttack) return;

        verInput = Input.GetAxis("Vertical");
        horInput = Input.GetAxis("Horizontal");

        // Move left
        if (horInput < 0)
        {
            Flip(true);
        }
        // Move right
        else if (horInput > 0)
        {
            Flip(false);
        }
        rb.velocity = new Vector2(horInput * moveSpeed, rb.velocity.y);

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.X))
        {
            // If on ground
            if (Mathf.Abs(rb.velocity.y) < 0.01f)
            {
                Jump();
            }
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            Attack();
        }

        AnimationControl();
    }

    private void AnimationControl()
    {
        if (rb.velocity.y > 0.1f)
        {
            state = State.jumping;
        }
        else if (rb.velocity.y < -0.1f)
        {
            state = State.falling;
        }
        else if (Mathf.Abs(rb.velocity.x) > 0.1f)
        {
            state = State.running;
        }
        else
        {
            state = State.idle;
        }

        anim.SetInteger("state", (int)state);
    }

    private void Attack()
    {
        anim.SetTrigger("attack");
    }

    public void BeginAttackAnim()
    {
        inAttack = true;
        rb.velocity = Vector2.zero;
        rb.gravityScale = 0f;
    }

    public void EndAttackAnim()
    {
        inAttack = false;
        rb.gravityScale = 3f;
    }

    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        // Play some dust particle when jump
    }

    // Flip the character sprite horizontally
    private void Flip(bool facingLeft)
    {
        if (facingLeft)
        {
            if (transform.localScale.x != -1)
            {
                transform.localScale = new Vector2(-1, 1);
                //SwitchDirectionDust();
            }
        }
        else
        {
            if (transform.localScale.x != 1)
            {
                transform.localScale = new Vector2(1, 1);
                //SwitchDirectionDust();
            }
        }
    }
}