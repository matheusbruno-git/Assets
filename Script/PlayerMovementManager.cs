using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class PlayerMovementManager : MonoBehaviour
{
    [Header("Data")]
    public PlayerData playerData;
    public AudioClip LandingAudioClip;
    public AudioClip[] FootstepAudioClips;
    [Range(0, 1)] public float FootstepAudioVolume = 0.5f;
    public Camera cam;
    public CinemachineFreeLook virtualCamera;

    [Header("Animator")]
    private Animator animator;
    private CharacterController _controller;

    [Header("Input")]
    private PlayerInput playerInput;
    private InputAction moveAction;
    private InputAction sprintAction;
    private InputAction crouchAction;
    private InputAction jumpAction;
    private InputAction dodgeAction;

    [Header("Movement")]
    public float walkSpeed = 5f;
    public float sprintSpeed = 10f;
    public float crouchSpeed = 2f;
    public float jumpForce = 5f;
    public float dodgeForce = 10f;

    private Vector3 _velocity;
    private bool isSprinting = false;
    private bool isCrouching = false;
    private bool isJumping = false;
    private bool isDodging = false;
    private float dodgeCooldown = 1f; // Cooldown duration for dodging
    private float lastDodgeTime;
    private bool isMovementEnabled = true;

    private CombatManager combatManager;

    private void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        _controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        combatManager = GetComponent<CombatManager>();

        moveAction = playerInput.actions["Move"];
        sprintAction = playerInput.actions["Sprint"];
        crouchAction = playerInput.actions["Crouch"];
        jumpAction = playerInput.actions["Jump"];
        dodgeAction = playerInput.actions["Dodge"];
    }

    private void Update()
    {
        if (_controller.isGrounded)
        {
            _velocity.y = -2f; // Ensure the player sticks to the ground
            animator.SetBool("Grounded", true);
        }
        else
        {
            animator.SetBool("Grounded", false);
        }

        if (isSprinting)
        {
            AdjustCameraFOV(70f); // Zoom out when sprinting
        }
        else
        {
            AdjustCameraFOV(60f); // Normal view
        }

        if (combatManager != null && combatManager.IsInCombat()) return;

        Move();
        Sprint();
        Crouch();
        Jump();
        Dodge();

        ApplyGravity();
        _controller.Move(_velocity * Time.deltaTime);
    }

    public void DisableMovement()
    {
        isMovementEnabled = false;
    }

    public void EnableMovement()
    {
        isMovementEnabled = true;
    }

    private void Move()
    {
        if (!isMovementEnabled) return;

        Vector2 input = moveAction.ReadValue<Vector2>();
        Vector3 inputDirection = new Vector3(input.x, 0f, input.y).normalized;

        float movementSpeed = 0f;

        if (inputDirection != Vector3.zero)
        {
            Vector3 cameraForward = cam.transform.forward;
            Vector3 cameraRight = cam.transform.right;

            // Ignore vertical components
            cameraForward.y = 0f;
            cameraRight.y = 0f;

            // Calculate movement direction relative to the camera
            Vector3 moveDirection = (cameraForward * inputDirection.z + cameraRight * inputDirection.x).normalized;

            // Rotate the player to face the movement direction
            transform.forward = moveDirection;

            // Determine the movement speed
            float currentSpeed = isSprinting ? sprintSpeed : (isCrouching ? crouchSpeed : walkSpeed);
            _velocity.x = moveDirection.x * currentSpeed;
            _velocity.z = moveDirection.z * currentSpeed;

            movementSpeed = currentSpeed; // Use the actual speed for animation
        }
        else
        {
            // Reset velocity if no input
            _velocity.x = 0f;
            _velocity.z = 0f;
        }

        // Update animator with movement speed
        animator.SetFloat("Speed", movementSpeed, 0.1f, Time.deltaTime);
    }




    private void Sprint()
    {
        bool sprintPressed = sprintAction.IsPressed();

        if (sprintPressed && !isCrouching && !isJumping && !isDodging)
        {
            if (!isSprinting)
            {
                isSprinting = true;
                animator.SetBool("IsSprinting", true);
            }
        }
        else if (isSprinting)
        {
            isSprinting = false;
            animator.SetBool("IsSprinting", false);
        }
    }

    private void Crouch()
    {
        bool crouchPressed = crouchAction.IsPressed();

        if (crouchPressed && !isSprinting && !isJumping && !isDodging)
        {
            if (!isCrouching)
            {
                isCrouching = true;
                animator.SetBool("IsCrouching", true);
            }
        }
        else if (isCrouching)
        {
            isCrouching = false;
            animator.SetBool("IsCrouching", false);
        }
    }

    private void Jump()
    {
        if (jumpAction.WasPressedThisFrame() && !isJumping && _controller.isGrounded)
        {
            isJumping = true;
            _velocity.y = Mathf.Sqrt(jumpForce * -2f * Physics.gravity.y);
            animator.SetTrigger("Jump");
        }
        else if (isJumping && _controller.isGrounded)
        {
            isJumping = false;
        }
    }

    private void Dodge()
    {
        if (dodgeAction.WasPressedThisFrame() && !isDodging && Time.time - lastDodgeTime > dodgeCooldown)
        {
            isDodging = true;
            lastDodgeTime = Time.time;

            Vector3 dodgeDirection = transform.forward;
            _velocity += dodgeDirection * dodgeForce;

            animator.SetTrigger("Dodge");

            // End dodge after animation
            StartCoroutine(EndDodge());
        }
    }

    private IEnumerator EndDodge()
    {
        yield return new WaitForSeconds(0.5f);
        isDodging = false;
    }

    private void ApplyGravity()
    {
        if (!_controller.isGrounded)
        {
            _velocity.y += Physics.gravity.y * Time.deltaTime;
        }
    }

    private void AdjustCameraFOV(float targetFOV)
    {
        if (virtualCamera != null)
        {
            var lens = virtualCamera.m_Lens;
            lens.FieldOfView = targetFOV;
            virtualCamera.m_Lens = lens;
        }
    }
}
