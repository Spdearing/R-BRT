using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class GroundBotStateMachine : MonoBehaviour
{
    [Header("Nav Mesh")]
    //[SerializeField] private NavMeshAgent navRobot;

    [Header("GameObjects")]
    [SerializeField] GameObject player;
    [SerializeField] GameObject playerCamera;

    [Header("Transform")]
    [SerializeField] private Transform playerCameraTransform;
    //[SerializeField] private Transform[] patrolPoints;


    [Header("Int")]
    //[SerializeField] private int currentWaypointIndex;

    [Header("Scripts")]
    [SerializeField] GameOverScreen gameOverScreen;
    [SerializeField] PlayerDetectionState playerDetectionState;
    [SerializeField] DetectionMeter detectionMeter;
    [SerializeField] PlayerController playerController;
    [SerializeField] EnemyFieldOfView enemyFieldOfView;
    [SerializeField] GroundBotSpawner groundBotSpawner;
    
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
        
        player = GameObject.FindWithTag("Player");
        playerCamera = GameObject.FindGameObjectWithTag("MainCamera");
        playerCameraTransform = playerCamera.transform;
        groundBotSpawner = GameObject.FindWithTag("GroundBotSpawner").GetComponent<GroundBotSpawner>();
        gameOverScreen = GameObject.Find("Canvas").GetComponentInChildren<GameOverScreen>();
        detectionMeter = GameObject.Find("EnemyDetectionManager").GetComponentInChildren<DetectionMeter>();
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        enemyFieldOfView = GetComponentInChildren<EnemyFieldOfView>(); 
        playerDetectionState = GameObject.Find("Player").GetComponent<PlayerDetectionState>();
        currentState = BehaviorState.patrolling;
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

                Debug.Log("inside player Caught");
                gameOverScreen.ReturnGameOverPanel().SetActive(true);

                playerController.SetCameraLock(true);
                playerCameraTransform.LookAt(enemyFieldOfView.ReturnThisEnemy());

                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;

                break;

            default:

                currentState = BehaviorState.patrolling;

                break;
        }
    }
}
