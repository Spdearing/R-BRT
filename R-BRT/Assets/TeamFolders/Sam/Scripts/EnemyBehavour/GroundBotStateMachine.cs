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
    //[SerializeField] private Transform[] patrolPoints;


    [Header("Int")]
    //[SerializeField] private int currentWaypointIndex;

    [Header("Scripts")]
    [SerializeField] GameOverScreen gameOverScreen;


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

                break;

            case BehaviorState.playerCaught:

                gameOverScreen.ReturnGameOverPanel().SetActive(true);
                Time.timeScale = 0.0f;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;

                break;

            default:

                currentState = BehaviorState.patrolling;

                break;
        }
    }
}
