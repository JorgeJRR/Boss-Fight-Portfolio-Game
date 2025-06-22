using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PlayerView : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    [SerializeField]
    private Animator animator;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void SetDirection(float directionX)
    {
        if (spriteRenderer != null)
        {
            if (directionX > 0)
            {
                spriteRenderer.flipX = false;
            }
            else if (directionX < 0)
            {
                spriteRenderer.flipX = true;
            }
        }
    }

    public void PlayWalkAnimation(float move) { animator.SetFloat("Speed", Mathf.Abs(move)); }
    public void PlayJumpAnimation(bool jump) { animator.SetBool("isJumping", jump); }

    public void PerformAttackVisual()
    {

        animator.SetTrigger("Attack");

    }

    public void StartDash()
    {
        animator.SetBool("isDashing", true);
    }

    public void FinishDash()
    {
        animator.SetBool("isDashing", false);
    }
}