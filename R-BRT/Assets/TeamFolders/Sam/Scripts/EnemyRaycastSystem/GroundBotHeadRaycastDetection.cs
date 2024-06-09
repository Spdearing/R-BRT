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
        player = gameManager.ReturnPlayer(); // Find the player by tag
        detection = gameManager.ReturnDetectionMeter();
        groundBotBehaviour = GetComponentInParent<GroundBotStateMachine>();
    }

    void Update()
    {
        EnemyLockedOn();

        Vector3 rayDirection = transform.forward; // Direction of the ray
        Ray ray = new Ray(transform.position, rayDirection);
        RaycastHit hitInfo;

        Debug.DrawRay(ray.origin, ray.direction * raycastDistance, Color.yellow); // Draw the ray in the Scene view

        if (Physics.Raycast(ray, out hitInfo, raycastDistance, ~ignoreLayerMask))
        {
            Debug.Log(hitInfo.collider.gameObject);

            if (hitInfo.collider.CompareTag("Player") && playerController.ReturnInvisibilityStatus() == true)// player has not used invisibility yet
            {
                if (playerIsBeingTracked)
                {
                    
                    playerDetected = true;
                    Vector3 lookPosition = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);
                    groundBot.transform.LookAt(lookPosition);
                    transform.LookAt(lookPosition);

                    if (playerDetected && playerController.ReturnInvisibilityStatus() == true)//player has not used invisibility yet
                    {
                        groundBotBehaviour.ChangeBehaviour(BehaviourState.scanning);
                        detection.IncreaseDetection(detectionIncreaseRate);
                        detectionIncreaseRate = (detectionIncreaseRate + .5f);
                        headMovement.SetPlayerSpotted(true); // Notify the head movement script
                        groundBotHeadColor.material.color = Color.red; // Change light color to red

                        if(detection.ReturnStartingDetection() == 200)// if the detection bar becomes full
                        {
                            groundBotBehaviour.ChangeBehaviour(BehaviourState.chasing); // Enemy will chase the player
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
        if (proximityCheck.ReturnPlayerProximity() == true && enemyFieldOfView.ReturnPlayerSpotted() == true)
        {
            playerIsBeingTracked = true;
        }

        else
        {
            playerIsBeingTracked = false;
        }
    }

    public bool ReturnPlayerDetected()
    {
        return this.playerDetected;
    }
}
