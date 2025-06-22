using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFlip : MonoBehaviour
{
    private Rigidbody2D rb;
    public GameObject player;

    public float bossPosition;
    public float playerPosition;
    public Vector3 originalScale;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        originalScale = transform.localScale;
    }

    void Update()
    {
        playerPosition = player.transform.position.x;
        bossPosition = transform.position.x;

        if (bossPosition < playerPosition  && rb.velocity == new Vector2(0f,0f))
        {
            transform.localScale = new Vector3 (-originalScale.x, transform.localScale.y, transform.localScale.z);
        }
        else if (bossPosition > playerPosition && rb.velocity == new Vector2(0f, 0f))
        {
            transform.localScale = new Vector3(originalScale.x, transform.localScale.y, transform.localScale.z);
        }
    }
}