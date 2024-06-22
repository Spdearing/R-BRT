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
    [SerializeField] private PlayerController player;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private EnemyProximityCheck enemyProximity;
    [SerializeField] private FlyingBotStateMachine flyingBotStateMachine;

    [Header("Bools")]
    [SerializeField] private bool playerIsBeingDetected;

    [Header("Transform")]
    [SerializeField] private Transform enemyGrandparentTransform;


    private void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerController>();
        enemyProximity = GameObject.Find("Player").GetComponent<EnemyProximityCheck>();
        playerDetectionState = GameObject.Find("Player").GetComponent<PlayerDetectionState>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        playerIsBeingDetected = false;
        enemyGrandparentTransform = gameObject.transform.parent.parent;
    }

    private void Update()
    {
        DetectingPlayer();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && enemyProximity.ReturnEnemyWithinRange() && !player.ReturnUsingInvisibility())
        {
            gameManager.SetPlayerIsSpotted(true);
            playerIsBeingDetected = true;


            Transform grandparentTransform = gameObject.transform.parent.parent;

            FlyingBotStateMachine flyingBotStateMachine = grandparentTransform.GetComponent<FlyingBotStateMachine>();

            EnemyFlyingBotFieldOfView enemyFieldOfView = gameObject.GetComponent<EnemyFlyingBotFieldOfView>();


            if (flyingBotStateMachine != null && enemyFieldOfView != null)
            {
                playerDetectionState.SetFlyingBotStateMachine(flyingBotStateMachine);
                playerDetectionState.SetFlyingBotFieldOfView(enemyFieldOfView);
            }

            flyingBotStateMachine.ChangeBehavior(FlyingBotStateMachine.FlyingState.scanning);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && enemyProximity.ReturnEnemyWithinRange() && !player.ReturnUsingInvisibility())
        {
            bool withinRange = enemyProximity.ReturnEnemyWithinRange();
            gameManager.SetPlayerIsSpotted(withinRange);

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
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && enemyProximity.ReturnEnemyWithinRange())
        {
            gameManager.SetPlayerIsSpotted(false);
            playerIsBeingDetected = false;

            playerDetectionState.ChangeDetectionState(PlayerDetectionState.DetectionState.meterRepleneshing);
            
 
            if (gameObject.transform.parent.tag == "FlyingBot")
            {
                flyingBotStateMachine.ChangeBehavior(FlyingBotStateMachine.FlyingState.patrolling);
            }
        }
    }

    private void DetectingPlayer()
    {
        if (enemyProximity.ReturnEnemyWithinRange() && gameManager.ReturnPlayerSpotted())
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
