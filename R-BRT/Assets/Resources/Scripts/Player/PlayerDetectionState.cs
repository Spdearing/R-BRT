using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
using static FlyingBotStateMachine;
using static GroundBotStateMachine;
using static SpiderBotStateMachine;

public class PlayerDetectionState : MonoBehaviour
{
    [Header("Scripts")]
    [SerializeField] private GameManager gameManager;
    [SerializeField] private DetectionMeter detection;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private Jetpack jetpack;

    [SerializeField] private GroundBotStateMachine groundBotStateMachine; // instantiated script
    [SerializeField] private EnemyGroundBotFieldOfView enemyGroundBotFieldOfView;
    //[SerializeField] GroundBotSpawner groundBotSpawner;

    [SerializeField] private FlyingBotStateMachine flyingBotStateMachine; // instantiated script
    [SerializeField] private EnemyFlyingBotFieldOfView enemyFlyingBotFieldOfView;
    //[SerializeField] FlyingBotSpawner flyingBotSpawner;

    [SerializeField] private SpiderBotStateMachine spiderBotStateMachine; // instantiated script
    [SerializeField] private EnemySpiderBotFieldOfView enemySpiderBotFieldOfView;
    //[SerializeField] SpiderBotSpawner spiderBotSpawner;

    [Header("Floats")]
    [SerializeField] private float crouchingDetectionIncrease;
    [SerializeField] private float detectionIncreaseRate;
    [SerializeField] private float detectionDecreaseRate;

    [Header("Bools")]
    [SerializeField] private bool detectedByGroundBot;
    [SerializeField] private bool detectedByFlyingBot;
    [SerializeField] private bool detectedBySpiderBot;

    [Header("Game Object")]
    [SerializeField] private GameObject DetectionMeter;

    public DetectionState currentState;

    public enum DetectionState
    {
        beingDetected,
        meterRepleneshing,
        exploring,
        detected
    }


    // Start is called before the first frame update
    void Start()
    {
        playerController = gameObject.GetComponent<PlayerController>();
        gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
        detection = GameObject.FindWithTag("DetectionMeter").GetComponent<DetectionMeter>();
        //groundBotSpawner = GameObject.FindWithTag("EnemySpawner").GetComponent<GroundBotSpawner>();
        //flyingBotSpawner = GameObject.FindWithTag("EnemySpawner").GetComponent<FlyingBotSpawner>();
        //spiderBotSpawner = GameObject.FindWithTag("EnemySpawner").GetComponent<SpiderBotSpawner>();
        detectedByGroundBot = false;
        detectedByFlyingBot = false;
        detectedBySpiderBot = false;
        crouchingDetectionIncrease = 1.0f;
        detectionIncreaseRate = 7.5f;
        detectionDecreaseRate = 25.0f;
        currentState = DetectionState.exploring;


    }

    // Update is called once per frame
    void Update()
    {
        UpdateDetectionState();
    }

    public void ChangeDetectionState(DetectionState newState)
    {
        currentState = newState;
    }

    void UpdateDetectionState()
    {
        switch (currentState)
        {
            case DetectionState.exploring:
                // Handle exploring state logic here if needed
                break;

            case DetectionState.beingDetected:

                if (playerController.ReturnCrouchingStatus(true))
                {
                    
                    detection.IncreaseDetection(crouchingDetectionIncrease);
                    detectionIncreaseRate += 0.5f;
                }
                else if (jetpack.IsUsingJetpack(true))
                {
                    
                    detection.IncreaseDetection(detectionIncreaseRate * 2);
                    detectionIncreaseRate += 0.5f;
                }
                else
                {
                    
                    detection.IncreaseDetection(detectionIncreaseRate);
                    detectionIncreaseRate += 0.5f;
                }

                // Check if detection has reached maximum
                if (detection.ReturnStartingDetection() >= detection.GetDetectionMax())
                {
                    Debug.Log("Max Detection");

                    ChangeDetectionState(DetectionState.detected);
                }
                break;

            case DetectionState.meterRepleneshing:
                // Decrease detection level over time
                detection.DecreaseDetection(detectionDecreaseRate);
                detectionIncreaseRate = 5.0f;

                // Transition to exploring state when detection is replenished
                if (detection.ReturnStartingDetection() <= 0)
                {
                    
                    ChangeDetectionState(DetectionState.exploring);
                }
                break;

            case DetectionState.detected:
                // Trigger behavior changes in bots when player is detected
                if (detectedByGroundBot)
                {
                    groundBotStateMachine.ChangeBehavior(BehaviorState.playerCaught);
                }

                if (detectedByFlyingBot)
                {
                    flyingBotStateMachine.ChangeBehavior(FlyingState.playerCaught);
                }

                if (detectedBySpiderBot)
                {
                    spiderBotStateMachine.ChangeBehavior(SpiderState.playerCaught);
                }
                break;

            default:
                // Handle any unexpected state transitions gracefully
                currentState = DetectionState.exploring;
                detectionIncreaseRate = 5.0f;
                break;
        }
    }

    public void SetGroundBotStateMachine(GroundBotStateMachine value)
    {
        this.groundBotStateMachine = value;
    }

    public void SetGroundBotFieldOfView(EnemyGroundBotFieldOfView value)
    {
        this.enemyGroundBotFieldOfView = value;
    }

    public void SetFlyingBotStateMachine(FlyingBotStateMachine value)
    {
        this.flyingBotStateMachine = value;
    }

    public void SetFlyingBotFieldOfView(EnemyFlyingBotFieldOfView value)
    {
        this.enemyFlyingBotFieldOfView = value;
    }

    public void SetSpiderBotStateMachine(SpiderBotStateMachine value)
    {
        this.spiderBotStateMachine = value;
    }

    public void SetSpiderBotFieldOfView(EnemySpiderBotFieldOfView value)
    {
        this.enemySpiderBotFieldOfView = value;
    }

    public void SetDetectedByGroundBot(bool value)
    {
        detectedByGroundBot = value;
    }

    public void SetDetectedByFlyingBot(bool value)
    {
        detectedByFlyingBot = value;
    }

    public void SetDetectedBySpiderBot(bool value)
    {
        detectedBySpiderBot = true;
    }
}
