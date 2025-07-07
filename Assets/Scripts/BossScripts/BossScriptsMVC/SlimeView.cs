using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeView : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    [SerializeField]
    private Animator animator;

    private Vector3 originalScale;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalScale = transform.localScale;
    }

    public void SetFacingDirection(bool playerRight)
    {
        if (playerRight)
        {
            spriteRenderer.flipX = false;
        }
        else
        {
            spriteRenderer.flipX = true;
        }
    }

    public void SetGrounded(bool grounded)
    {
        animator.SetBool("Grounded", grounded);
    }

    public void PlayWalkAnimation(float speed)
    {
        if (animator != null)
        {
            animator.SetFloat("Speed", speed);
        }
    }

    public void PlayAttackAnimation(string triggerName)
    {
        if (animator != null)
        {
            animator.SetTrigger(triggerName);
            animator.SetBool("IsAttacking", true);
        }
    }

    public void FinishAttack()
    {
        if (animator != null)
        {
            animator.SetBool("IsAttacking", false);
        }
    }

    public void SetSpriteColor(Color color)
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.color = color;
        }
    }

    public void ResetSpriteColor()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.color = Color.white;
        }
    }
}