using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{

    [SerializeField] private float speed = 8f;
    [SerializeField] private float jumpingPower = 16f;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Animator animator;

    private float horizontal;
    private bool isFacingRight = true;
    public bool GetFacingRight() => isFacingRight;

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    private void Flip()
    {
        Vector3 newScale = transform.localScale;
        if(isFacingRight && horizontal < 0f)
        {
            isFacingRight = false;
            newScale.x = -1;
        }
        else if (!isFacingRight && horizontal > 0f)
        {
            isFacingRight = true;
            newScale.x = 1;
        }
        transform.localScale = newScale;
        //spriteRenderer.flipX = !isFacingRight;
    }

    public void SetHorizontal(float givenHorizontal)
    {
        horizontal = givenHorizontal;
        Flip();
        if (animator != null)
        {
            if(horizontal != 0)
            {
                animator.SetBool("Running", true);
            }
            else
            {
                animator.SetBool("Running", false);
            }
        }
    }

    public void TryJump()
    {
        if (IsGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
        }
    }

    public void HaltJump()
    {
        if (rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }
    }
}
