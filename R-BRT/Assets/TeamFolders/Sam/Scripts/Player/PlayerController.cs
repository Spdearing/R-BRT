using System.Collections;
using UnityEngine;
using UnityEngine.UI;

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
    [SerializeField] private bool onSlope;
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
    [SerializeField] private bool invisibilityMeterFillingBackUp;
    [SerializeField] private bool usingInvisibility;

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

    [Header("Images")]
    [SerializeField] private Image invisibilityVisualMeter;
    [SerializeField] private Image invisibilityVisualEmpty;
    [SerializeField] private Image invisibilityVisualAmount;


    [Header("Invisibility")]
    [SerializeField] private Image invisibleMeter;
    [SerializeField] float maxInvisible;
    [SerializeField] float invisibleIncrement;
    [SerializeField] float startingInvisible;

    [Header("Trail Renderer")]
    [SerializeField] private TrailRenderer tr;

    [Header("Animator")]
    [SerializeField] private Animator playerAnimator;

    public enum MovementState
    {
        walking,
        sprinting,
        crouching,
        air
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        rb.freezeRotation = true;

        groundDrag = 5.0f;

        usingInvisibility = false;
        invisibilityAvailable = true;
        jetPackUnlocked = false;
        invisibilityUnlocked = false;
        readyToJump = true;
        gravity = -1.0f;

        maxInvisible = 7.5f;
        invisibleIncrement = .25f;
        startingInvisible = 7.5f;
        invisibilityMeterFillingBackUp = false;

        if (playerCollider == null)
        {
            playerCollider = GameObject.Find("Player").GetComponentInChildren<CapsuleCollider>();
        }

        if (cameraTransform == null)
        {
            cameraTransform = GameObject.Find("Main Camera").GetComponent<Transform>();
        }

        if (playerAnimator == null)
        {
            playerAnimator = GameObject.Find("R-BRT-UI-Done").GetComponent<Animator>();
        }

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

        HideInvisibilityMeter();
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
        InvisibilityMeter();
        InvisibilityMeterFillingBackUp();

        rb.drag = isGrounded ? groundDrag : (OnSlope() && !exitingSlope ? groundDrag : 0);
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
        // Use the camera's forward and right vectors for movement direction
        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;

        // Remove the y component to keep the movement horizontal
        forward.y = 0;
        right.y = 0;

        forward.Normalize();
        right.Normalize();

        moveDirection = forward * verticalInput + right * horizontalInput;

        if (OnSlope() && !exitingSlope)
        {
            Vector3 slopeMoveDirection = GetSlopeMoveDirection();
            rb.AddForce(slopeMoveDirection * moveSpeed * 20f, ForceMode.Force);

            // Apply additional downward force to ensure player stays grounded
            if (rb.velocity.y > 0 )
            {
                rb.AddForce(Vector3.down * 30f, ForceMode.Force);
            }
            else if (rb.velocity.y < 0)
            {
                rb.AddForce(Vector3.down * 60f, ForceMode.Force);
            }
        }
        else if (isGrounded)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
        }
        else
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
        if (Input.GetKeyDown(sprintKey))
        {
            if (isSprinting == false)
            {
                isSprinting = true;
            }

            Sprint();
            HandleSprintAnimation();

        }
        else if (Input.GetKeyUp(sprintKey))
        {
            Walk();
            isSprinting = false;
            HandleSprintAnimation();

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



    public void HandleInvisibility()
    {
        if (Input.GetKeyDown(KeyCode.E) && invisibilityAvailable && invisibilityUnlocked && !invisibilityMeterFillingBackUp)
        {
            gameObject.tag = "Invisible";
            usingInvisibility = true;
            StartCoroutine(InvisibilityTimer());
        }
    }

    private IEnumerator InvisibilityTimer()
    {
        invisibilityAvailable = false;
        yield return new WaitForSeconds(6.0f);
        Debug.Log("Invisibility Over (Coroutine)");
    }

    public void InvisibilityMeter()
    {
        if(!invisibilityAvailable) 
        {
            startingInvisible -= 5.0f * Time.deltaTime * invisibleIncrement;
            startingInvisible = Mathf.Clamp(startingInvisible, 0, maxInvisible);
            invisibleMeter.fillAmount = startingInvisible / maxInvisible;

            if (startingInvisible <= 0)
            {
                Debug.Log("Invisibility Over (decrement)");
                startingInvisible = 0;
                invisibilityAvailable = true;
                invisibilityMeterFillingBackUp = true;
                gameObject.tag = "Player";
            }
        }
        
    }

    public void HideInvisibilityMeter()
    {
        Color amountColor = invisibilityVisualAmount.color;
        amountColor.a = 0.0f;
        invisibilityVisualAmount.color = amountColor;

        Color emptyColor = invisibilityVisualEmpty.color;
        emptyColor.a = 0.0f;
        invisibilityVisualEmpty.color = emptyColor;

        Color meterColor = invisibilityVisualMeter.color;
        meterColor.a = 0.0f;
        invisibilityVisualMeter.color = meterColor;
    }

    public void DisplayInvisibilityMeter()
    {
        Color amountColor = invisibilityVisualAmount.color;
        amountColor.a = 255.0f;
        invisibilityVisualAmount.color = amountColor;

        Color emptyColor = invisibilityVisualEmpty.color;
        emptyColor.a = 255.0f;
        invisibilityVisualEmpty.color = emptyColor;

        Color meterColor = invisibilityVisualMeter.color;
        meterColor.a = 255.0f;
        invisibilityVisualMeter.color = meterColor;
    }

    public void InvisibilityMeterFillingBackUp()
    {
        if(invisibilityMeterFillingBackUp)
        {
            startingInvisible += 2.5f * Time.deltaTime * invisibleIncrement;
            startingInvisible = Mathf.Clamp(startingInvisible, 0, maxInvisible);
            invisibleMeter.fillAmount = startingInvisible / maxInvisible;

            if (startingInvisible >= 7.5f)
            {
                startingInvisible = 7.5f;
                invisibilityAvailable = true;
                invisibilityMeterFillingBackUp = false;
            }
        }
     
    }

   

    public bool ReturnIsGrounded()
    {
        return this.isGrounded;
    }

    public bool ReturnInvisibilityStatus()
    {
        return this.invisibilityAvailable;
    }

    public void SetCameraLock(bool value)
    {
        isCameraLocked = value;
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

    public float ReturnGravity()
    {
        return this.gravity;
    }

    public float ReturnStartingInvisibility()
    {
        return this.startingInvisible;
    }

    public float ReturnMaxInvisibility()
    {
        return this.maxInvisible;
    }

    public Image ReturnInvisibilityMeter()
    {
        return this.invisibleMeter;
    }

    public bool ReturnUsingInvisibility()
    {
        return this.usingInvisibility;
    }
}
