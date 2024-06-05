using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadRaycastDetection : MonoBehaviour
{
    [SerializeField] private float raycastDistance; // Distance of the raycast
    [SerializeField] private Light enemyLight; // Light component on the enemy
    [SerializeField] private GameObject player; // Reference to the player object
    [SerializeField] private HeadMovement headMovement; // Reference to the HeadMovement script
    [SerializeField] private DetectionMeter detection; // Reference to the DetectionMeter script
    [SerializeField] private float detectionDelay; // Delay before increasing detection
    [SerializeField] private float detectionAmount; // Amount of detection increase

    private float detectionTimer = 0.0f; // Timer to track detection delay
    private float detectionPeriod = 0.0f; // Period of time that the player is being detected.
    private bool playerDetected = false; // Flag to track player detection status

    void Start()
    {
        detectionDelay = .0005f;
        detectionAmount = .075f;
        raycastDistance = 10.0f;
        player = GameObject.FindWithTag("Player"); // Find the player by tag
        headMovement = GetComponent<HeadMovement>(); // Get the HeadMovement component
        enemyLight = GetComponentInChildren<Light>(); // Get the Light component in children
        enemyLight.color = Color.green; // Set initial light color to green
        detection = GameObject.Find("EnemyDetectionManager").GetComponent<DetectionMeter>(); // Find the DetectionMeter script
    }

    void Update()
    {
        Vector3 rayDirection = transform.forward; // Direction of the ray
        Ray ray = new Ray(transform.position, rayDirection);
        RaycastHit hitInfo;

        Debug.DrawRay(ray.origin, ray.direction * raycastDistance, Color.yellow); // Draw the ray in the Scene view

        if (Physics.Raycast(ray, out hitInfo, raycastDistance))
        {
            if (hitInfo.collider.CompareTag("Player"))
            {
                playerDetected = true;
                Vector3 lookPosition = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);
                transform.LookAt(lookPosition); // Make the head look at the player
                detectionTimer += Time.deltaTime; // Increment the detection timer
                detectionPeriod += Time.deltaTime; // Increment the detection timer

                if (detectionTimer >= detectionDelay)
                {
                    detection.IncreaseDetection(detectionAmount); // Increase detection level
                    detectionTimer = 0.0f; // Reset the detection timer
                }
            }
            else
            {
                playerDetected = false;
                detectionTimer = 0.0f; // Reset the detection timer if the player is not detected
            }
        }
        else
        {
            playerDetected = false;
            detectionTimer = 0.0f; // Reset the detection timer if the player is not detected
        }

        if (playerDetected)
        {
            headMovement.SetPlayerSpotted(true); // Notify the head movement script
            enemyLight.color = Color.red; // Change light color to red
        }
        else
        {
            headMovement.SetPlayerSpotted(false); // Notify the head movement script
            enemyLight.color = Color.green; // Change light color to green
        }
    }
}
