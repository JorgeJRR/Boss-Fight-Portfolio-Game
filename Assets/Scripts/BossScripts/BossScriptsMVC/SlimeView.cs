using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeView : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    [SerializeField]
    private Animator animator;

    private Vector3 originalScale;
    private float lastDirectionX = 1f;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalScale = transform.localScale;
    }

    public void SetFacingDirection(float horizontalDirection)
    {
  
        if (horizontalDirection > 0.1f)
        {
            transform.localScale = new Vector3(originalScale.x, originalScale.y, originalScale.z);
            lastDirectionX = 1f;
        }
        else if (horizontalDirection < -0.1f)
        {
            transform.localScale = new Vector3(-originalScale.x, originalScale.y, originalScale.z);
            lastDirectionX = -1f;
        }
    }

    public void PlayWalkAnimation(float speed)
    {
        if (animator != null)
        {
            animator.SetFloat("Speed", Mathf.Abs(speed));
        }
    }

    public void PlayAttackAnimation(string triggerName)
    {
        if (animator != null)
        {
            animator.SetTrigger(triggerName);
        }
    }

    public void SetDashAnimation(bool isDashing)
    {
        if (animator != null)
        {
            animator.SetBool("isDashing", isDashing);
        }
    }

    public void SetLeapAnimation(bool isLeaping)
    {
        if (animator != null)
        {
            animator.SetBool("isLeaping", isLeaping);
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

    public void SetExpandVisual(bool isExpanded, Vector3 targetScale)
    {
        if (isExpanded)
        {
            transform.localScale = targetScale;
        }
        else
        {
            transform.localScale = originalScale;
        }
    }
}