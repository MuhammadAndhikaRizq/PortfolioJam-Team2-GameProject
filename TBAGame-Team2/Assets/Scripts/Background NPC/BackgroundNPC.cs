using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundNPC : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2f;
    private Vector2 moveDirection;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Initialize(Vector2 direction)
    {
        moveDirection = direction;
        // Flip sprite if moving left
        if (direction.x < 0) GetComponent<SpriteRenderer>().flipX = true;
    }

    void FixedUpdate()
    {
        rb.velocity = moveDirection * moveSpeed;
    }
}
