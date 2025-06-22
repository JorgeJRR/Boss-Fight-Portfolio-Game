using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeDash : MonoBehaviour
{
    public float dashForce = 10f;
    public Transform player;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            PerformDashAttack();
        }
    }

    public void PerformDashAttack()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();

        Vector2 dashDirection = (player.position - transform.position).normalized;

        dashDirection.y = 0;

        rb.velocity = dashDirection * dashForce;
    }
}