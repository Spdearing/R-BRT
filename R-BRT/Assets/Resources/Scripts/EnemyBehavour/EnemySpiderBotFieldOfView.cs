using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.tag);
        if (IsPlayer(other) && IsDetectable())
        {
            sceneActivity.SetPlayerIsSpotted(true);
            playerIsBeingDetected = true;
            SetSpiderBotStateToScanning();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (IsPlayer(other))
        {
            bool withinRange = enemyProximity.ReturnEnemyWithinRange();
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

    private void SetSpiderBotStateToScanning()
    {
        Transform grandparentTransform = gameObject.transform.parent.parent;
        SpiderBotStateMachine stateMachine = gameObject.GetComponentInChildren<SpiderBotStateMachine>();
        EnemySpiderBotFieldOfView enemyFieldOfView = GetComponent<EnemySpiderBotFieldOfView>();

        if (stateMachine != null && enemyFieldOfView != null)
        {
            playerDetectionState.SetSpiderBotStateMachine(stateMachine);
            playerDetectionState.SetSpiderBotFieldOfView(enemyFieldOfView);
            stateMachine.ChangeBehavior(SpiderBotStateMachine.SpiderState.scanning);
        }
    }

    private void ResetDetection()
    {
        sceneActivity.SetPlayerIsSpotted(false);
        playerIsBeingDetected = false;
        playerDetectionState.ChangeDetectionState(PlayerDetectionState.DetectionState.meterRepleneshing);
    }

    private bool IsPlayer(Collider other)
    {
        return other.CompareTag("Player");
    }

    private bool IsDetectable()
    {
        return enemyProximity.ReturnEnemyWithinRange();
    }

    public Transform ReturnThisEnemy()
    {
        return this.gameObject.transform.parent;
    }
}
