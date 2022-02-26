using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] private LayerMask groundLayer;
    private Rigidbody2D playerRigidbody2D;
    private Collider2D boxCollider2D;
    [Header("Jump Variables")]
    [SerializeField] private float jumpVelocity = 13f;
    

    [Header("Movement Variables")]
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float midAirControl = 2.5f;
    [SerializeField] private float linearDrag = 5f;
    [SerializeField] private float groundCheckDistance;
    private float horizontalDirection;
    public Animator animator;
    private float jumpTimeCounter;
    public float jumpTime;
    private bool isJumping;
    private bool isCrouching;
    private bool Shooting;
    private bool Jumping; // For animation
    private bool groundDetected;

    [SerializeField] private Transform groundCheck;


    private void Awake()
    {
        playerRigidbody2D = GetComponent<Rigidbody2D>();
        boxCollider2D = GetComponent<Collider2D>();
    }

    private void Update()
    {
        horizontalDirection = GetInput().x;
        
        //TURN LEFT OR RIGHT
        Vector3 characterScale = transform.localScale;

        if (GetInput().x < 0)
        {
            characterScale.x = -5;
        }

        if (GetInput().x > 0)
        {
            characterScale.x = 5;
        }

        transform.localScale = characterScale;

        animator.SetBool("IsCrouching", isCrouching);
        Crouch();
        
        animator.SetBool("Shooting", Shooting);
        ShootingAnim();
        
        //Speed for animation
        float moveDirection = 0;
        if (Mathf.Abs(horizontalDirection) >= 0.0001f)
            moveDirection =  0.9f;
        animator.SetFloat("Speed", Mathf.Abs(moveDirection));
        animator.SetBool("IsJumping", isJumping);
        animator.SetBool("inAir", Jumping);
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
        groundDetected = Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, groundLayer);
        return groundDetected;
    }

    private void HandleMovement()
    {
        
            if (IsGrounded())
            {
                playerRigidbody2D.velocity = new Vector2(horizontalDirection * moveSpeed, playerRigidbody2D.velocity.y);
            }
            else
            {
                playerRigidbody2D.velocity += new Vector2(horizontalDirection * midAirControl* moveSpeed, 0f);
                playerRigidbody2D.velocity = new Vector2(Mathf.Clamp(playerRigidbody2D.velocity.x, -moveSpeed, +moveSpeed), playerRigidbody2D.velocity.y);
            }
    }

    private void Crouch()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            isCrouching = true;
          
        }
        else if (Input.GetKeyUp(KeyCode.S))
        {
            isCrouching = false;
        }
    }
    
    private void ShootingAnim()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Shooting = true;
          
        }
        else if (Input.GetMouseButtonUp(0))
        {
            Shooting = false;
        }
    }

    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            isJumping = true;
            Jumping = true;
            jumpTimeCounter = jumpTime;
            playerRigidbody2D.velocity = Vector2.up * jumpVelocity;
        }
        //Space control
        if (Input.GetKey(KeyCode.Space) && isJumping)
        {
            if (jumpTimeCounter > 0)
            {
                playerRigidbody2D.velocity = Vector2.up * jumpVelocity;
                jumpTimeCounter -= Time.deltaTime;
            }
            else
            {
                isJumping = false;
            }
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            isJumping = false;
            Jumping = false;
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

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheck.position, new Vector2(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));
    }
}

