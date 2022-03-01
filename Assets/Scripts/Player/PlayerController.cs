using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private LayerMask groundLayer;
    private Rigidbody2D playerRigidbody2D;
    private Collider2D boxCollider2D;
    [Header("Jump Variables")]
    [SerializeField] private float jumpVelocity = 13f;
    
    [Header("Dash Variables")]
    [SerializeField] private float dashVelocity = 5f;
    

    [Header("Movement Variables")]
    [SerializeField] private float moveSpeed = 7f,
     midAirControl = 2.5f,
     linearDrag = 5f,
     groundCheckDistance,
     knockbackDuration;
    
    private float horizontalDirection;
    public Animator animator;
    
    public float jumpTime;
    public float dashTime;
    
    private bool 
        isJumping,
        isCrouching,
        Shooting, 
        Jumping, // For animation
        groundDetected,
        isDashing,
        knockback;
    
    private float 
        dashTimeCounter,
        jumpTimeCounter,
        knockbackStartTime;

    [SerializeField] private Transform groundCheck;
    [SerializeField] private Vector2 knockbackSpeed;

    private PlayerStats PS;


    private void Awake()
    {
        playerRigidbody2D = GetComponent<Rigidbody2D>();
        boxCollider2D = GetComponent<Collider2D>();
    }

    private void Start()
    {
        PS = GetComponent<PlayerStats>();
    }

    private void Update()
    {
        if (!PS.isDead)
        {
            horizontalDirection = GetInput().x;
        
            //TURN LEFT OR RIGHT
            Vector3 characterScale = transform.localScale;

            if (GetInput().x < 0)
            { characterScale.x = -5;
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
            animator.SetBool("IsDashing", isDashing);
            Dash();
            Jump();
        
            CheckKnockback();
        }
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
        
            if (IsGrounded() && !knockback)
            {
                playerRigidbody2D.velocity = new Vector2(horizontalDirection * moveSpeed, playerRigidbody2D.velocity.y);
            }
            else if(!knockback)
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

    private void Dash()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && !isDashing)
        {
            isDashing = true;
            dashTimeCounter = dashTime;
            //moveSpeed += 2f;
        }

        if (Input.GetKey(KeyCode.LeftShift) && isDashing)
        {
            if (dashTimeCounter > 0)
            {
                moveSpeed = 10f;
                dashTimeCounter -= Time.deltaTime;
            }
            else
            {
                moveSpeed = 7f;
                isDashing = false;
            }
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            moveSpeed = 7f;
            isDashing = false;
        }
    }
    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) ^ Input.GetKeyDown(KeyCode.W) && IsGrounded())
        {
            isJumping = true;
            Jumping = true;
            jumpTimeCounter = jumpTime;
            playerRigidbody2D.velocity = Vector2.up * jumpVelocity;
        }
        //Space control
        if (Input.GetKey(KeyCode.Space) ^ Input.GetKey(KeyCode.W) && isJumping)
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

        if (Input.GetKeyUp(KeyCode.Space) ^ Input.GetKeyUp(KeyCode.W))
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

    public void Knockback(int direction)
    {
        knockback = true;
        knockbackStartTime = Time.time;
        playerRigidbody2D.velocity = new Vector2(knockbackSpeed.x * direction, knockbackSpeed.y);
    }

    private void CheckKnockback()
    {
        if (Time.time >= knockbackStartTime + knockbackDuration && knockback)
        {
            knockback = false;
            playerRigidbody2D.velocity = new Vector2(0.0f, playerRigidbody2D.velocity.y);
        }
    }

    private void Damage(float[] attackDetails)
    {
        if (!isDashing && !PS.isDead)
        {
            int direction;
        
            //Damage player here using attackDetails[0]
            PS.DecreaseHealth(attackDetails[0]);
        
            if (attackDetails[1] < transform.position.x)
            {
                direction = 1;
            }
            else
            {
                direction = -1;
            }
        
            Knockback(direction);
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheck.position, new Vector2(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));
    }
}

