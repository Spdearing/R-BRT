using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadRaycastGroundBotDetection : MonoBehaviour
{
    [SerializeField] private float raycastDistance; // Distance of the raycast
    [SerializeField] private GameObject groundBotHead;
    [SerializeField] private Renderer groundBotHeadColor;
    [SerializeField] private GameObject player; // Reference to the player object
    [SerializeField] private GroundBotHeadMovement headMovement; // Reference to the HeadMovement script
    [SerializeField] private DetectionMeter detection; // Reference to the DetectionMeter script
    [SerializeField] private float detectionIncreaseRate; // Base rate at which detection increases
    [SerializeField] private float detectionDecreaseRate; // Rate at which detection decreases when player is not detected

    private bool playerDetected = false; // Flag to track player detection status

    void Start()
    {
        detectionDecreaseRate = 25.0f;
        detectionIncreaseRate = 5.0f;
        raycastDistance = 10.0f;
        player = GameObject.FindWithTag("Player"); // Find the player by tag
        headMovement = GetComponent<GroundBotHeadMovement>(); // Get the HeadMovement component
        groundBotHead = gameObject.transform.Find("Head").gameObject;
        groundBotHeadColor = groundBotHead.GetComponent<Renderer>();
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
                transform.LookAt(lookPosition); 
                detection.IncreaseDetection(detectionIncreaseRate);
                detectionIncreaseRate = (detectionIncreaseRate + .5f);
            }
            else
            {
                playerDetected = false;
            }
        }
        else
        {
            playerDetected = false;
        }

        if (playerDetected)
        {
            headMovement.SetPlayerSpotted(true); // Notify the head movement script
            groundBotHeadColor.material.color = Color.red; // Change light color to red
        }
        else
        {
            headMovement.SetPlayerSpotted(false); // Notify the head movement script
            groundBotHeadColor.material.color = Color.green; // Change light color to green
            detection.DecreaseDetection(detectionDecreaseRate); // Gradually decrease detection when the player is not detected
            detectionIncreaseRate = 5.0f;

        }
    }
}
