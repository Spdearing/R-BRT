using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GroundBotStateMachine;

public class GroundBotHeadRaycastDetection : MonoBehaviour
{
    [Header("Scripts")]
    [SerializeField] private GameManager gameManager;
    [SerializeField] private EnemyFieldOfView enemyFieldOfView;
    [SerializeField] private GroundBotHeadMovement headMovement;
    [SerializeField] private EnemyProximity proximityCheck;
    [SerializeField] private DetectionMeter detection;
    [SerializeField] private GroundBotStateMachine groundBotBehaviour;
    [SerializeField] private PlayerController playerController;

    [Header("Floats")]
    [SerializeField] private float raycastDistance;
    [SerializeField] private float detectionIncreaseRate;
    [SerializeField] private float detectionDecreaseRate;
    [SerializeField] private float capsuleHeight;

    [Header("Game Objects")]
    [SerializeField] private GameObject groundBotHead;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject groundBot;

    [Header("Renderer")]
    [SerializeField] private Renderer groundBotHeadColor;

    [Header("Bools")]
    [SerializeField] private bool playerDetected;
    [SerializeField] private bool playerIsBeingTracked;

    [Header("LayerMask")]
    [SerializeField] private LayerMask ignoreLayerMask;

    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        playerIsBeingTracked = false;
        playerDetected = false;
        detectionDecreaseRate = 25.0f;
        detectionIncreaseRate = 5.0f;
        raycastDistance = 10.0f;
        player = gameManager.ReturnPlayer();
        detection = gameManager.ReturnDetectionMeter();
        groundBotBehaviour = GetComponentInParent<GroundBotStateMachine>();
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
            if (hitInfo.collider.CompareTag("Player") && playerController.ReturnInvisibilityStatus())
            {
                if (playerIsBeingTracked)
                {
                    playerDetected = true;
                    Vector3 playerCenterPosition = player.transform.position + new Vector3(0, capsuleHeight / 2.5f, 0);
                    groundBot.transform.LookAt(playerCenterPosition);
                    transform.LookAt(playerCenterPosition);

                    if (playerDetected && playerController.ReturnInvisibilityStatus())
                    {
                        groundBotBehaviour.ChangeBehaviour(BehaviourState.scanning);
                        detection.IncreaseDetection(detectionIncreaseRate);
                        detectionIncreaseRate += 0.5f;
                        headMovement.SetPlayerSpotted(true);
                        groundBotHeadColor.material.color = Color.red;

                        if (detection.ReturnStartingDetection() == 200)
                        {
                            groundBotBehaviour.ChasePlayer(true);
                            groundBotBehaviour.ChangeBehaviour(BehaviourState.playerCaught);
                        }
                    }
                }
                else if (!playerIsBeingTracked)
                {
                    groundBotBehaviour.ChangeBehaviour(BehaviourState.patrolling);
                    headMovement.SetPlayerSpotted(false);
                    groundBotHeadColor.material.color = Color.green;
                    detection.DecreaseDetection(detectionDecreaseRate);
                    detectionIncreaseRate = 5.0f;
                }
            }
            else
            {
                playerDetected = false;
                headMovement.SetPlayerSpotted(false);
                groundBotHeadColor.material.color = Color.green;
                detection.DecreaseDetection(detectionDecreaseRate);
                detectionIncreaseRate = 5.0f;
            }
        }
        else
        {
            headMovement.SetPlayerSpotted(false);
            groundBotHeadColor.material.color = Color.green;
            detection.DecreaseDetection(detectionDecreaseRate);
            detectionIncreaseRate = 5.0f;
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
