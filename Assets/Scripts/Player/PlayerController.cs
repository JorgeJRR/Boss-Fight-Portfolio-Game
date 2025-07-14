using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;

    public PlayerModel playerModel;
    public PlayerView playerView;

    public LayerMask groundLayer;
    public Transform groundCheck;

    public LayerMask BossLayer;

    public PlayerInput playerInput;
    private InputActionMap playerActionMap;
    public Vector2 moveInput;

    public LayerMask doorLayer;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else 
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(gameObject);

        if (playerActionMap == null)
        {
            playerActionMap = playerInput.actions.FindActionMap("Player");
        }

        playerActionMap["Move"].performed += context => moveInput = context.ReadValue<Vector2>();
        playerActionMap["Move"].canceled += context => moveInput = Vector2.zero;

        playerActionMap["Jump"].started += context => OnJumpPerformed();
        playerActionMap["Jump"].canceled += context => OnJumpPerformed();

        playerActionMap["Attack"].performed += context => OnAttackPerformed();

        playerActionMap["Dash"].performed += context => OnDashPerformed();

        playerActionMap["openDoor"].started += context => OpenDoor();
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
        if (playerModel.canDash)
        {
            float dashDirectionX = moveInput.x;
            if (dashDirectionX == 0)
            {
                dashDirectionX = playerView.IsFacingRight() ? 1f : -1f;
            }

            StartCoroutine(playerModel.PerformDash(dashDirectionX));
            playerView.StartDash();
        }
    }

    void OnAttackPerformed()
    {
        if (!playerModel.isAttacking)
        {
            playerModel.isAttacking = true;
            playerView.PerformAttackVisual();
            StartCoroutine(playerModel.SwordAttack(playerView.IsFacingRight()));
        }
    }

    void OpenDoor()
    {
        if (Physics2D.OverlapCircle(transform.position, .1f, doorLayer) != null)
        {
            GameManager.Instance.OpenedDoorCurtain();
        }
    }

    public void PlayerWin()
    {
        playerView.SetWinAnimation();
        playerModel.OnPlayerWin();
        playerActionMap.Disable();
    }

    void FixedUpdate()
    {
        if (!playerModel.isDashing)
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
            playerModel.isGrounded = true;
            playerView.PlayJumpAnimation(false);
        }
        else if (((1 << collision.gameObject.layer) & BossLayer) != 0)
        {
            playerModel.TakeDamage(20);
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (((1 << collision.gameObject.layer) & groundLayer) != 0)
        {
            playerModel.isGrounded = false;
        }
    }
}