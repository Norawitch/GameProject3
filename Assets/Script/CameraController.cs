using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    MenuControllerInGame menuControllerInGame;

    [Header("Camera Settings")]
    [SerializeField] public Transform firstPersonCamera;
    [SerializeField] private bool isFirstPerson = true;

    public Vector2 look;
    private float mouseSensitivity;

    private Camera mainCamera;

    // Input system
    PlayerInput playerInput;
    InputAction lookAction;

    private void Awake()
    {
        menuControllerInGame = FindObjectOfType<MenuControllerInGame>();
        playerInput = GetComponent<PlayerInput>();
        lookAction = playerInput.actions["Look"];

        mainCamera = Camera.main;
    }
    private void Start()
    {
        mouseSensitivity = GameSettings.ControllerSensitivity;
    }
    private void Update()
    {
        UpdateLook();
    }

    void UpdateLook()
    {
        if (menuControllerInGame.isGamePause == false)
        {
            var lookInput = lookAction.ReadValue<Vector2>();
            look.x += lookInput.x * mouseSensitivity;
            look.y += lookInput.y * mouseSensitivity;

            look.y = Mathf.Clamp(look.y, -89f, 89f);

            firstPersonCamera.localRotation = Quaternion.Euler(-look.y, 0, 0);
            transform.localRotation = Quaternion.Euler(0, look.x, 0);

            // Update the main camera's rotation to match the current camera transform
            mainCamera.transform.position = firstPersonCamera.position;
            mainCamera.transform.rotation = firstPersonCamera.rotation;
        }
        else
        {
            // Game Pause
            mouseSensitivity = 0f;
        }
    }

    public void UpdateMouseSensitivity(float newSensitivity)
    {
        mouseSensitivity = newSensitivity;
    }
}
