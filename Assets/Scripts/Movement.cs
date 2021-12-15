using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] private LayerMask groundLayer;
    private Rigidbody2D playerRigidbody2D;
    private Collider2D boxCollider2D;
    [Header("Jump Variables")]
    [SerializeField] private float jumpVelocity = 100f;
    

    [Header("Movement Variables")]
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float midAirControl = 1.5f;
    [SerializeField] private float linearDrag = 5f;
    private float horizontalDirection;

    private void Awake()
    {
        playerRigidbody2D = GetComponent<Rigidbody2D>();
        boxCollider2D = GetComponent<Collider2D>();
    }

    private void Update()
    {
        horizontalDirection = GetInput().x;
        Jump();
    }

    private void FixedUpdate()
    {
        HandleMovement();
        ApplyLinearDrag();
    }

    private Vector2 GetInput()
    {
        return new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
    }

    private bool IsGrounded()
    {
        RaycastHit2D raycastHit2d = Physics2D.BoxCast(boxCollider2D.bounds.center, boxCollider2D.bounds.size, 0f, Vector2.down, 0.1f, groundLayer);
        return raycastHit2d.collider != null;
    }

    private void HandleMovement()
    {
        
            if (IsGrounded())
            {
                playerRigidbody2D.velocity = new Vector2(horizontalDirection * moveSpeed, playerRigidbody2D.velocity.y);
            }
            else
            {
                playerRigidbody2D.velocity += new Vector2(horizontalDirection * midAirControl * Time.deltaTime * moveSpeed, 0f);
                playerRigidbody2D.velocity = new Vector2(Mathf.Clamp(playerRigidbody2D.velocity.x, -moveSpeed, +moveSpeed), playerRigidbody2D.velocity.y);
            }
    }

    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            playerRigidbody2D.velocity = Vector2.up * jumpVelocity;
        }
    }

    private void ApplyLinearDrag()
    {
        if (Mathf.Abs(horizontalDirection) < 0.4f)
        {
            playerRigidbody2D.drag = linearDrag;
        }
        else
        {
            playerRigidbody2D.drag = 0f;
        }
    }
}

