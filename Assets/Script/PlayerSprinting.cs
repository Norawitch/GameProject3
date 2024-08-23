using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Player))]
public class PlayerSprinting : MonoBehaviour
{
    [SerializeField] float speedMultiplier = 2f;
    
    Player player;
    PlayerInput playerInput;
    InputAction sprintAction;

    private void Awake()
    {
        player = GetComponent<Player>();
        playerInput = GetComponent<PlayerInput>();
        sprintAction = playerInput.actions["Sprint"];
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
        var sprintInput = sprintAction.ReadValue<float>();
        if (sprintInput == 0) return;
        var forwardMovementFactor = Mathf.Clamp01(
            Vector3.Dot(player.transform.forward, player.velocity.normalized)
        );
        var multiplier = Mathf.Lerp(1f, speedMultiplier, forwardMovementFactor);
        player.movementSpeedMultiplier *= multiplier;
    }
}
