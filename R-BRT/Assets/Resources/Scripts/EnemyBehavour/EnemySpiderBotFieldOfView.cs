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
    [SerializeField] private EnemyProximityCheck enemyProximity;
    [SerializeField] private SpiderBotStateMachine spiderBotStateMachine;
    [SerializeField] private SpiderBotSpawner spiderBotSpawner;
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
        spiderBotSpawner = GameManager.instance.ReturnSpiderBotSpawner();
        spiderBotStateMachine = spiderBotSpawner.ReturnSpiderBotStateInstance();
        playerIsBeingDetected = false;
        enemyGrandparentTransform = gameObject.transform.parent;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && enemyProximity.ReturnEnemyWithinRange() && !ability.ReturnUsingInvisibility())
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
