using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [Header("Player controlls")]
    [SerializeField] private float movementSpeed = 5f;
    [SerializeField] private float mass = 1f;
    [SerializeField] private float acceration = 20f;

    [SerializeField] float worldBottomBoundary = -10f;

    CharacterController controller;
    CameraController cameraController;

    public event Action OnbeforeMove;
    public event Action<bool> OnGroundStateChange;
    public bool IsGrounded => controller.isGrounded;
    bool wasGrounded;

    internal float movementSpeedMultiplier;
    internal Vector3 velocity;

    (Vector3, Quaternion) initialPositionAndRotation;

    public float Height
    {
        get => controller.height;
        set => controller.height = value;
    }

    // Input system
    PlayerInput playerInput;
    InputAction moveAction;

    private void Awake()
    {
        cameraController = GetComponent<CameraController>();
        controller = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();
        moveAction = playerInput.actions["Move"];
    }
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        initialPositionAndRotation = (transform.position, transform.rotation);
    }

    public void Teleport(Vector3 position, Quaternion rotation)
    {
        transform.position = position;
        Physics.SyncTransforms();
        cameraController.look.x = rotation.eulerAngles.y;
        cameraController.look.y = rotation.eulerAngles.z;
        velocity = Vector3.zero;
    }

    private void Update()
    {
        UpdateGround();
        UpdateGravity();
        UpdateMovement();
        CheckBounds();
    }
    void CheckBounds()
    {
        if (transform.position.y < worldBottomBoundary)
        {
            var (position, rotation) = initialPositionAndRotation;
            Teleport(position, rotation);
        }
    }
    void UpdateGround()
    {
        if (wasGrounded != IsGrounded)
        {
            OnGroundStateChange?.Invoke(IsGrounded);
            wasGrounded = IsGrounded;
        }
    }
    void UpdateGravity()
    {
        var gravity = Physics.gravity * mass * Time.deltaTime;
        velocity.y = controller.isGrounded ? -1f : velocity.y + gravity.y;
    }
    Vector3 GetMovementInput()
    {
        var moveInput = moveAction.ReadValue<Vector2>();

        var input = new Vector3();
        input += transform.forward * moveInput.y;
        input += transform.right * moveInput.x;
        input = Vector3.ClampMagnitude(input, 1f);
        input *= movementSpeed * movementSpeedMultiplier;

        return input;
    }
    void UpdateMovement()
    { 
        movementSpeedMultiplier = 1f;
        OnbeforeMove?.Invoke();
        
        var input = GetMovementInput();

        var factor = acceration * Time.deltaTime;
        velocity.x = Mathf.Lerp(velocity.x, input.x, factor);
        velocity.z = Mathf.Lerp(velocity.z, input.z, factor);

        controller.Move(velocity * Time.deltaTime);
    }

}
