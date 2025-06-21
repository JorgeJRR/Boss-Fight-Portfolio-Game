using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public PlayerModel playerModel;
    public PlayerView playerView;

    public LayerMask groundLayer;
    public Transform groundCheck;

    public PlayerInput playerInput;
    private InputActionMap playerActionMap;
    public Vector2 moveInput;

    void Awake()
    {
        playerActionMap = playerInput.actions.FindActionMap("Player");

        playerActionMap["Move"].performed += context => moveInput = context.ReadValue<Vector2>();
        playerActionMap["Move"].canceled += context => moveInput = Vector2.zero;

        playerActionMap["Jump"].performed += context => OnJumpPerformed();

        playerActionMap["Attack"].performed += context => OnAttackPerformed();

        playerActionMap["Dash"].performed += context => OnDashPerformed();
    }

    void OnEnable()
    {
        if (playerActionMap != null)
        {
            playerActionMap.Enable();
        }
    }

    void OnDisable()
    {
        if (playerActionMap != null)
        {
            playerActionMap.Disable();
        }
    }

    void OnJumpPerformed()
    {
        playerModel.Jump();
        playerView.PlayJumpAnimation(true);
    }
    void OnDashPerformed()
    {
        if (playerModel.CanDash)
        {
            float dashDirectionX = moveInput.x;
            if (dashDirectionX == 0)
            {
                dashDirectionX = playerView.GetComponent<SpriteRenderer>().flipX ? -1f : 1f;
            }

            StartCoroutine(playerModel.PerformDash(dashDirectionX));
            playerView.StartDash();
        }
    }

    void OnAttackPerformed()
    {
        playerView.PerformAttackVisual();
    }

    void FixedUpdate()
    {
        if (!playerModel.IsDashing)
        {
            playerModel.Move(moveInput);
        }

        playerView.SetDirection(moveInput.x);
        playerView.PlayWalkAnimation(moveInput.x);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (((1 << collision.gameObject.layer) & groundLayer) != 0)
        {
            playerModel.IsGrounded = true;
            playerView.PlayJumpAnimation(false);
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (((1 << collision.gameObject.layer) & groundLayer) != 0)
        {
            playerModel.IsGrounded = false;
        }
    }
}
