using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeLeap : MonoBehaviour
{
    public float leapForce = 10f;
    public float leapAngle = 45f;
    public float fallForce = 10;

    private bool bossUpPlayer;
    public Transform player;
    public LayerMask playerLayer;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        bossUpPlayer = Physics2D.Raycast(transform.position, Vector2.down, 20f, playerLayer);
    }

    private void FixedUpdate()
    {
        if (bossUpPlayer)
        {
            rb.velocity = new Vector2(0, rb.velocity.y - 5); 
        }
    }

    public void PerformLeapAttack()
    {
        if (rb == null) rb = GetComponent<Rigidbody2D>();

        Vector2 jumpDirection = (player.position - transform.position).normalized;

        float verticalForce = leapForce * Mathf.Sin(leapAngle * Mathf.Deg2Rad);
        float horizontalForce = leapForce * Mathf.Cos(leapAngle * Mathf.Deg2Rad);

        rb.velocity = new Vector2(jumpDirection.x * horizontalForce, verticalForce);
    }
}