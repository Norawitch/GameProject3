using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Player))]
public class PlayerJumping : MonoBehaviour
{
    Player player;

    [SerializeField] private float jumpSpeed = 5f;
    [SerializeField] private float jumpPressBufferTime = .05f;
    [SerializeField] private float jumpGroundGraceTime = .2f;

    bool tryingToJump;
    float lastJumpPressTime;
    float lastGroundedTime;

    // Input system
    PlayerInput playerInput;
    InputAction jumpAction;

    private void Awake()
    {
        player = GetComponent<Player>();
        playerInput = GetComponent<PlayerInput>();
        jumpAction = playerInput.actions["Jump"];
    }

    private void OnEnable()
    {
        player.OnbeforeMove += OnBeforeMove;
        player.OnGroundStateChange += OnGroundStatrChange;
    }
    private void OnDisable()
    {
        player.OnbeforeMove -= OnBeforeMove;
        player.OnGroundStateChange -= OnGroundStatrChange;
    }

    void OnJump()
    {
        tryingToJump = true;
        lastJumpPressTime = Time.time;
    }

    void OnBeforeMove()
    {
        bool wasTryingToJump = Time.time - lastJumpPressTime < jumpPressBufferTime;
        bool wasGrounded = Time.time - lastGroundedTime < jumpGroundGraceTime;
        
        bool isOrWasTryingToJump = tryingToJump || (wasTryingToJump && player.IsGrounded);
        bool isOrWasGrounded = player.IsGrounded || wasGrounded;

        if (isOrWasTryingToJump && isOrWasGrounded)
        {
            player.velocity.y += jumpSpeed;
        }
        tryingToJump = false;
    }

    void OnGroundStatrChange(bool isGrounded)
    {
        if (!isGrounded) lastGroundedTime = Time.time;
    }
}
