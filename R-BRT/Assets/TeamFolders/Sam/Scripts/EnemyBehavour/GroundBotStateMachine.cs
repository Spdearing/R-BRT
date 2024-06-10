using System.Collections;
using UnityEngine;

public class GroundBotStateMachine : MonoBehaviour
{
    [Header("LayerMask")]
    [SerializeField] LayerMask groundMask;

    [Header("GameObjects")]
    [SerializeField] GameObject player;
    [SerializeField] GameObject endingPoint;

    [Header("Vector3")]
    [SerializeField] Vector3 startingLocation;
    [SerializeField] Vector3 endingLocation;

    [Header("Rotations")]
    [SerializeField] private Quaternion startRotation;
    [SerializeField] private Quaternion targetRotation;

    [Header("Transform")]
    [SerializeField] Transform endingLocationObject;

    [Header("Floats")]
    [SerializeField] private float robotHeight;
    [SerializeField] private float duration;
    [SerializeField] private float elapsedTime;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float chaseSpeed; 
    [SerializeField] private float stoppingDistance = 2f;

    [Header("Rigidbody")]
    private Rigidbody rb;

    [Header("Bools")]
    [SerializeField] bool isGrounded;
    [SerializeField] bool goingToTarget;
    [SerializeField] bool isChasing;


    public BehaviourState currentState;

    public enum BehaviourState
    {
        patrolling,
        scanning,
        chasing,
        reset

    }

    // Start is called before the first frame update
    void Start()
    {
        stoppingDistance = 1.0f;
        chaseSpeed = 2.5f;
        rotationSpeed = 2.0f;
        duration = 20.0f;
        robotHeight = 1.329f;
        startingLocation = transform.position;
        endingLocation = endingLocationObject.position;
        elapsedTime = 0f;
        startRotation = transform.rotation;
        targetRotation = Quaternion.LookRotation(endingLocation - startingLocation);
        goingToTarget = true;
        currentState = BehaviourState.patrolling;
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, robotHeight * 2.5f, groundMask);
        
    }

    private void FixedUpdate()
    {
        UpdateBehaviour();
    }

    public void ChangeBehaviour(BehaviourState newState)
    {
        currentState = newState;
    }

    public void ChasePlayer(bool value)
    {
        isChasing = value;
    }

    void UpdateBehaviour()
    {
        switch (currentState)
        {
            case BehaviourState.patrolling:

                if (isGrounded)
                {
                    elapsedTime += Time.deltaTime;

                    float patrollingSpeed = elapsedTime / duration;

                    // Lerp the position
                    if (goingToTarget)
                    {
                        transform.position = Vector3.Lerp(startingLocation, endingLocation, patrollingSpeed);
                    }
                    else
                    {
                        transform.position = Vector3.Lerp(endingLocation, startingLocation, patrollingSpeed);
                    }

                    // If the lerp is complete, reverse direction and reset elapsedTime
                    if (patrollingSpeed >= 1.0f)
                    {
                        goingToTarget = !goingToTarget; // Reverse the direction
                        elapsedTime = 0f; // Reset the elapsed time

                        // Swap start and target rotations for the next movement
                        if (goingToTarget)
                        {
                            startRotation = Quaternion.LookRotation(endingLocation - startingLocation);
                            targetRotation = Quaternion.LookRotation(startingLocation - endingLocation);
                        }
                        else
                        {
                            startRotation = Quaternion.LookRotation(startingLocation - endingLocation);
                            targetRotation = Quaternion.LookRotation(endingLocation - startingLocation);
                        }

                        // Rotate to face the new direction
                        StartCoroutine(RotateToFaceDirection());
                    }
                }
                break;

            case BehaviourState.scanning:
               
                break;

            case BehaviourState.chasing:

                if (isChasing && isGrounded)
                {
                    
                    Vector3 directionToPlayer = (player.transform.position - transform.position).normalized;

                    
                    float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

                    
                    if (distanceToPlayer > stoppingDistance)
                    {
                        
                        transform.Translate(directionToPlayer * chaseSpeed * Time.deltaTime, Space.World);

                       
                        transform.LookAt(player.transform.position);
                    }
                    else
                    {
                        // Stop chasing if within stopping distance
                        isChasing = false;
                    }
                }
        

            break;

            case BehaviourState.reset:

                float returnSpeed = 10.0f;

                transform.position = Vector3.Lerp(transform.position, startingLocation, returnSpeed);

                break;

            default:
                if (isGrounded)
                {
                    currentState = BehaviourState.patrolling;
                }
                break;
        }
    }

    IEnumerator RotateToFaceDirection()
    {
        float rotationTime = 1.0f; // Duration to complete the rotation
        float elapsedRotationTime = 0f;
        Quaternion initialRotation = transform.rotation;

        while (elapsedRotationTime < rotationTime)
        {
            elapsedRotationTime += Time.deltaTime;
            transform.rotation = Quaternion.Lerp(initialRotation, targetRotation, elapsedRotationTime / rotationTime);
            yield return null;
        }
    }
}
