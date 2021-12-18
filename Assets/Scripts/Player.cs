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

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
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

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (Mathf.Abs(rb.velocity.y) < 0.01f)
            {
                Jump();
            }
        }
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