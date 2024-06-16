using System.Collections;
using UnityEngine;

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

    [Header("Bools")]
    [SerializeField] private bool isSprinting;
    [SerializeField] private bool readyToJump;
    [SerializeField] private bool isJumping;
    [SerializeField] private bool isCrouching;
    [SerializeField] private bool exitingSlope;
    [SerializeField] private bool isGrounded;
    [SerializeField] public bool isCameraLocked;
    [SerializeField] private bool invisibilityUnlocked;
    [SerializeField] private bool invisibilityAvailable;
    [SerializeField] private bool jetPackUnlocked;

    [Header("Vector3")]
    [SerializeField] private Vector3 originalCameraPosition;
    [SerializeField] private Vector3 velocity;
    [SerializeField] private Vector3 moveDirection;

    [Header("Transform")]
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private Transform orientation;

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

    [Header("Trail Renderer")]
    [SerializeField] private TrailRenderer tr;


    public enum MovementState
    {
        walking,
        sprinting,
        crouching,
        air
    }

    private void Start()
    {
        invisibilityAvailable = true;
        jetPackUnlocked = false;
        invisibilityUnlocked = false;
        readyToJump = true;
        gravity = -1.0f;
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        originalHeight = playerCollider.height;
        originalCameraPosition = cameraTransform.localPosition;
    }

    private void Update()
    {
        // Ground check
        isGrounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.6f, groundMask);

        HandleInput();
        ControlSpeed();
        HandleSprint();
        UpdateState();
        HandleInvisibility();

        // Handle drag
        if (isGrounded)
        {
            rb.drag = groundDrag;
        }
        else if (OnSlope() && !exitingSlope)
        {
            rb.drag = groundDrag;
        }
        else
        {
            rb.drag = 0;
        }
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void HandleInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        // Jump
        if (Input.GetKey(jumpKey) && readyToJump && isGrounded)
        {
            readyToJump = false;
            Jump();
            isJumping = true;
            Invoke(nameof(ResetJump), jumpCooldown);
        }

        // Crouch
        if (Input.GetKeyDown(crouchKey))
        {
            Crouch();
        }

        if (Input.GetKeyUp(crouchKey))
        {
            StandUp();
        }
    }

    private void MovePlayer()
    {
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        if (OnSlope() && !exitingSlope)
        {
            rb.AddForce(GetSlopeMoveDirection() * moveSpeed * 20f, ForceMode.Force);

            if (rb.velocity.y > 0)
            {
                rb.AddForce(Vector3.down * 80f, ForceMode.Force);
            }
        }
        else if (isGrounded)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
        }
        else if (!isGrounded)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
        }

        rb.useGravity = !OnSlope();
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

    public void HandleInvisibility()
    {
        if (Input.GetKeyDown(KeyCode.E) && invisibilityAvailable && invisibilityUnlocked)
        {
            gameObject.tag = "Invisible";
            StartCoroutine(InvisibilityTimer());
        }
    }

    private IEnumerator InvisibilityTimer()
    {
        invisibilityAvailable = false;
        yield return new WaitForSeconds(6.0f);
        gameObject.tag = "Player";
        invisibilityAvailable = true;
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
        if (Input.GetKeyDown(sprintKey))
        {
            Sprint();
        }
        else if (Input.GetKeyUp(sprintKey))
        {
            Walk();
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
    }

    private void ChangeState(MovementState newState)
    {
        currentState = newState;
    }

    private void Crouch()
    {
        playerCollider.height = crouchHeight;
        cameraTransform.localPosition = new Vector3(cameraTransform.localPosition.x, crouchHeight / 2f, cameraTransform.localPosition.z);
        ChangeState(MovementState.crouching);
    }

    private void StandUp()
    {
        playerCollider.height = originalHeight;
        cameraTransform.localPosition = originalCameraPosition;
        ChangeState(MovementState.walking);
    }

    private void UpdateState()
    {
        switch (currentState)
        {
            case MovementState.crouching:
                moveSpeed = crouchSpeed;
                break;
            case MovementState.walking:
                moveSpeed = walkSpeed;
                break;
            case MovementState.sprinting:
                moveSpeed = sprintSpeed;
                break;
            default:
                if (isGrounded)
                {
                    currentState = MovementState.walking;
                }
                break;
        }
    }

    private bool OnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight * 2.0f))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < maxSlopeAngle && angle != 0;
        }
        return false;
    }

    private Vector3 GetSlopeMoveDirection()
    {
        return Vector3.ProjectOnPlane(moveDirection, slopeHit.normal).normalized;
    }

    public bool ReturnIsGrounded()
    {
        return this.isGrounded;
    }

    public bool ReturnInvisibilityStatus()
    {
        return this.invisibilityAvailable;
    }

    public void SetInvisibilityUnlock(bool value)
    {
        invisibilityUnlocked = value;
    }

    public void SetJetPackUnlock(bool value)
    {
        jetPackUnlocked = value;
    }

    public GameObject ReturnThisPlayer()
    {
        return this.gameObject;
    }

    public void SetCameraLock(bool value)
    {
        isCameraLocked = value;
    }

    public float ReturnGravity()
    {
        return this.gravity;
    }
    public bool ReturnCameraLock()
    {
        return this.isCameraLocked;
    }
}
