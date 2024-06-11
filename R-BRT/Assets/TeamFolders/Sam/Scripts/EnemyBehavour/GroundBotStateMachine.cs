using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.AI;

public class GroundBotStateMachine : MonoBehaviour
{
    [Header("Nav Mesh")]
    [SerializeField] private NavMeshAgent navRobot;

    [Header("LayerMask")]
    [SerializeField] LayerMask groundMask;

    [Header("GameObjects")]
    [SerializeField] GameObject player;
    [SerializeField] GameObject destination;
    

    [Header("Vector3")]
    [SerializeField] Vector3 startingLocation;
    

    [Header("Rotations")]
    [SerializeField] private Quaternion startRotation;
    [SerializeField] private Quaternion targetRotation;

    [Header("Transform")]
    [SerializeField] private Transform[] patrolPoints;

    [Header("Floats")]
    [SerializeField] private float robotHeight;
    [SerializeField] private float duration;
    [SerializeField] private float elapsedTime;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float chaseSpeed; 
    [SerializeField] private float stoppingDistance = 2f;


    [Header("Int")]
    [SerializeField] private int currentWaypointIndex;

    [Header("Rigidbody")]
    private Rigidbody rb;

    [Header("Bools")]
    [SerializeField] bool isGrounded;
    //[SerializeField] bool goingToTarget;
    [SerializeField] bool isChasing;

    [Header("Scripts")]
    [SerializeField] GameOverScreen gameOverScreen;


    public BehaviourState currentState;

    public enum BehaviourState
    {
        patrolling,
        scanning,
        playerCaught,
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
        elapsedTime = 0f;
        startRotation = transform.rotation;
        navRobot = GetComponent<NavMeshAgent>();
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

                if (navRobot.remainingDistance < 0.1f)
                {
                    currentWaypointIndex = UnityEngine.Random.Range(0,patrolPoints.Length); 

                    navRobot.SetDestination(patrolPoints[currentWaypointIndex].position);
                }

                break;

            case BehaviourState.scanning:
               
                break;

            case BehaviourState.playerCaught:

            gameOverScreen.ReturnGameOverPanel().SetActive(true);
            Time.timeScale = 0.0f;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;


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
