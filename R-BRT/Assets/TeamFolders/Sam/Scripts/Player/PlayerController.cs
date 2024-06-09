using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    public float walkSpeed;
    public float sprintSpeed;
    public float gravity = -1f;
    public bool isSprinting;

    public float groundDrag;

    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    bool readyToJump;
    public bool isJumping;

    [Header("Crouching")]
    public float crouchSpeed;
    public bool isCrouching;
    public float crouchYScale;
    private float startYScale;

    [Header("Slope Handling")]
    public float maxSlopeAngle;
    private RaycastHit slopeHit;
    private bool exitingSlope;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode sprintKey = KeyCode.LeftShift;
    public KeyCode crouchKey = KeyCode.LeftControl;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask groundMask;
    public bool isGrounded;

    public bool isCameraLocked;

    public Vector3 velocity;

    public Transform orientation;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    public Rigidbody rb;

    public MovementState currentState;

    [SerializeField] private TrailRenderer tr;

    [SerializeField] private bool invisibilityUnlocked;
    [SerializeField] private bool invisibilityAvailable;

    [SerializeField] private bool jetPackUnlocked;
    


    //public AudioSource footstepsSound, sprintSound;

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
        isCameraLocked = false;

        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        readyToJump = true;

        startYScale = transform.localScale.y;

        invisibilityUnlocked = false;
        jetPackUnlocked = false;
    }

    private void Update()
    {
        // ground check
        isGrounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * .5f , groundMask);

        MyInput();
        SpeedControl();
        Sprint();
        UpdateState();
        BecomeInvisible();

        if (!isGrounded)
        {
            InAir();
        }

        
        

        //FootSteps();
        // handle drag
        if (isGrounded)
        {
            rb.drag = groundDrag;
        }  
        else if (OnSlope() && !exitingSlope)
        {
            rb.drag = groundDrag;
        }

        else
            rb.drag = 0;
            
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        // when to jump
        if (Input.GetKey(jumpKey) && readyToJump && isGrounded)
        {
            readyToJump = false;

            Jump();
            isJumping = true;

            Invoke(nameof(ResetJump), jumpCooldown);
        }

        // Start Crouch
        if (Input.GetKeyDown(crouchKey))
        {
            transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);
            rb.AddForce(Vector3.down * 10.0f, ForceMode.Impulse);
            Crouching();
        }

        if (Input.GetKeyUp(crouchKey))
        {
            transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);
            Walking();
        }
    }

    private void MovePlayer()
    {
        // calculate movement direction
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        if (OnSlope() && !exitingSlope)
        {
            rb.AddForce(GetSlopeMoveDirection() * moveSpeed * 20.0f, ForceMode.Force);

            if (rb.velocity.y > 0)
            {
                rb.AddForce(Vector3.down * 80.0f, ForceMode.Force);
            }
        }

        // on ground
        if (isGrounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);

        // in air
        else if (!isGrounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);

        rb.useGravity = !OnSlope();
    }

    private void SpeedControl()
    {
        if (OnSlope() && !exitingSlope)
        {
            if (rb.velocity.magnitude > moveSpeed)
            {
                rb.velocity = rb.velocity.normalized * moveSpeed;
            }
        }

        //limiting speed on ground or in air
        else
        {
            Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

            // limit velocity if needed
            if (flatVel.magnitude > moveSpeed)
            {
                Vector3 limitedVel = flatVel.normalized * moveSpeed;
                rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
            }
        }
    }

    public void BecomeInvisible()
    {
        if(Input.GetKeyDown(KeyCode.E) && invisibilityAvailable && invisibilityUnlocked)
        {
            this.gameObject.tag = "Invisible";
            StartCoroutine(InvisibilityTimer());
        }
    }

    IEnumerator InvisibilityTimer()
    {
        invisibilityAvailable = false;
        yield return new WaitForSeconds(6.0f);
        this.gameObject.tag = "Player";
        invisibilityAvailable = true;

    }

    private void Jump()
    {
        exitingSlope = true;

        // reset y velocity
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    void InAir()
    {
        if (!isGrounded && isJumping)
        {
            ChangeState(MovementState.air);
        }    
    }

    void Sprint()
    {
        if(Input.GetKeyDown(sprintKey)) 
        {
            Sprinting();
        }
        else if(Input.GetKeyUp(sprintKey))
        {
            Walking();
        }
    }

    void Sprinting()
    {
        ChangeState(MovementState.sprinting);
    }

    void Walking()
    {
        ChangeState(MovementState.walking);
    }


    private void ResetJump()
    {
        readyToJump = true;
        isJumping = false;

        exitingSlope = false;
    }

    public void ChangeState(MovementState newState)
    {
        currentState = newState;
    }

    void Crouching()
    {
        ChangeState(MovementState.crouching);
    }

    

    void UpdateState()
    {
       switch(currentState) 
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

                if(isGrounded)
                {
                    currentState = MovementState.walking;
                }

                break;


        }
    }

    private bool OnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight * 1.0f))
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

    //private void FootSteps() 
    //{
    //    if (horizontalInput !=0 || verticalInput !=0 && isGrounded)
    //    {
    //        if (isGgrounded && Input.GetKey(sprintKey))
    //        {
    //            footstepsSound.enabled = false;
    //            sprintSound.enabled = true;
    //        }
    //        else
    //        {
    //            footstepsSound.enabled = true;
    //            sprintSound.enabled = false;
    //        }
    //    }
    //    else
    //    {
    //        footstepsSound.enabled = false;
    //        sprintSound.enabled = false;
    //    }
    //}
}
    
