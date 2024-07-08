using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static FlyingBotStateMachine;
using UnityEngine.InputSystem.LowLevel;
using Unity.VisualScripting;

public class EnemyFlyingBotFieldOfView : MonoBehaviour
{
    [Header("Scripts")]
    [SerializeField] private PlayerDetectionState playerDetectionState;
    [SerializeField] private EnemyProximityCheck enemyProximity;
    [SerializeField] private FlyingBotStateMachine flyingBotStateMachine;
    [SerializeField] private FlyingBotSpawner flyingBotSpawner;
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
        flyingBotSpawner = GameManager.instance.ReturnFlyingBotSpawner(); 
        playerIsBeingDetected = false;
        enemyGrandparentTransform = gameObject.transform.parent.parent;
        flyingBotStateMachine = enemyGrandparentTransform.GetComponent<FlyingBotStateMachine>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && enemyProximity.ReturnEnemyWithinRange())
        {
            if (!ability.ReturnUsingInvisibility())
            {
                Debug.Log("entering " + transform.parent.parent.gameObject.name);
                sceneActivity.SetPlayerIsSpotted(true);
                playerIsBeingDetected = true;


                Transform grandparentTransform = gameObject.transform.parent.parent;

                FlyingBotStateMachine flyingBotStateMachine = grandparentTransform.GetComponent<FlyingBotStateMachine>();

                EnemyFlyingBotFieldOfView enemyFieldOfView = gameObject.GetComponent<EnemyFlyingBotFieldOfView>();


                if (flyingBotStateMachine != null && enemyFieldOfView != null)
                {
                    playerDetectionState.SetFlyingBotStateMachine(flyingBotStateMachine);
                    playerDetectionState.SetFlyingBotFieldOfView(enemyFieldOfView);
                }

                playerDetectionState.SetDetectedByFlyingBot(true);
                flyingBotStateMachine.ChangeBehavior(FlyingBotStateMachine.FlyingState.scanning);
            }
            sceneActivity.SetPlayerIsSpotted(false);
            playerIsBeingDetected = false;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && enemyProximity.ReturnEnemyWithinRange() && !ability.ReturnUsingInvisibility())
        {
            Debug.Log("inside " + transform.parent.parent.gameObject.name);
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
            sceneActivity.SetPlayerIsSpotted(false);
            playerIsBeingDetected = false;
            playerDetectionState.ChangeDetectionState(PlayerDetectionState.DetectionState.meterRepleneshing);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && enemyProximity.ReturnEnemyWithinRange())
        {
            Debug.Log("exiting " + transform.parent.parent.gameObject.name);
            sceneActivity.SetPlayerIsSpotted(false);
            playerIsBeingDetected = false;
            playerDetectionState.ChangeDetectionState(PlayerDetectionState.DetectionState.meterRepleneshing);
            playerDetectionState.SetDetectedByFlyingBot(false);
            flyingBotStateMachine.ChangeBehavior(FlyingBotStateMachine.FlyingState.patrolling);
            flyingBotStateMachine.SetPatrollingStatus(true);
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
    }

    public Transform ReturnThisEnemy()
    {
        return this.enemyGrandparentTransform;
    }
}
