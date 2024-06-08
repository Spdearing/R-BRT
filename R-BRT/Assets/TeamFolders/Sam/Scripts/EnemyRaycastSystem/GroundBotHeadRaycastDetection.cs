using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundBotHeadRaycastDetection : MonoBehaviour
{
    [SerializeField] private float raycastDistance = 10.0f; // Distance of the raycast
    [SerializeField] private Renderer groundBotHeadColor;
    [SerializeField] private GameObject player; // Reference to the player object
    [SerializeField] private GameObject groundBot;
    [SerializeField] private EnemyFieldOfView enemyFieldOfView;
    [SerializeField] private GroundBotHeadMovement headMovement; // Reference to the HeadMovement script
    [SerializeField] private EnemyProximity proximityCheck;
    [SerializeField] private DetectionMeter detection; // Reference to the DetectionMeter script
    [SerializeField] private float detectionIncreaseRate = 5.0f; // Base rate at which detection increases
    [SerializeField] private float detectionDecreaseRate = 25.0f; // Rate at which detection decreases when player is not detected

    private bool playerDetected = false; // Flag to track player detection status
    private bool playerIsBeingTracked = false;
    [SerializeField] private LayerMask ignoreLayerMask;

    void Start()
    {
        // Use GetComponentInChildren or GetComponentInParent if the components are in the hierarchy
        player = GameObject.FindWithTag("Player");
        groundBot = GameObject.FindWithTag("GroundBot");

        if (player != null)
        {
            enemyFieldOfView = groundBot.GetComponentInChildren<EnemyFieldOfView>();

            if (enemyFieldOfView == null)
            {
                Debug.LogError("EnemyFieldOfView component not found on the Player GameObject.");
            }
        }
        else
        {
            Debug.LogError("Player GameObject not found.");
        }

        // Get components on the same GameObject or its children
        groundBotHeadColor = GetComponentInChildren<Renderer>();
        headMovement = GetComponent<GroundBotHeadMovement>();
        proximityCheck = GetComponent<EnemyProximity>();
        detection = FindObjectOfType<DetectionMeter>(); // Assuming there's only one DetectionMeter in the scene

        // Validate component assignments
        if (groundBotHeadColor == null || headMovement == null || proximityCheck == null || detection == null)
        {
            Debug.LogError("Required components not found on GroundBot or its children.");
        }
    }

    void Update()
    {
        EnemyLockedOn();

        Vector3 rayDirection = transform.forward; // Direction of the ray
        Ray ray = new Ray(transform.position, rayDirection);
        RaycastHit hitInfo;

        Debug.DrawRay(ray.origin, ray.direction * raycastDistance, Color.yellow); // Draw the ray in the Scene view

        // Raycast with LayerMask to ignore specified layers
        if (Physics.Raycast(ray, out hitInfo, raycastDistance, ~ignoreLayerMask))
        {
            Debug.Log(hitInfo.collider.gameObject);

            if (hitInfo.collider.CompareTag("Player"))
            {
                if (playerIsBeingTracked)
                {
                    playerDetected = true;
                    Vector3 lookPosition = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);
                    transform.LookAt(lookPosition);

                    if (playerDetected)
                    {
                        detection.IncreaseDetection(detectionIncreaseRate);
                        detectionIncreaseRate += 0.5f;
                        headMovement.SetPlayerSpotted(true); // Notify the head movement script
                        groundBotHeadColor.material.color = Color.red; // Change light color to red
                    }
                }
                else
                {
                    ResetDetection();
                }
            }
            else
            {
                playerDetected = false;
                ResetDetection();
            }
        }
        else
        {
            ResetDetection();
        }
    }

    void EnemyLockedOn()
    {
        playerIsBeingTracked = proximityCheck.ReturnPlayerProximity() && enemyFieldOfView.ReturnPlayerSpotted();
    }

    void ResetDetection()
    {
        headMovement.SetPlayerSpotted(false); // Notify the head movement script
        groundBotHeadColor.material.color = Color.green; // Change light color to green
        detection.DecreaseDetection(detectionDecreaseRate); // Gradually decrease detection when the player is not detected
        detectionIncreaseRate = 5.0f;
    }

    public bool ReturnPlayerDetected()
    {
        return playerDetected;
    }
}
