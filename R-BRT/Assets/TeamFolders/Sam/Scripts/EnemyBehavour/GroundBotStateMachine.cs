using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class GroundBotStateMachine : MonoBehaviour
{
    [Header("Nav Mesh")]
    //[SerializeField] private NavMeshAgent navRobot;

    [Header("GameObjects")]
    [SerializeField] GameObject player;

    [Header("Transform")]
    [SerializeField] private Transform playerCamera;
    //[SerializeField] private Transform[] patrolPoints;


    [Header("Int")]
    //[SerializeField] private int currentWaypointIndex;

    [Header("Scripts")]
    [SerializeField] GameOverScreen gameOverScreen;
    [SerializeField] PlayerDetectionState playerDetectionState;
    [SerializeField] DetectionMeter detectionMeter;
    [SerializeField] PlayerController playerController;
    [SerializeField] EnemyFieldOfView enemyFieldOfView;
    
    public BehaviorState currentState;

    public enum BehaviorState
    {
        patrolling,
        scanning,
        playerCaught,
    }

    // Start is called before the first frame update
    void Start()
    {
        //navRobot = GetComponent<NavMeshAgent>();
        currentState = BehaviorState.patrolling;
        playerDetectionState = GameObject.Find("Player").GetComponent<PlayerDetectionState>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateBehavior();
    }

    private void FixedUpdate()
    {
        UpdateBehavior();
    }

    public void ChangeBehavior(BehaviorState newState)
    {
        currentState = newState;
    }

    void UpdateBehavior()
    {
        switch (currentState)
        {
            case BehaviorState.patrolling:

                //if (navRobot.remainingDistance < 0.1f)
                //{
                //    currentWaypointIndex = UnityEngine.Random.Range(0,patrolPoints.Length); 

                //    navRobot.SetDestination(patrolPoints[currentWaypointIndex].position);
                //}

                break;

            case BehaviorState.scanning:

                playerDetectionState.ChangeDetectionState(PlayerDetectionState.DetectionState.beingDetected);

                break;

            case BehaviorState.playerCaught:

               
                gameOverScreen.ReturnGameOverPanel().SetActive(true);

                
                playerController.SetCameraLock(true);
                playerCamera.LookAt(transform.position);

                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;

                break;

            default:

                currentState = BehaviorState.patrolling;

                break;
        }
    }
}
