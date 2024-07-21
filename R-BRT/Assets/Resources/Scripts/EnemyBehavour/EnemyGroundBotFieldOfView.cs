using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyGroundBotFieldOfView : MonoBehaviour
{
    [Header("Scripts")]
    [SerializeField] private PlayerDetectionState playerDetectionState;
    [SerializeField] private EnemyProximityCheck enemyProximity;
    [SerializeField] private GroundBotHeadMovement groundBotHeadMovement;
    [SerializeField] private GroundBotStateMachine groundBotStateMachine;
    [SerializeField] private GroundBotSpawner groundBotSpawner;
    [SerializeField] private SceneActivity sceneActivity;
    [SerializeField] private PlayerAbilities ability;

    [Header("Bools")]
    [SerializeField] private bool playerIsBeingDetected;

    [Header("Transform")]
    [SerializeField] private Transform enemyGrandparentTransform;

    private void Start()
    {
        Setup();
    }

    private void Update()
    {
        DetectingPlayer();
    }

    void Setup()
    {
        sceneActivity = GameManager.instance.ReturnSceneActivity();
        ability = GameManager.instance.ReturnPlayerAbilities();
        enemyProximity = GameManager.instance.ReturnEnemyProximityCheck();
        playerDetectionState = GameManager.instance.ReturnPlayerDetectionState();
        groundBotSpawner = GameManager.instance.ReturnGroundBotSpawner();
        groundBotHeadMovement = transform.parent.parent.GetComponent<GroundBotHeadMovement>();
        groundBotStateMachine = transform.parent.parent.GetComponent<GroundBotStateMachine>();
        playerIsBeingDetected = false;
        enemyGrandparentTransform = gameObject.transform.parent.parent;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (IsPlayer(other) && groundBotStateMachine.ReturnDetectingPlayer() == true && !ability.ReturnUsingInvisibility())
        {
            sceneActivity.SetPlayerIsSpotted(true);
            playerIsBeingDetected = true;
            SetGroundBotStateToScanning();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (IsPlayer(other))
        {
            bool withinRange = groundBotStateMachine.ReturnDetectingPlayer();
            sceneActivity.SetPlayerIsSpotted(withinRange);

            if (!withinRange)
            {
                ResetDetection();
            }
        }
        else
        {
            ResetDetection();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (IsPlayer(other))
        {
            sceneActivity.SetPlayerIsSpotted(false);
            playerIsBeingDetected = false;

            groundBotStateMachine.ChangeBehavior(GroundBotStateMachine.BehaviorState.patrolling);
            playerDetectionState.ChangeDetectionState(PlayerDetectionState.DetectionState.meterRepleneshing);
            groundBotHeadMovement.SetPlayerSpotted(false);
            playerDetectionState.SetDetectedByGroundBot(false);
        }
    }

    private void DetectingPlayer()
    {
        if (groundBotStateMachine.ReturnDetectingPlayer() && sceneActivity.ReturnPlayerSpotted())
        {
            if (playerIsBeingDetected)
            {
                playerDetectionState.ChangeDetectionState(PlayerDetectionState.DetectionState.beingDetected);
                playerIsBeingDetected = false;
            }
        }
        else
        {
            playerIsBeingDetected = false;
        }
    }

    private bool IsPlayer(Collider other)
    {
        return other.CompareTag("Player") && !ability.ReturnUsingInvisibility();
    }

    private bool IsDetectable()
    {
        return enemyProximity.ReturnEnemyWithinRange();
    }

    private void SetGroundBotStateToScanning()
    {
        Transform grandparentTransform = gameObject.transform.parent.parent;
        GroundBotStateMachine stateMachine = grandparentTransform.GetComponent<GroundBotStateMachine>();
        EnemyGroundBotFieldOfView enemyFieldOfView = GetComponent<EnemyGroundBotFieldOfView>();

        if (stateMachine != null && enemyFieldOfView != null)
        {
            playerDetectionState.SetGroundBotStateMachine(stateMachine);
            playerDetectionState.SetGroundBotFieldOfView(enemyFieldOfView);
            stateMachine.ChangeBehavior(GroundBotStateMachine.BehaviorState.scanning);
        }

        if (groundBotHeadMovement != null)
        {
            groundBotHeadMovement.SetPlayerSpotted(true);
            playerDetectionState.SetDetectedByGroundBot(true);
        }
    }

    private void ResetDetection()
    {
        groundBotHeadMovement.SetPlayerSpotted(false);
        sceneActivity.SetPlayerIsSpotted(false);
        playerIsBeingDetected = false;
        playerDetectionState.ChangeDetectionState(PlayerDetectionState.DetectionState.meterRepleneshing);
    }

    public Transform ReturnThisEnemy()
    {
        return this.enemyGrandparentTransform;
    }
}

