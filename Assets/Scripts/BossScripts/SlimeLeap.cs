using System.Collections;
using UnityEngine;

public class SlimeLeap : MonoBehaviour
{
    [Header("Configuración del Salto")]
    public float leapForce = 10f;
    public float leapAngle = 45f;
    public float maxLeapDuration = 2f;

    [Header("Configuración de la Caída")]
    public float fallDetectionDistance = 20f;
    public float fallForceMultiplier = 2f;

    [Header("Referencias")]
    public Transform player;
    public LayerMask playerLayer;

    private Rigidbody2D rb;
    private bool isLeaping = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        player = PlayerController.Instance.transform;
    }

    public void PerformLeapAttack()
    {
        if (isLeaping) return;

        StartCoroutine(LeapAttackCoroutine());
    }

    private IEnumerator LeapAttackCoroutine()
    {
        isLeaping = true;

        Vector2 directionToPlayer = (player.position - transform.position).normalized;
        float horizontalDirection = directionToPlayer.x;

        float verticalForce = leapForce * Mathf.Sin(leapAngle * Mathf.Deg2Rad);
        float horizontalForce = leapForce * Mathf.Cos(leapAngle * Mathf.Deg2Rad);

        rb.velocity = new Vector2(horizontalDirection * horizontalForce, verticalForce);

        float startTime = Time.time;

        while (rb.velocity.y > 0 && Time.time < startTime + maxLeapDuration)
        {
            yield return null;
        }

        while (isLeaping)
        {
            bool playerIsBelow = Physics2D.Raycast(transform.position, Vector2.down, fallDetectionDistance, playerLayer);

            Debug.DrawRay(transform.position, Vector2.down * fallDetectionDistance, playerIsBelow ? Color.green : Color.red);

            if (playerIsBelow)
            {
                rb.AddForce(Vector2.down * fallForceMultiplier, ForceMode2D.Force);
            }

            yield return new WaitForFixedUpdate();
        }
    }

    public void StopLeapAttack()
    {
        if (isLeaping)
        {
            StopAllCoroutines();
            isLeaping = false;

        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isLeaping && (collision.gameObject.layer == LayerMask.NameToLayer("Ground") || collision.gameObject.layer == playerLayer))
        {
            StopLeapAttack();
        }
    }
}