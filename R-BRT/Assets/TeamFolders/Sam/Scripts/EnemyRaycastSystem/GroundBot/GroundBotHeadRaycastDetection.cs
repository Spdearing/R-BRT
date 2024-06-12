using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GroundBotStateMachine;

public class GroundBotHeadRaycastDetection : MonoBehaviour
{
    [Header("Scripts")]
    [SerializeField] private GameManager gameManager;
    [SerializeField] private DetectionMeter detectionMeter;
    [SerializeField] private EnemyFieldOfView enemyFieldOfView;
    [SerializeField] private GroundBotHeadMovement headMovement;
    [SerializeField] private GroundEnemyProximity proximityCheck;
    [SerializeField] private GroundBotStateMachine groundBotBehavior;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private PlayerDetectionState playerDetectionState;

    [Header("Floats")]
    [SerializeField] private float raycastDistance;
    
    [SerializeField] private float capsuleHeight;

    [Header("Game Objects")]
    [SerializeField] private GameObject groundBotHead;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject groundBot;


    [Header("Bools")]
    [SerializeField] private bool playerDetected;
    [SerializeField] private bool playerIsBeingTracked;
    [SerializeField] private bool playerHasBeenDetected;

    [Header("LayerMask")]
    [SerializeField] private LayerMask ignoreLayerMask;

    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        playerIsBeingTracked = false;
        playerDetected = false;
        playerHasBeenDetected = false;
        raycastDistance = 10.0f;
        player = gameManager.ReturnPlayer();
        groundBotBehavior = GetComponentInParent<GroundBotStateMachine>();
        capsuleHeight = 0.96f;
    }

    void Update()
    {
        EnemyLockedOn();

        Vector3 rayDirection = transform.forward;
        Ray ray = new Ray(transform.position, rayDirection);
        RaycastHit hitInfo;

        Debug.DrawRay(ray.origin, ray.direction * raycastDistance, Color.yellow);

        if (Physics.Raycast(ray, out hitInfo, raycastDistance, ~ignoreLayerMask))
        {
            if (hitInfo.collider.CompareTag("Player") && playerController.ReturnInvisibilityStatus() == true)
            {
                if (playerIsBeingTracked)
                {
                    playerDetected = true;
                    Vector3 playerCenterPosition = player.transform.position + new Vector3(0, capsuleHeight / 2.5f, 0);
                    groundBot.transform.LookAt(playerCenterPosition);
                    transform.LookAt(playerCenterPosition);

                    if (playerDetected && playerController.ReturnInvisibilityStatus() == true && playerHasBeenDetected == false)
                    {
                        playerHasBeenDetected = true;
                        PlayerBeingDetected();
                        headMovement.SetPlayerSpotted(true);
                    }
                }
                else if (!playerIsBeingTracked)
                {
                    playerHasBeenDetected = false;
                    EnemyDisengaged();
                    groundBotBehavior.ChangeBehavior(BehaviorState.patrolling);
                    headMovement.SetPlayerSpotted(false);
                }
            }
            else
            {
                playerHasBeenDetected = false;
                EnemyDisengaged();
                playerDetected = false;
                headMovement.SetPlayerSpotted(false);
            }
        }
        else
        {
            EnemyDisengaged();
            headMovement.SetPlayerSpotted(false);
        }
    }

    void PlayerBeingDetected()
    {
        if(playerDetected)
        {
            playerDetectionState.ChangeDetectionState(PlayerDetectionState.DetectionState.beingDetected);
        }
        
    }

    public void EnemyDisengaged()
    {
        
        if (detectionMeter.ReturnStartingDetection() <= 200 && detectionMeter.ReturnStartingDetection() >= 0)
        {
            playerDetectionState.ChangeDetectionState(PlayerDetectionState.DetectionState.meterRepleneshing);
        }
        else if (detectionMeter.ReturnStartingDetection() <= 0)
        {
            playerDetectionState.ChangeDetectionState(PlayerDetectionState.DetectionState.exploring);
        }
    }

    void EnemyLockedOn()
    {
        playerIsBeingTracked = proximityCheck.ReturnPlayerProximity() && enemyFieldOfView.ReturnPlayerSpotted();
    }

    public bool ReturnPlayerDetected()
    {
        return this.playerDetected;
    }
    public void SetPlayerDetected(bool value)
    {
        this.playerDetected = value;
    }
}
