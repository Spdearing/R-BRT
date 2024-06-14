using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GroundBotStateMachine;

public class EnemyProximityCheck : MonoBehaviour
{
    [Header("Game Objects")]

    [Header("String")]

    [Header("Bools")]
    [SerializeField] bool playerWithinRange;

    [Header("Scripts")]

    [Header("Game Manager")]
    [SerializeField] GameManager gameManager;

    [Header("Floats")]
    [SerializeField] private float capsuleHeight;
    [SerializeField] private float raycastDistance;

    [Header("Vector3")]
    [SerializeField] private Vector3 rayCastDirection;

    [Header("LayerMask")]
    [SerializeField] private LayerMask ignoreLayerMask;

    [Header("List Of Enemies")]
    [SerializeField] private List<GameObject> detectedEnemies = new List<GameObject>();


    private void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        capsuleHeight = 0.96f;
        raycastDistance = 10.0f;

    }

    private void Update()
    {
        //if (playerWithinRange && groundBotDetection.ReturnPlayerDetected() == false)
        //{
        //    Vector3 playerCenterPosition = player.transform.position + new Vector3(0, capsuleHeight / 2, 0);


        //    enemyRaycast.transform.LookAt(playerCenterPosition);
        //}
    }


    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("GroundBot"))
        {
            detectedEnemies.Add(other.gameObject);
            DetectEnemy(other.gameObject);
        }
    }

    public void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("GroundBot"))
        {
            DetectEnemy(other.gameObject);
        }
    }


    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("GroundBot"))
        {
            detectedEnemies.Remove(other.gameObject);
        }
    }

    public bool ReturnPlayerProximity()
    {
        return this.playerWithinRange;
    }

    private void DetectEnemy(GameObject enemyDetected)
    {
        if (enemyDetected == null) 
            
            return;

        Vector3 rayOrigin = transform.position;
        Vector3 rayDirection = (enemyDetected.transform.position - transform.position).normalized;

        RaycastHit hitInfo;

        Debug.DrawRay(rayOrigin, rayDirection * raycastDistance, Color.blue);

        if (Physics.Raycast(rayOrigin, rayDirection, out hitInfo, raycastDistance, ~ignoreLayerMask))
        {
       

          
        }
        else
        {
            Debug.Log("Raycast did not hit anything.");
        }
    }
}




        