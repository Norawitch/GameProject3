using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Player))]
public class PlayerCrouching : MonoBehaviour
{
    [SerializeField] private float crouchHeight = 1f;
    [SerializeField] private float crouchTransitionSpeed = .5f;
    [SerializeField] private float crouchSpeedMultiplier = .5f;

    CameraController cameraController;
    Player player;
    PlayerInput playerInput;
    InputAction crouchAction;

    Vector3 initialCameraPosition;
    private float currentHeight;
    private float standingHeight;

    bool IsCrouching => standingHeight - currentHeight > .1f;

    private void Awake()
    {
        cameraController = GetComponent<CameraController>();
        player = GetComponent<Player>();
        playerInput = GetComponent<PlayerInput>();
        crouchAction = playerInput.actions["Crouch"];
    }
    private void Start()
    {
        initialCameraPosition = cameraController.firstPersonCamera.transform.localPosition;
        standingHeight = currentHeight = player.Height;
    }

    private void OnEnable()
    {
        player.OnbeforeMove += OnBeforeMove;
    }
    private void OnDisable()
    {
        player.OnbeforeMove -= OnBeforeMove;
    }

    void OnBeforeMove()
    {
        var isTryingToCrouch = crouchAction.ReadValue<float>() > 0;

        var heightTarget = isTryingToCrouch ? crouchHeight : standingHeight;

        if (IsCrouching && !isTryingToCrouch)
        {
            var castOrigin = transform.position + new Vector3(0, currentHeight / 2, 0);
            if (Physics.Raycast(castOrigin, Vector3.up, out RaycastHit hit, 0.2f))
            {
                var distanceToCeiling = hit.point.y - castOrigin.y;
                heightTarget = Mathf.Max
                (
                    currentHeight + distanceToCeiling - 0.1f,
                    crouchHeight
                );
            }
        }

        if (!Mathf.Approximately(heightTarget, currentHeight))
        {
            var crouchDelta = Time.deltaTime * crouchTransitionSpeed;
            currentHeight = Mathf.Lerp(currentHeight, heightTarget, crouchDelta);

            var halfHeightDifference = new Vector3(0, (standingHeight - currentHeight) / 2, 0);
            var newCameraPosition = initialCameraPosition - halfHeightDifference;

            cameraController.firstPersonCamera.localPosition = newCameraPosition;
            player.Height = currentHeight;
        }

        if (IsCrouching)
        {
            player.movementSpeedMultiplier *= crouchSpeedMultiplier;
        }
    }
}
