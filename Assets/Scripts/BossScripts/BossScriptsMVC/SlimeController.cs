using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Todav�a necesario si el controlador maneja directamente el slider UI

public class SlimeController : MonoBehaviour
{
    public SlimeModel slimeModel;
    public SlimeView slimeView;

    public SlimeDash SlimeDash;
    public SlimeLeap SlimeLeap;
    public SlimeExpand SlimeExpand;

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

        if (slimeModel.isGrounded && slimeModel.canWalk)
        {
            float directionX = (transform.localScale.x > 0 ? -1 : 1);
            slimeModel.Move(directionX);
            slimeView.PlayWalkAnimation(slimeModel.moveSpeed);
            slimeView.SetFacingDirection(directionX);
        }
        else
        {
            slimeView.PlayWalkAnimation(0);
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
        slimeView.PlayAttackAnimation("DashAttack");

        yield return new WaitForSeconds(delay);

        SlimeDash.PerformDashAttack();
        slimeView.SetDashAnimation(true);

        yield return new WaitForSeconds(1.5f);

        slimeView.SetDashAnimation(false);
        slimeView.ResetSpriteColor();
        slimeModel.SetCanWalk(true);
        slimeModel.SetIsAttacking(false);
    }

    private IEnumerator PerformLeapAttackRoutine(float delay)
    {
        slimeModel.SetIsAttacking(true);
        slimeModel.SetCanWalk(false);

        slimeView.SetSpriteColor(leapAttackColor);
        slimeView.PlayAttackAnimation("LeapAttack");

        yield return new WaitForSeconds(delay);

        SlimeLeap.PerformLeapAttack();
        slimeView.SetLeapAnimation(true);

        yield return new WaitForSeconds(1.5f);

        slimeView.SetLeapAnimation(false);
        slimeView.ResetSpriteColor();
        slimeModel.SetCanWalk(true);
        slimeModel.SetIsAttacking(false);
    }

    private IEnumerator PerformExpandAndContractRoutine(float delay)
    {
        slimeModel.SetIsAttacking(true);
        slimeModel.SetCanWalk(false);

        slimeView.SetSpriteColor(expandAttackColor);
        slimeView.PlayAttackAnimation("ExpandAttack");

        yield return new WaitForSeconds(delay);

        SlimeExpand.PerformExpandAndContract();

        yield return new WaitForSeconds(SlimeExpand.expandDuration);

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