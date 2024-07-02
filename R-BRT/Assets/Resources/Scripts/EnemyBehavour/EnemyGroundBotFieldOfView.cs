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
    [SerializeField] private PlayerController player;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private EnemyProximityCheck enemyProximity;
    [SerializeField] private GroundBotHeadMovement groundBotHeadMovement;
    [SerializeField] private SceneActivity sceneActivity;


    [Header("Bools")]
    [SerializeField] private bool playerIsBeingDetected;

    [Header("Transform")]
    [SerializeField] private Transform enemyGrandparentTransform;



    private void Start()
    {
        sceneActivity = GameObject.FindWithTag("Canvas").GetComponent<SceneActivity>();
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
            sceneActivity.SetPlayerIsSpotted(true);
            playerIsBeingDetected = true;

            
            Transform grandparentTransform = gameObject.transform.parent.parent;

            GroundBotStateMachine groundBotStateMachine = grandparentTransform.GetComponent<GroundBotStateMachine>();
            
            EnemyGroundBotFieldOfView enemyFieldOfView = gameObject.GetComponent<EnemyGroundBotFieldOfView>();

            if (groundBotStateMachine != null && enemyFieldOfView != null)
            {
                playerDetectionState.SetGroundBotStateMachine(groundBotStateMachine);
                playerDetectionState.SetGroundBotFieldOfView(enemyFieldOfView);
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
        if (other.CompareTag("Player") && enemyProximity.ReturnEnemyWithinRange() && !player.ReturnUsingInvisibility())
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
        if (other.CompareTag("Player") && enemyProximity.ReturnEnemyWithinRange() && !player.ReturnUsingInvisibility())
        {
            sceneActivity.SetPlayerIsSpotted(false);
            playerIsBeingDetected = false;

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
