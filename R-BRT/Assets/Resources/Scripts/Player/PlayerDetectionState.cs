using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GroundBotStateMachine;

public class PlayerDetectionState : MonoBehaviour
{
    [Header("Scripts")]
    [SerializeField] DetectionMeter detection;
    [SerializeField] GroundBotStateMachine groundBotReference; // orginal instance of prefab script
    [SerializeField] GroundBotStateMachine groundBotStateMachine; // instantiated script

    [Header("Floats")]
    [SerializeField] private float detectionIncreaseRate;
    [SerializeField] private float detectionDecreaseRate;

    [Header("GameObject")]
    [SerializeField] GameObject groundBot;


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
        currentState = DetectionState.exploring;
        detectionIncreaseRate = 5.0f;
        detectionDecreaseRate = 25.0f;
        groundBot = Resources.Load<GameObject>("Sam's_Prefabs/groundBotDone");
        groundBotReference = groundBot.GetComponent<GroundBotStateMachine>();
        groundBotStateMachine = groundBotReference.ReturnThisScript();
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

                Debug.Log("Detected");
                Debug.Log(groundBotStateMachine);
                groundBotStateMachine.ChangeBehavior(BehaviorState.playerCaught);

                break;

            default:

                currentState = DetectionState.exploring;
                detectionIncreaseRate = 5.0f;

                break;
        }
    }
}
