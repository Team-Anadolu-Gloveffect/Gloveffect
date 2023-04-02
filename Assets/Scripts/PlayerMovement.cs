using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    float playerHeight = 2f;

    [SerializeField] Transform orientation;

    [Header("Movement")]
    [SerializeField] float moveSpeed = 6f;
    [SerializeField] float airMultiplier = 0.4f;
    float movementMultiplier = 10f;

    [Header("Sprinting")]
    [SerializeField] float walkSpeed = 4f;
    [SerializeField] float sprintSpeed = 6f;
    [SerializeField] float acceleration = 10f;

    [Header("Jumping")]
    public float jumpForce = 5f;

    public int jumpsLeft = 2;
    
    [Header("Crouch and Slide")]
    [SerializeField] float slideForce = 400f;
    [SerializeField] float slideDuration = 1f;
    [SerializeField] float slideCooldown = 2f;
    bool isSliding = false;
    float slideTimer = 0f;
    float slideCooldownTimer = 0f;
    Vector3 originalScale;
    
    [Header("Kick")]
    [SerializeField] float kickForce = 300f;
    [SerializeField] float kickCooldown = 2f;
    bool isKicking = false;
    float kickCooldownTimer = 0f;
    
    [Header("Keybinds")]
    [SerializeField] KeyCode crouchKey = KeyCode.LeftControl;
    [SerializeField] KeyCode kickKey = KeyCode.C;
    [SerializeField] KeyCode jumpKey = KeyCode.Space;
    [SerializeField] KeyCode sprintKey = KeyCode.LeftShift;

    [Header("Drag")]
    [SerializeField] float groundDrag = 6f;
    [SerializeField] float airDrag = 2f;

    float horizontalMovement;
    float verticalMovement;

    [Header("Ground Detection")]
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask groundMask;
    [SerializeField] float groundDistance = 0.2f;
    public bool isGrounded { get; private set; }

    Vector3 moveDirection;
    Vector3 slopeMoveDirection;

    Rigidbody rb;

    RaycastHit slopeHit;

    private bool OnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight / 2 + 0.5f))
        {
            if (slopeHit.normal != Vector3.up)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        return false;
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        originalScale = transform.localScale;
    }

    private void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        MyInput();
        ControlDrag();
        ControlSpeed(); 
        Jump();
        Crouch();
        Slide();
        Kick();

        slopeMoveDirection = Vector3.ProjectOnPlane(moveDirection, slopeHit.normal);
    }

    void MyInput()
    {
        horizontalMovement = Input.GetAxisRaw("Horizontal");
        verticalMovement = Input.GetAxisRaw("Vertical");

        moveDirection = orientation.forward * verticalMovement + orientation.right * horizontalMovement;
    }

    void Jump()
    {
        if (Input.GetKeyDown(jumpKey) && isGrounded)
        {
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
            jumpsLeft--;
        }
        else if (!Input.GetKeyDown(jumpKey) && isGrounded)
        {
            jumpsLeft = 1;
        }
        else if (Input.GetKeyDown(jumpKey) && !isGrounded && jumpsLeft > 0)
        {
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
            jumpsLeft--;
        }
    }

    void ControlSpeed()
    {
        if (Input.GetKey(sprintKey) && isGrounded)
        {
            moveSpeed = Mathf.Lerp(moveSpeed, sprintSpeed, acceleration * Time.deltaTime);
        }
        else
        {
            moveSpeed = Mathf.Lerp(moveSpeed, walkSpeed, acceleration * Time.deltaTime);
        }
    }

    void ControlDrag()
    {
        if (isGrounded)
        {
            rb.drag = groundDrag;
        }
        else
        {
            rb.drag = airDrag;
        }
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    void MovePlayer()
    {
        if (isGrounded && !OnSlope())
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * movementMultiplier, ForceMode.Acceleration);
        }
        else if (isGrounded && OnSlope())
        {
            rb.AddForce(slopeMoveDirection.normalized * moveSpeed * movementMultiplier, ForceMode.Acceleration);
        }
        else if (!isGrounded)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * movementMultiplier * airMultiplier, ForceMode.Acceleration);
        }
    }
    
    void Crouch()
    {
        if (Input.GetKey(crouchKey) && isGrounded)
        {
            transform.localScale = new Vector3(33, 16.5f, 33);
        }
        else if (!Input.GetKey(crouchKey) && isGrounded)
        {
            transform.localScale = originalScale;
        }
    }

    void Slide()
    {
        if (Input.GetKeyDown(crouchKey) && !isSliding && slideCooldownTimer <= 0f)
        {
            isSliding = true;
            slideTimer = 0f;
            rb.AddForce(moveDirection.normalized * slideForce, ForceMode.Impulse);
            transform.localScale = new Vector3(1, 0.5f, 1);
        }

        if (isSliding)
        {
            slideTimer += Time.deltaTime;
            if (slideTimer >= slideDuration)
            {
                isSliding = false;
                slideCooldownTimer = slideCooldown;
                transform.localScale = originalScale;
            }
        }

        if (!isSliding && slideCooldownTimer > 0f)
        {
            slideCooldownTimer -= Time.deltaTime;
        }
    }
    void Kick()
    {
        if (Input.GetKeyDown(kickKey) && !isKicking && kickCooldownTimer <= 0f)
        {
            isKicking = true;
            kickCooldownTimer = kickCooldown;
            //anim.SetTrigger("Kick");
        }

        if (isKicking)
        {
            rb.AddForce(moveDirection.normalized * kickForce, ForceMode.Impulse);
            isKicking = false;
        }

        if (kickCooldownTimer > 0f)
        {
            kickCooldownTimer -= Time.deltaTime;
        }
    }
}