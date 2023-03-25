using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //Gravity
    public Vector3 velocity;
    public float gravityModifier = 10f;

    //Movement
    public CharacterController characterController;
    public float speed = 12.5f, runSpeed = 25f;

    //Jump
    public float jumpHeight = 20f;
    private bool canJump;
    public Transform ground;
    public LayerMask groundLayer;
    public float groundDistance = 0.1f;
    //public int jumpsPerformed = 0;
    public int jumpsPerformed = 0;
    public int maxJumps = 2;

    //Camera Vision
    public Transform cameraFollower;
    public float mouseSensitivity = 700f;
    private float cameraVerticalRotation;

    void Start()
    {
        
    }


    void Update()
    {
        PlayerMovement();
        CameraMovement();
        Jump();
    }

    void PlayerMovement()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 movement = x * transform.right + z * transform.forward;

        if(Input.GetKey(KeyCode.LeftShift))
        {
            movement = movement * runSpeed * Time.deltaTime;
        }

        else
        {
            movement = movement * speed * Time.deltaTime;
        }

        characterController.Move(movement);

        velocity.y += Physics.gravity.y * Mathf.Pow(Time.deltaTime, 2) * gravityModifier;
        
        if(characterController.isGrounded)
        {
            velocity.y = -2f;
            jumpsPerformed = 0;
        }

        characterController.Move(velocity);
    }

    void Jump()
    {
        canJump = Physics.OverlapSphere(ground.position, groundDistance, groundLayer).Length > 0;

        if(Input.GetButtonDown("Jump") && (canJump || jumpsPerformed < maxJumps))
        {
            jumpsPerformed++;
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * Physics.gravity.y) * Time.deltaTime;
        }

        //prevents the conflict of velocities between jump and movement
        characterController.Move(velocity);
    }

    void CameraMovement()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxisRaw("Mouse Y") * mouseSensitivity * Time.deltaTime;

        cameraVerticalRotation -= mouseY;
        cameraVerticalRotation = Mathf.Clamp(cameraVerticalRotation, -90f, 90f);

        transform.Rotate(Vector3.up * mouseX);
        cameraFollower.localRotation = Quaternion.Euler(cameraVerticalRotation, 0f, 0f);
    }
}
