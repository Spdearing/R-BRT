using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GroundBotStateMachine;

public class PlayerDetectionState : MonoBehaviour
{
    [Header("Scripts")]
    [SerializeField] GameManager gameManager;
    [SerializeField] DetectionMeter detection;
    [SerializeField] GroundBotStateMachine groundBotStateMachine; // instantiated script
    [SerializeField] FlyingBotStateMachine flyingBotStateMachine; // instantiated script
    [SerializeField] SpiderBotStateMachine spiderBotStateMachine; // instantiated script
    [SerializeField] GroundBotSpawner groundBotSpawner;
    [SerializeField] FlyingBotSpawner flyingBotSpawner;
    [SerializeField] SpiderBotSpawner spiderBotSpawner;
    [SerializeField] EnemyGroundBotFieldOfView enemyGroundBotFieldOfView;
    [SerializeField] EnemyFlyingBotFieldOfView enemyFlyingBotFieldOfView;
    [SerializeField] EnemySpiderBotFieldOfView enemySpiderBotFieldOfView;

    [Header("Floats")]
    [SerializeField] private float detectionIncreaseRate;
    [SerializeField] private float detectionDecreaseRate;

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
        groundBotSpawner = GameObject.FindWithTag("EnemySpawner").GetComponent<GroundBotSpawner>();
        flyingBotSpawner = GameObject.FindWithTag("EnemySpawner").GetComponent<FlyingBotSpawner>();
        spiderBotSpawner = GameObject.FindWithTag("EnemySpawner").GetComponent<SpiderBotSpawner>();
        currentState = DetectionState.exploring;
        detectionIncreaseRate = 5.0f;
        detectionDecreaseRate = 25.0f;
        
        
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
            case DetectionState.beingDetected:

                detection.IncreaseDetection(detectionIncreaseRate);
                detectionIncreaseRate += .5f;
                

                if (detection.ReturnStartingDetection() >= detection.GetDetectionMax())// checks to see if bar is full
                {
                    Debug.Log("Max Detection");
                    ChangeDetectionState(DetectionState.detected);// sets the capped amount, and then changes the state
                }

                break;

            case DetectionState.meterRepleneshing:

                detection.DecreaseDetection(detectionDecreaseRate);
                detectionIncreaseRate = 5.0f;

                if(detection.ReturnStartingDetection() <= 0)
                {
                    ChangeDetectionState(DetectionState.exploring);
                }

                break;


            case DetectionState.detected:

                groundBotStateMachine.ChangeBehavior(BehaviorState.playerCaught);

                break;

            default:

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
}
