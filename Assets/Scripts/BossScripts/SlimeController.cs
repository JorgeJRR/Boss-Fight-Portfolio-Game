using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlimeController : MonoBehaviour
{
    private Rigidbody2D rb;
    public float groundFriction = 5f;
    public float moveSpeed;

    private bool isGrounded;
    public GameObject groundCheck;
    public LayerMask groundMask;

    public float minTimeBetweenMovements = 2f;
    public float maxTimeBetweenMovements = 5f;

    public SlimeDash SlimeDash;
    public SlimeLeap SlimeLeap;
    public SlimeExpand SlimeExpand;

    public SpriteRenderer slimeSpriteRenderer;
    public Color AttackColor;

    private bool canWalk = true;

    public float maxHealth;
    public float currentHealth;
    public Slider healthSlider;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;
    }

    private void Start()
    {
        InvokeRepeating("ActivateRandomMovement", 2f, Random.Range(minTimeBetweenMovements, maxTimeBetweenMovements));
    }

    private void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.transform.position, 0.2f, groundMask);

        if(isGrounded && canWalk)
        {
            rb.velocity = new Vector2((transform.localScale.x > 0 ? -1:1) * moveSpeed, rb.velocity.y);
        }
    }

    private void ActivateRandomMovement()
    {
        StartCoroutine(ChangeSlimeColorWithDelay(1f));

        int randomMovement = Random.Range(1, 4);

        switch (randomMovement)
        {
            case 1:
                StartCoroutine(PerformDashAttackWithDelay(2f));
                AttackColor = Color.blue;
                break;
            case 2:
                StartCoroutine(PerformLeapAttackWithDelay(2f));
                AttackColor = Color.red;
                break;
            case 3:
                StartCoroutine(PerformExpandAndContractWithDelay(2f));
                AttackColor = Color.black;
                break;
            default:
                break;
        }
    }

    private IEnumerator ChangeSlimeColorWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        slimeSpriteRenderer.color = AttackColor;
    }

    private IEnumerator PerformDashAttackWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        canWalk = false;

        SlimeDash.PerformDashAttack();

        slimeSpriteRenderer.color = Color.white;
        yield return new WaitForSeconds(1.5f);
        canWalk = true;
    }

    private IEnumerator PerformLeapAttackWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        canWalk = false;

        SlimeLeap.PerformLeapAttack();

        slimeSpriteRenderer.color = Color.white;
        yield return new WaitForSeconds(1.5f);
        canWalk = true;
    }

    private IEnumerator PerformExpandAndContractWithDelay(float delay)
    {
        
        yield return new WaitForSeconds(delay);
        canWalk = false;

        SlimeExpand.PerformExpandAndContract();

        slimeSpriteRenderer.color = Color.white;
        yield return new WaitForSeconds(1.5f);
        canWalk = true;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        healthSlider.value = currentHealth;
        if (currentHealth <= 0f)
        {
            gameObject.SetActive(false);
        }
    }
}
