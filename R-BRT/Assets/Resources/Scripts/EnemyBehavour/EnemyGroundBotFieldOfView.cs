using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static FlyingBotStateMachine;
using UnityEngine.InputSystem.LowLevel;
using Unity.VisualScripting;

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
        Debug.Log("GroundBotFOV");
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
        if (other.CompareTag("Player") && enemyProximity.ReturnEnemyWithinRange() && !ability.ReturnUsingInvisibility())
        {
            sceneActivity.SetPlayerIsSpotted(true);
            playerIsBeingDetected = true;



            Transform grandparentTransform = gameObject.transform.parent.parent;

            GroundBotStateMachine groundBotStateMachine = grandparentTransform.GetComponent<GroundBotStateMachine>();
            
            EnemyGroundBotFieldOfView enemyFieldOfView = gameObject.GetComponent<EnemyGroundBotFieldOfView>();

            if (groundBotStateMachine != null && enemyFieldOfView != null)
            {
                playerDetectionState.SetGroundBotStateMachine(groundBotStateMachine);
                playerDetectionState.SetGroundBotFieldOfView(enemyFieldOfView);
                groundBotStateMachine.ChangeBehavior(GroundBotStateMachine.BehaviorState.scanning);
            }

            if (groundBotHeadMovement != null)
            {
                groundBotHeadMovement.SetPlayerSpotted(true); // Assuming this method exists
                playerDetectionState.SetDetectedByGroundBot(true);
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && enemyProximity.ReturnEnemyWithinRange() && !ability.ReturnUsingInvisibility())
        {
            bool withinRange = enemyProximity.ReturnEnemyWithinRange();
            sceneActivity.SetPlayerIsSpotted(withinRange);

            if (!withinRange)
            {
                playerIsBeingDetected = false;
                playerDetectionState.ChangeDetectionState(PlayerDetectionState.DetectionState.meterRepleneshing); 
            }
            else
            {
                playerIsBeingDetected = true;
            }
        }
        else
        {
            groundBotHeadMovement.SetPlayerSpotted(false);
            sceneActivity.SetPlayerIsSpotted(false);
            playerIsBeingDetected = false;
            playerDetectionState.ChangeDetectionState(PlayerDetectionState.DetectionState.meterRepleneshing); 
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && enemyProximity.ReturnEnemyWithinRange() && !ability.ReturnUsingInvisibility())
        {
            sceneActivity.SetPlayerIsSpotted(false);
            playerIsBeingDetected = false;

            groundBotStateMachine.ChangeBehavior(GroundBotStateMachine.BehaviorState.patrolling);
            playerDetectionState.ChangeDetectionState(PlayerDetectionState.DetectionState.meterRepleneshing);
            
            if (gameObject.transform.parent.parent.tag == "GroundBot")
            {
                groundBotHeadMovement.SetPlayerSpotted(false);
                playerDetectionState.SetDetectedByGroundBot(false);
            }
        }
    }

    private void DetectingPlayer()
    {
        if (enemyProximity.ReturnEnemyWithinRange() && sceneActivity.ReturnPlayerSpotted())
        {
            if (playerIsBeingDetected)
            {
                playerDetectionState.ChangeDetectionState(PlayerDetectionState.DetectionState.beingDetected);
                playerIsBeingDetected = false;
            }
        }
        else
        
            playerIsBeingDetected = false;
        
    }

    public Transform ReturnThisEnemy()
    {
        return this.enemyGrandparentTransform;
    }
}
