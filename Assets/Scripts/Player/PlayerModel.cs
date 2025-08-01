using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerModel : MonoBehaviour
{
    private Rigidbody2D rb;

    public float moveSpeed = 5f;
    public float startedJumpForce = 10f;
    public float canceledJumpForce = -5f;
    public bool isGrounded = false;

    public float dashForce = 15f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 1f;
    public bool canDash = true;
    public bool isDashing = false;

    public GameObject[] swordDamage;
    public bool isAttacking = false;
    public LayerMask bossLayer;

    public float maxHealth;
    public float currentHealth;
    public Slider healthSlider;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
    }

    private void Start()
    {
        transform.position = new Vector3(-6.5f, -1.6f, 0);
    }

    public void Move(Vector2 direction)
    {
        rb.velocity = new Vector2(direction.x * moveSpeed, rb.velocity.y);
    }

    public void Jump()
    {
        if (isGrounded)
        {
            rb.AddForce(new Vector2(0f, startedJumpForce), ForceMode2D.Impulse);
            isGrounded = false;
        }
        else if (!isGrounded && rb.velocity.y > 1)
        {
            canceledJumpForce = ((rb.velocity.y) / 2) * -1;
            rb.AddForce(new Vector2(0f, canceledJumpForce), ForceMode2D.Impulse);
        }
    }

    public IEnumerator SwordAttack(bool isFacingRight)
    {
        yield return new WaitForSeconds(0.25f);

        Collider2D collider = Physics2D.OverlapCircle((isFacingRight ? swordDamage[1] : swordDamage[0]).transform.position, 0.6f, bossLayer);
        if (collider != null)
        {
            collider.GetComponent<SlimeController>().TakeDamage(20);
        }

        isAttacking = false;
    }

    public IEnumerator PerformDash(float dashDirectionX)
    {
        canDash = false;
        isDashing = true;

        rb.velocity = new Vector2(dashDirectionX * dashForce, 0f);

        yield return new WaitForSeconds(dashDuration);

        if (isDashing)
        {
            rb.velocity = new Vector2(0f, rb.velocity.y);
        }

        isDashing = false;

        yield return new WaitForSeconds(dashCooldown);

        canDash = true;
    }

    public void OnPlayerWin()
    {
        rb.gravityScale = 0;
    }

    public void TakeDamage(float damage)
    {

    }
}