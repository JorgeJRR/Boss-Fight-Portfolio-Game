using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModel : MonoBehaviour
{
    private Rigidbody2D rb;

    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    private bool _isGrounded = false;

    public float dashForce = 15f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 1f;
    private bool _canDash = true;
    private bool _isDashing = false;

    public GameObject swordDamage;

    public bool IsGrounded
    {
        get { return _isGrounded; }
        set { _isGrounded = value; }
    }

    public bool CanDash
    {
        get { return _canDash; }
        set { _canDash = value; }
    }

    public bool IsDashing
    {
        get { return _isDashing; }
        set { _isDashing = value; }
    }

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.LogError("PlayerModel requiere un Rigidbody2D en el mismo GameObject.");
        }
    }

    public void Move(Vector2 direction)
    {
        rb.velocity = new Vector2(direction.x * moveSpeed, rb.velocity.y);
    }

    public void Jump()
    {
        if (IsGrounded)
        {
            rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
            IsGrounded = false;
        }
    }

    public IEnumerator EnableSwordDamage()
    {
        swordDamage.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        swordDamage.SetActive(false);
    }

    public IEnumerator PerformDash(float dashDirectionX)
    {
        CanDash = false;
        IsDashing = true;

        rb.velocity = new Vector2(dashDirectionX * dashForce, 0f);

        yield return new WaitForSeconds(dashDuration);

        if (IsDashing)
        {
            rb.velocity = new Vector2(0f, rb.velocity.y);
        }

        IsDashing = false;

        yield return new WaitForSeconds(dashCooldown);
        CanDash = true;
    }
}