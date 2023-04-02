using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BasicPlayerMovement : MonoBehaviour
{
    [SerializeField] private Collider playerCollider;
    [SerializeField] private CharacterController characterController;
    [SerializeField] private float movementSpeed = 2f;
    [SerializeField] private float turnSpeed = 2f;

    private Vector2 _movementInputValues, _rotationInputValues;

    private void Awake()
    {
        AssignComponents();
    }

    private void AssignComponents()
    {
        if (GetComponent<Collider>() == null) playerCollider = GetComponent<Collider>();
        if (characterController == null) characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        _movementInputValues = GetMovementInputData();
        _rotationInputValues = GetRotationMovementData();  
    }

    private void FixedUpdate()
    {
        if (!Input.anyKey) return;
        Move();
        Rotate();
    }

    private Vector2 GetMovementInputData()
    {
        return new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
    }

    private Vector2 GetRotationMovementData()
    {
        return new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
    }

    private void Move()
    {
        Vector3 move = new Vector3(_movementInputValues.x * movementSpeed * Time.fixedDeltaTime, 0,
            _movementInputValues.y * movementSpeed * Time.fixedDeltaTime);

        move = (Camera.main.transform.forward * move.z) + (Camera.main.transform.right * move.x);
        move.y = 0;

        characterController.Move(new Vector3(move.x, -1,
            move.z));
    }

    private void Rotate()
    {
        float targetAngle = Mathf.Atan2(_rotationInputValues.x, _rotationInputValues.y) * Mathf.Rad2Deg +
                            Camera.main.transform.eulerAngles.y;

        Quaternion rotation = Quaternion.Euler(0f, targetAngle, 0f);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * turnSpeed);
    }  
}
