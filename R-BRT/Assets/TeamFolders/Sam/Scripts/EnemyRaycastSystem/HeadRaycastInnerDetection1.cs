using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadRaycastInnerDetection : MonoBehaviour
{
    [SerializeField] private float raycastDistance;
    [SerializeField] private float detectionAngle; // Total angle of the cone
    [SerializeField] private int numberOfRays; // Number of rays in the cone
    [SerializeField] Light enemyLight;
    [SerializeField] private GameObject player;
    [SerializeField] private HeadMovement headMovement;

    // Start is called before the first frame update
    void Start()
    {
        detectionAngle = 50.0f;
        numberOfRays = 3;
        raycastDistance = 10.0f;
        player = GameObject.Find("Player");
        headMovement = GetComponent<HeadMovement>();
        enemyLight = GetComponentInChildren<Light>();
        enemyLight.color = Color.green;
    }

    // Update is called once per frame
    void Update()
    {
        bool playerDetected = false;

        float angleStep = detectionAngle / (numberOfRays - 1);
        float startAngle = -detectionAngle / 2;

        for (int i = 0; i < numberOfRays; i++)
        {
            float angle = startAngle + (angleStep * i);
            Vector3 rayDirection = Quaternion.Euler(0, angle, 0) * transform.forward;
            Ray ray = new Ray(transform.position, rayDirection);
            RaycastHit hitInfo;

            Debug.DrawRay(ray.origin, ray.direction * raycastDistance, Color.yellow);

            if (Physics.Raycast(ray, out hitInfo, raycastDistance))
            {
                if (hitInfo.collider.CompareTag("Player"))
                {
                    playerDetected = true;
                    Vector3 lookPosition = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);
                    transform.LookAt(lookPosition);
                    break;
                }
            }
        }

        if (playerDetected)
        {
            headMovement.SetPlayerSpotted(true);
            enemyLight.color = Color.red;
        }
        else
        {
            headMovement.SetPlayerSpotted(false);
            enemyLight.color = Color.green;
        }
    }
}
