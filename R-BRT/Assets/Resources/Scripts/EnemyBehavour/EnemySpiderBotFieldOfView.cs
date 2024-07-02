using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static FlyingBotStateMachine;
using UnityEngine.InputSystem.LowLevel;
using Unity.VisualScripting;

public class EnemySpiderBotFieldOfView : MonoBehaviour
{
    [Header("Scripts")]
    [SerializeField] private PlayerDetectionState playerDetectionState;
    [SerializeField] private PlayerController player;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private EnemyProximityCheck enemyProximity;
    [SerializeField] private SpiderBotStateMachine spiderBotStateMachine;
    [SerializeField] private SpiderBotSpawner spiderBotSpawner;
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
        spiderBotSpawner = GameObject.FindWithTag("EnemySpawner").GetComponent<SpiderBotSpawner>();
        spiderBotStateMachine = spiderBotSpawner.ReturnSpiderBotStateInstance();
        playerIsBeingDetected = false;
        enemyGrandparentTransform = gameObject.transform.parent;
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

            
            Transform grandparentTransform = gameObject.transform.parent;

            SpiderBotStateMachine spiderBotStateMachine = grandparentTransform.GetComponent<SpiderBotStateMachine>();
            
            EnemySpiderBotFieldOfView enemyFieldOfView = gameObject.GetComponent<EnemySpiderBotFieldOfView>();

            

            if (spiderBotStateMachine != null && enemyFieldOfView != null)
            {
                playerDetectionState.SetSpiderBotStateMachine(spiderBotStateMachine);
                playerDetectionState.SetSpiderBotFieldOfView(enemyFieldOfView);
            }

            playerDetectionState.SetDetectedBySpiderBot(true);
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
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && enemyProximity.ReturnEnemyWithinRange())
        {
            sceneActivity.SetPlayerIsSpotted(false);
            playerIsBeingDetected = false;

            playerDetectionState.ChangeDetectionState(PlayerDetectionState.DetectionState.meterRepleneshing);
            playerDetectionState.SetDetectedBySpiderBot(false);
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
