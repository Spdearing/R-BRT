using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEditor;

public class PlayerController : MonoBehaviour
{
    [Header("Floats")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float walkSpeed;
    [SerializeField] private float sprintSpeed;
    [SerializeField] private float gravity;
    [SerializeField] private float jumpForce;
    [SerializeField] private float jumpCooldown;
    [SerializeField] private float airMultiplier;
    [SerializeField] private float groundDrag;
    [SerializeField] private float crouchSpeed;
    [SerializeField] private float crouchHeight;
    [SerializeField] private float originalHeight;
    [SerializeField] private float maxSlopeAngle;
    [SerializeField] private float playerHeight;
    [SerializeField] private float horizontalInput;
    [SerializeField] private float verticalInput;

    [Header("Flash Light")]
    [SerializeField] private Light flashLight;

    [Header("Bools")]
    [SerializeField] private bool playerIsActive;
    [SerializeField] private bool onSlope;
    [SerializeField] private bool isSprinting;
    [SerializeField] private bool readyToJump;
    [SerializeField] private bool isJumping;
    [SerializeField] private bool isCrouching;
    [SerializeField] private bool exitingSlope;
    [SerializeField] private bool isGrounded;
    [SerializeField] public bool isCameraLocked;
   

    [Header("Vector3")]
    [SerializeField] private Vector3 originalCameraPosition;
    [SerializeField] private Vector3 velocity;
    [SerializeField] private Vector3 moveDirection;

    [Header("Transform")]
    [SerializeField] private Transform cameraTransform;

    [Header("RigidBody")]
    public Rigidbody rb;

    [Header("Raycast")]
    [SerializeField] private RaycastHit slopeHit;

    [Header("Keybinds")]
    [SerializeField] private KeyCode jumpKey = KeyCode.Space;
    [SerializeField] private KeyCode sprintKey = KeyCode.LeftShift;
    [SerializeField] private KeyCode crouchKey = KeyCode.LeftControl;

    [Header("Layer Mask")]
    [SerializeField] private LayerMask groundMask;

    [Header("Colliders")]
    [SerializeField] private CapsuleCollider playerCollider;

    [Header("Movement State")]
    [SerializeField] private MovementState currentState;

    [Header("Animator")]
    [SerializeField] private Animator playerAnimator;

    [Header("Audio Source")]

    [SerializeField] private AudioSource walkingSound;
    [SerializeField] private AudioSource sprintingSound;
    [SerializeField] private AudioSource jumpingSound;

    public enum MovementState
    {
        walking,
        sprinting,
        crouching,
        air
    }

    private void Start()
    {
        Setup();
    }

    private void Update()
    {
        if (playerIsActive)
        {
            // Ground check
            isGrounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.6f, groundMask);

            HandleInput();
            ControlSpeed();
            HandleSprint();
            UpdateState();

            rb.drag = isGrounded ? groundDrag : (OnSlope() && !exitingSlope ? groundDrag : 0);
        }

        TurnFlashLightOn();
        
    }

    private void FixedUpdate()
    {
        if(playerIsActive) 
        {
            MovePlayer();
        }
    }

    void Setup()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        rb.freezeRotation = true;
        groundDrag = 5.0f;
        playerCollider = GameManager.instance.ReturnPlayerCollider();
        cameraTransform = GameManager.instance.ReturnCameraTransform();
        playerAnimator = GameManager.instance.ReturnAnimator();
        walkingSound = GameManager.instance.ReturnWalkingSound();
        sprintingSound = GameManager.instance.ReturnSprintingSound();
        jumpingSound = GameManager.instance.ReturnJumpingSound();
        readyToJump = true;
        gravity = -1.0f;

        if (playerCollider != null)
        {
            originalHeight = playerCollider.height;
            originalCameraPosition = cameraTransform.localPosition;
            Debug.Log("CapsuleCollider successfully assigned.");
        }
        else
        {
            Debug.LogError("CapsuleCollider not assigned!");
        }

        
        flashLight = GameManager.instance.ReturnFlashLight();
        flashLight.enabled = false;

    }

    private void HandleInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        // Jump
        if (Input.GetKey(jumpKey) && readyToJump && isGrounded)
        {
            jumpingSound.Play();
            readyToJump = false;
            Jump();
            playerAnimator.SetBool("isJumping", true);
            isJumping = true;
            Invoke(nameof(ResetJump), jumpCooldown);
            
        }

        // Crouch
        if (Input.GetKeyDown(crouchKey))
        {
            Crouch();
            playerAnimator.SetBool("isCrouching", true);
            playerAnimator.SetBool("isCrouched", true);
        }

        if (Input.GetKeyUp(crouchKey))
        {
            StandUp();
            playerAnimator.SetBool("isCrouching", false);
            playerAnimator.SetBool("isCrouched", false);
            playerAnimator.SetBool("standingUp", true);
        }
    }

    private void MovePlayer()
    {
        
        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;

        
        forward.y = 0;
        right.y = 0;

        forward.Normalize();
        right.Normalize();

        moveDirection = forward * verticalInput + right * horizontalInput;

        if (OnSlope() && !exitingSlope)
        {
            Vector3 slopeMoveDirection = GetSlopeMoveDirection();
            rb.AddForce(slopeMoveDirection * moveSpeed * 20f, ForceMode.Force);

            if (moveDirection == Vector3.zero)
            {
                
                rb.AddForce(-slopeMoveDirection * moveSpeed * 20f, ForceMode.Force);
            }
            else
            {
                rb.AddForce(Vector3.down * 30f, ForceMode.Force);
            }
        }
        else if (isGrounded)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);

            
            if (moveDirection != Vector3.zero && !walkingSound.isPlaying && !isCrouching)
            {
                walkingSound.Play();
            }
        }
        else
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
        }

        rb.useGravity = !OnSlope();
    }

    void TurnFlashLightOn()
    {
        if(Input.GetKeyDown(KeyCode.F))
        {
            if(flashLight.enabled == false)
            {
                flashLight.enabled = true;
            }
            else 

                flashLight.enabled = false;
            
        }
        
    }

    private void ControlSpeed()
    {
        if (OnSlope() && !exitingSlope)
        {
            if (rb.velocity.magnitude > moveSpeed)
            {
                rb.velocity = rb.velocity.normalized * moveSpeed;
            }
        }
        else
        {
            Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

            if (flatVel.magnitude > moveSpeed)
            {
                Vector3 limitedVel = flatVel.normalized * moveSpeed;
                rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
            }
        }
    }

    private bool OnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight * 0.6f))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);

            if (angle < maxSlopeAngle && angle != 0)
            {
                onSlope = true;
                return true;
            }
        }
        onSlope = false;
        return false;
    }

    private Vector3 GetSlopeMoveDirection()
    {
        return Vector3.ProjectOnPlane(moveDirection, slopeHit.normal).normalized;
    }

    private void Jump()
    {
        exitingSlope = true;
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void InAir()
    {
        if (!isGrounded && isJumping)
        {
            ChangeState(MovementState.air);
        }
    }

    private void HandleSprint()
    {
       
        if (Input.GetKey(sprintKey) && (horizontalInput != 0f || verticalInput != 0))
        {
            // Start sprinting
            if (!isSprinting)
            {
                sprintingSound.Play();
                isSprinting = true;
                Sprint();
                HandleSprintAnimation();
            }
        }
        else
        {
            
            if (isSprinting)
            {
                walkingSound.pitch = 1.0f;
                isSprinting = false;
                Walk();
                HandleSprintAnimation();
            }
        }
    }

    private void HandleSprintAnimation()
    {
        if (isSprinting == true)
        {
            playerAnimator.SetBool("isSprinting", true);
        }
        else if (isSprinting == false)
        {
            playerAnimator.SetBool("isSprinting", false);

        }
    }

    private void Sprint()
    {
        ChangeState(MovementState.sprinting);
    }

    private void Walk()
    {
        ChangeState(MovementState.walking);
    }

    private void ResetJump()
    {
        readyToJump = true;
        isJumping = false;
        exitingSlope = false;
        playerAnimator.SetBool("isJumping", false);
    }

    private void ChangeState(MovementState newState)
    {
        currentState = newState;
    }

    private void Crouch()
    {
        if (playerCollider != null)
        {
            playerCollider.height = crouchHeight;
        }
        cameraTransform.localPosition = new Vector3(cameraTransform.localPosition.x, crouchHeight / 2f, cameraTransform.localPosition.z);
        ChangeState(MovementState.crouching);
    }

    private void StandUp()
    {
        if (playerCollider != null)
        {
            playerCollider.height = originalHeight;
        }
        cameraTransform.localPosition = originalCameraPosition;
        ChangeState(MovementState.walking);
    }

    private void UpdateState()
    {
        switch (currentState)
        {
            case MovementState.crouching:

                moveSpeed = crouchSpeed;
                isCrouching = true;

                break;

            case MovementState.walking:

                isCrouching = false;
                moveSpeed = walkSpeed;

                break;

            case MovementState.sprinting:

                isCrouching = false;
                moveSpeed = sprintSpeed;

                break;

            default:
                if (isGrounded)
                {
                    isCrouching = false;
                    currentState = MovementState.walking;
                }
                break;
        }
    }

    public bool ReturnCrouchingStatus(bool value)
    {
        return this.isCrouching;
    }

    public bool ReturnIsGrounded()
    {
        return this.isGrounded;
    }

    public void SetCameraLock(bool value)
    {
        isCameraLocked = value;
    }

    public float ReturnGravity()
    {
        return this.gravity;
    }

    public bool ReturnPlayerActivity()
    {
        return this.playerIsActive;
    }

    public void SetPlayerActivity(bool value)
    {
       playerIsActive = value;
    }
}
