using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlimeController : MonoBehaviour
{
    public SlimeModel slimeModel;
    public SlimeView slimeView;

    public SlimeDash SlimeDash;
    public SlimeLeap SlimeLeap;
    public SlimeExpand SlimeExpand;

    public GameObject player;
    public bool playerRight;

    public Transform groundCheck;
    public LayerMask groundMask;

    public float minTimeBetweenMovements = 2f;
    public float maxTimeBetweenMovements = 5f;

    public Color dashAttackColor = Color.blue;
    public Color leapAttackColor = Color.red;
    public Color expandAttackColor = Color.black;

    public Slider healthSlider;

    private Coroutine currentAttackCoroutine;

    private void Awake()
    {
        if (slimeModel == null) slimeModel = GetComponent<SlimeModel>();
        if (slimeView == null) slimeView = GetComponent<SlimeView>();
        if (SlimeDash == null) SlimeDash = GetComponent<SlimeDash>();
        if (SlimeLeap == null) SlimeLeap = GetComponent<SlimeLeap>();
        if (SlimeExpand == null) SlimeExpand = GetComponent<SlimeExpand>();

        if (healthSlider != null && slimeModel != null)
        {
            healthSlider.maxValue = slimeModel.maxHealth;
            slimeModel.currentHealth = slimeModel.maxHealth;
            healthSlider.value = slimeModel.currentHealth;
        }
    }

    private void Start()
    {
        InvokeRepeating("ActivateRandomMovement", 2f, Random.Range(minTimeBetweenMovements, maxTimeBetweenMovements));
    }

    private void FixedUpdate()
    {
        slimeModel.SetGrounded(Physics2D.OverlapCircle(groundCheck.transform.position, 0.2f, groundMask));
        
        playerRight = transform.position.x < player.transform.position.x;

        if (slimeModel.isGrounded && slimeModel.canWalk)
        {
            slimeView.SetFacingDirection(playerRight);
            slimeModel.Move(playerRight? 1: -1);
            slimeView.PlayWalkAnimation(1);
        }
        else
        {
            slimeView.PlayWalkAnimation(-1);
        }
    }

    private void ActivateRandomMovement()
    {
        if (slimeModel.isAttacking)
        {
            return;
        }

        int randomMovement = Random.Range(1, 4);

        switch (randomMovement)
        {
            case 1:
                currentAttackCoroutine = StartCoroutine(PerformDashAttackRoutine(2f));
                break;
            case 2:
                currentAttackCoroutine = StartCoroutine(PerformLeapAttackRoutine(2f));
                break;
            case 3:
                currentAttackCoroutine = StartCoroutine(PerformExpandAndContractRoutine(2f));
                break;
            default:
                break;
        }
    }

    private IEnumerator PerformDashAttackRoutine(float delay)
    {
        slimeModel.SetIsAttacking(true);
        slimeModel.SetCanWalk(false);

        slimeView.SetSpriteColor(dashAttackColor);

        yield return new WaitForSeconds(delay);

        slimeView.PlayAttackAnimation("DashAttack");
        SlimeDash.PerformDashAttack();

        yield return new WaitForSeconds(1.5f);

        slimeView.FinishAttack();
        slimeView.ResetSpriteColor();
        slimeModel.SetCanWalk(true);
        slimeModel.SetIsAttacking(false);
    }

    private IEnumerator PerformLeapAttackRoutine(float delay)
    {
        slimeModel.SetIsAttacking(true);
        slimeModel.SetCanWalk(false);

        slimeView.SetSpriteColor(leapAttackColor);

        yield return new WaitForSeconds(delay);

        slimeView.PlayAttackAnimation("LeapAttack");
        SlimeLeap.PerformLeapAttack();

        yield return new WaitForSeconds(1.5f);

        slimeView.FinishAttack();
        slimeView.ResetSpriteColor();
        slimeModel.SetCanWalk(true);
        slimeModel.SetIsAttacking(false);
    }

    private IEnumerator PerformExpandAndContractRoutine(float delay)
    {
        slimeModel.SetIsAttacking(true);
        slimeModel.SetCanWalk(false);

        slimeView.SetSpriteColor(expandAttackColor);

        yield return new WaitForSeconds(delay);

        slimeView.PlayAttackAnimation("ExpandAttack");
        SlimeExpand.PerformExpandAndContract();

        yield return new WaitForSeconds(SlimeExpand.expandDuration);

        slimeView.FinishAttack();
        slimeView.ResetSpriteColor();
        slimeModel.SetCanWalk(true);
        slimeModel.SetIsAttacking(false);
    }

    public void TakeDamage(float damage)
    {
        slimeModel.TakeDamage(damage);

        if (healthSlider != null)
        {
            healthSlider.value = slimeModel.currentHealth;
        }
    }
}