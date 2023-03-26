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
    public int jumpsPerformed = 0;
    public int maxJumps = 2;

    //Camera Vision
    public Transform cameraFollower;
    public float mouseSensitivity = 700f;
    private float cameraVerticalRotation;

    //WallRunning
    public LayerMask wallMask;
    public float wallRunForce, maxWallRunTime, maxWallSpeed;
    bool isWallRight, isWallLeft;
    bool isWallRunning;
    public float maxWallRunCameraTilt, wallRunCameraTilt;
    public Transform orientation;

    void Start()
    {
        
    }


    void Update()
    {
        PlayerMovement();
        CameraMovement();
        Jump();
        CheckForWall();
        WallRunInput();
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

        //WallRunning
        /**
        if (isWallRunning)
        {
            canJump = false;

            //normal jump
            if (isWallLeft && !Input.GetKey(KeyCode.D) || isWallRight && !Input.GetKey(KeyCode.A))
            {
                characterController.Move(Vector2.up * jumpHeight * 1.5f);
                characterController.Move(Vector3.up * jumpHeight * 0.5f);
            }

            //sidwards wallhop
            if (isWallRight || isWallLeft && Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)) characterController.Move(-orientation.up * jumpHeight * 1f);
            if (isWallRight && Input.GetKey(KeyCode.A)) characterController.Move(-orientation.right * jumpHeight * 3.2f);
            if (isWallLeft && Input.GetKey(KeyCode.D)) characterController.Move(orientation.right * jumpHeight * 3.2f);

            //Always add forward force
            characterController.Move(orientation.forward * jumpHeight * 1f);

            velocity = Vector3.zero;
        }
        **/
    }

    void WallRunInput()
    {
        if(Input.GetKey(KeyCode.D) && isWallRight) StartWallRun();
        if(Input.GetKey(KeyCode.A) && isWallLeft) StartWallRun();
    }

    void StartWallRun()
    {
        gravityModifier = 10f;
        isWallRunning = true;

        if(velocity.magnitude <= maxWallSpeed)
        {
            characterController.Move(orientation.forward * wallRunForce * Time.deltaTime);

            if(isWallRight)
            {
                characterController.Move(orientation.right * wallRunForce / 5 * Time.deltaTime);
            }
            else
            {
                characterController.Move(-orientation.right * wallRunForce / 5 * Time.deltaTime);
            }
        }
    }

    void StopWallRun()
    {
        gravityModifier = 10f;
        isWallRunning = false;
    }

    void CheckForWall()
    {
        isWallRight = Physics.Raycast(transform.position, orientation.right, 1f, wallMask);
        isWallLeft = Physics.Raycast(transform.position, -orientation.right, 1f, wallMask);

        if(!isWallLeft && !isWallRight) StopWallRun();

        if (isWallLeft || isWallRight) jumpsPerformed = 0;
    }

    void CameraMovement()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxisRaw("Mouse Y") * mouseSensitivity * Time.deltaTime;

        cameraVerticalRotation -= mouseY;
        cameraVerticalRotation = Mathf.Clamp(cameraVerticalRotation, -90f, 90f);

        transform.Rotate(Vector3.up * mouseX);
        cameraFollower.localRotation = Quaternion.Euler(cameraVerticalRotation, 0f, 0f);

        //fix

        cameraFollower.transform.localRotation = Quaternion.Euler(cameraVerticalRotation, 0, wallRunCameraTilt);
        orientation.transform.localRotation = Quaternion.Euler(0, 0, 0);

        if (Mathf.Abs(wallRunCameraTilt) < maxWallRunCameraTilt && isWallRunning && isWallRight)
            wallRunCameraTilt += Time.deltaTime * maxWallRunCameraTilt * 2;
        if (Mathf.Abs(wallRunCameraTilt) < maxWallRunCameraTilt && isWallRunning && isWallLeft)
            wallRunCameraTilt -= Time.deltaTime * maxWallRunCameraTilt * 2;

        if (wallRunCameraTilt > 0 && !isWallRight && !isWallLeft)
            wallRunCameraTilt -= Time.deltaTime * maxWallRunCameraTilt * 2;
        if (wallRunCameraTilt < 0 && !isWallRight && !isWallLeft)
            wallRunCameraTilt += Time.deltaTime * maxWallRunCameraTilt * 2;
    }
}
