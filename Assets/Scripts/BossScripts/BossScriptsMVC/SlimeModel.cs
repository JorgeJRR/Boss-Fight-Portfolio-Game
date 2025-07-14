using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeModel : MonoBehaviour
{
    public Rigidbody2D rb;
    public float groundFriction = 5f;
    public float moveSpeed;

    public bool isGrounded;
    public LayerMask groundMask;

    public float maxHealth;
    public float currentHealth;

    public bool canWalk = true;
    public bool isAttacking = false;

    public static Action bossKilled;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
    }

    public void SetGrounded(bool grounded)
    {
        isGrounded = grounded;
    }

    public void Move(float directionX)
    {
        if (canWalk)
        {
            rb.velocity = new Vector2(directionX * moveSpeed, rb.velocity.y);
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        if (currentHealth <= 0f)
        {
            BossDead();
            Debug.Log("Slime Boss defeated!");
        }
        else
        {
            Debug.Log($"Slime Boss took {damage} damage. Current Health: {currentHealth}");
        }
    }

    public void SetCanWalk(bool value)
    {
        canWalk = value;
    }

    public void SetIsAttacking(bool value)
    {
        isAttacking = value;
    }

    public void BossDead()
    {
        GameManager.Instance.PlayerWinPanel();
        gameObject.SetActive(false);
    }
}