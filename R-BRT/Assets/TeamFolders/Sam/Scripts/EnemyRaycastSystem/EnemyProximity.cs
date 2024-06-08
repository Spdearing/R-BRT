using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProximity : MonoBehaviour
{
    [SerializeField] private GameObject enemyRaycast;
    [SerializeField] private GameObject player;
    [SerializeField] private bool playerWithinRange;
    [SerializeField] private GroundBotHeadRaycastDetection groundBotDetection;

    private void Start()
    {
        // Using GetComponentInChildren to find the GroundBotHeadRaycastDetection script within the prefab instance
        groundBotDetection = GetComponentInChildren<GroundBotHeadRaycastDetection>();

        // Ensure player is correctly referenced
        player = GameObject.FindWithTag("Player");

        // Validate component assignments
        if (groundBotDetection == null)
        {
            Debug.LogError("GroundBotHeadRaycastDetection component not found in children.");
        }

        if (player == null)
        {
            Debug.LogError("Player GameObject not found.");
        }
    }

    private void Update()
    {
        if (playerWithinRange && groundBotDetection.ReturnPlayerDetected() == false)
        {
            Vector3 playerPosition = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);
            enemyRaycast.transform.LookAt(playerPosition);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerWithinRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerWithinRange = false;
        }
    }

    public bool ReturnPlayerProximity()
    {
        return playerWithinRange;
    }
}
