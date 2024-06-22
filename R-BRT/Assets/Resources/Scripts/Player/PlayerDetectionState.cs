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
    [SerializeField] GameManager gameManager;
    [SerializeField] DetectionMeter detection;

    [SerializeField] GroundBotStateMachine groundBotStateMachine; // instantiated script
    [SerializeField] EnemyGroundBotFieldOfView enemyGroundBotFieldOfView;
    //[SerializeField] GroundBotSpawner groundBotSpawner;

    [SerializeField] FlyingBotStateMachine flyingBotStateMachine; // instantiated script
    [SerializeField] EnemyFlyingBotFieldOfView enemyFlyingBotFieldOfView;
    //[SerializeField] FlyingBotSpawner flyingBotSpawner;

    [SerializeField] SpiderBotStateMachine spiderBotStateMachine; // instantiated script
    [SerializeField] EnemySpiderBotFieldOfView enemySpiderBotFieldOfView;
    //[SerializeField] SpiderBotSpawner spiderBotSpawner;

    [Header("Floats")]
    [SerializeField] private float detectionIncreaseRate;
    [SerializeField] private float detectionDecreaseRate;

    [Header("Bools")]
    [SerializeField] private bool detectedByGroundBot;
    [SerializeField] private bool detectedByFlyingBot;
    [SerializeField] private bool detectedBySpiderBot;

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
        //groundBotSpawner = GameObject.FindWithTag("EnemySpawner").GetComponent<GroundBotSpawner>();
        //flyingBotSpawner = GameObject.FindWithTag("EnemySpawner").GetComponent<FlyingBotSpawner>();
        //spiderBotSpawner = GameObject.FindWithTag("EnemySpawner").GetComponent<SpiderBotSpawner>();
        detectedByGroundBot = false;
        detectedByFlyingBot = false;
        detectedBySpiderBot = false;
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

                if(detectedByGroundBot)
                {
                    groundBotStateMachine.ChangeBehavior(BehaviorState.playerCaught);
                }

                if(detectedByFlyingBot)
                {
                    flyingBotStateMachine.ChangeBehavior(FlyingState.playerCaught);
                }

                if (detectedBySpiderBot)
                {
                    spiderBotStateMachine.ChangeBehavior(SpiderState.playerCaught);
                }



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
