using System.Collections;
using UnityEngine;
using TMPro; // Use this if you are using TextMeshPro

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
        enemyGrandparentTransform = transform.parent.parent;
        flyingBotStateMachine = enemyGrandparentTransform.GetComponent<FlyingBotStateMachine>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (IsPlayer(other) && flyingBotStateMachine.ReturnDetectingPlayer() == true && !ability.ReturnUsingInvisibility())
        {
            HandlePlayerDetection();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        Debug.Log(this.gameObject.name + other.name);
        if (IsPlayer(other))
        {
            bool withinRange = flyingBotStateMachine.ReturnDetectingPlayer();
            sceneActivity.SetPlayerIsSpotted(withinRange);

            if (!withinRange)
            {
                ResetPlayerDetection();
            }
        }
        else
        {
            ResetPlayerDetection();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (IsPlayer(other))
        {
            Disengage();
        }
    }

    public void Disengage()
    {
        if(flyingBotStateMachine.ReturnDetectingPlayer() == false)
        {
            ResetPlayerDetection();
            flyingBotStateMachine.ChangeBehavior(FlyingBotStateMachine.FlyingState.patrolling);
            flyingBotStateMachine.SetPatrollingStatus(true);
        }
    }

    private void DetectingPlayer()
    {
        if (flyingBotStateMachine.ReturnDetectingPlayer() && sceneActivity.ReturnPlayerSpotted() && playerIsBeingDetected)
        {
            playerDetectionState.ChangeDetectionState(PlayerDetectionState.DetectionState.beingDetected);
            playerIsBeingDetected = false;
        }
    }

    bool IsPlayer(Collider other)
    {
        return other.CompareTag("Player");
    }

    void HandlePlayerDetection()
    {
        sceneActivity.SetPlayerIsSpotted(true);
        playerIsBeingDetected = true;

        playerDetectionState.SetFlyingBotStateMachine(flyingBotStateMachine);
        playerDetectionState.SetFlyingBotFieldOfView(this);

        playerDetectionState.SetDetectedByFlyingBot(true);
        flyingBotStateMachine.SetPatrollingStatus(false);
        flyingBotStateMachine.ChangeBehavior(FlyingBotStateMachine.FlyingState.lookingAtPlayer);
    }

    void ResetPlayerDetection()
    {
        sceneActivity.SetPlayerIsSpotted(false);
        playerIsBeingDetected = false;
        playerDetectionState.ChangeDetectionState(PlayerDetectionState.DetectionState.meterRepleneshing);
        playerDetectionState.SetDetectedByFlyingBot(false);
        flyingBotStateMachine.ChangeBehavior(FlyingBotStateMachine.FlyingState.patrolling);
    }

    public Transform ReturnThisEnemy()
    {
        return enemyGrandparentTransform;
    }
}


