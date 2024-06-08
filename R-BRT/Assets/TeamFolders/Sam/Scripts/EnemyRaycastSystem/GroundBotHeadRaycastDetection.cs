using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundBotHeadRaycastDetection : MonoBehaviour
{
    [SerializeField] private float raycastDistance; // Distance of the raycast
    [SerializeField] private GameObject groundBotHead;
    [SerializeField] private Renderer groundBotHeadColor;
    [SerializeField] private GameObject player; // Reference to the player object
    [SerializeField] private EnemyFieldOfView enemyFieldOfView;
    [SerializeField] private GameObject groundBot;
    [SerializeField] private GroundBotHeadMovement headMovement; // Reference to the HeadMovement script
    [SerializeField] private EnemyProximity proximityCheck;
    [SerializeField] private DetectionMeter detection; // Reference to the DetectionMeter script
    [SerializeField] private float detectionIncreaseRate; // Base rate at which detection increases
    [SerializeField] private float detectionDecreaseRate; // Rate at which detection decreases when player is not detected

    [SerializeField] private bool playerDetected; // Flag to track player detection status
    [SerializeField] private bool playerIsBeingTracked;
    [SerializeField] private LayerMask ignoreLayerMask;

    void Start()
    {
        playerIsBeingTracked = false;
        playerDetected = false;
        detectionDecreaseRate = 25.0f;
        detectionIncreaseRate = 5.0f;
        raycastDistance = 10.0f;
        player = GameObject.Find("Player"); // Find the player by tag
        groundBot = GameObject.FindWithTag("GroundBot");
        enemyFieldOfView = groundBot.GetComponentInChildren<EnemyFieldOfView>(); 
        headMovement = GameObject.Find("GroundBot").GetComponent<GroundBotHeadMovement>(); // Get the HeadMovement component
        proximityCheck = GameObject.Find("GroundBot").GetComponent<EnemyProximity>();
        groundBotHead = GameObject.FindWithTag("GroundBotHead");
        groundBotHeadColor = groundBotHead.GetComponent<Renderer>();
        detection = GameObject.Find("EnemyDetectionManager").GetComponent<DetectionMeter>(); // Find the DetectionMeter script
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

            if (hitInfo.collider.CompareTag("Player"))
            {
                if (playerIsBeingTracked)
                {
                    playerDetected = true;
                    Vector3 lookPosition = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);
                    groundBot.transform.LookAt(lookPosition);
                    transform.LookAt(lookPosition);

                    if (playerDetected)
                    {
                        detection.IncreaseDetection(detectionIncreaseRate);
                        detectionIncreaseRate = (detectionIncreaseRate + .5f);
                        headMovement.SetPlayerSpotted(true); // Notify the head movement script
                        groundBotHeadColor.material.color = Color.red; // Change light color to red
                    }
                }
                else if (!playerIsBeingTracked)
                {
                    headMovement.SetPlayerSpotted(false); // Notify the head movement script
                    groundBotHeadColor.material.color = Color.green; // Change light color to green
                    detection.DecreaseDetection(detectionDecreaseRate); // Gradually decrease detection when the player is not detected
                    detectionIncreaseRate = 5.0f;
                }
            }
            else
            {
                playerDetected = false;
                headMovement.SetPlayerSpotted(false); // Notify the head movement script
                groundBotHeadColor.material.color = Color.green; // Change light color to green
                detection.DecreaseDetection(detectionDecreaseRate); // Gradually decrease detection when the player is not detected
                detectionIncreaseRate = 5.0f;
            }
        }

        else 
        {
            headMovement.SetPlayerSpotted(false); // Notify the head movement script
            groundBotHeadColor.material.color = Color.green; // Change light color to green
            detection.DecreaseDetection(detectionDecreaseRate); // Gradually decrease detection when the player is not detected
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
