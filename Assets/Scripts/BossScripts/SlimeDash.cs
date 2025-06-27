using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeDash : MonoBehaviour
{
    public float dashForce = 10f;
    public Transform player;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void PerformDashAttack()
    {
        if (rb == null) rb = GetComponent<Rigidbody2D>();

        Vector2 dashDirection = (player.position - transform.position).normalized;
        dashDirection.y = 0;

        rb.velocity = dashDirection * dashForce;
    }
}