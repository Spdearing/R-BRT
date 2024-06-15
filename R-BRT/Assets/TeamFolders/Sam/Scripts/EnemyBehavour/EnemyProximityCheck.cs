using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GroundBotStateMachine;

public class EnemyProximityCheck : MonoBehaviour
{
    [Header("Bools")]
    [SerializeField] bool enemyWithinRange;

    [Header("Game Manager")]
    [SerializeField] GameManager gameManager;

    [Header("Floats")]
    [SerializeField] private float raycastDistance;

    [Header("Vector3")]
    [SerializeField] private Vector3 rayCastDirection;

    [Header("LayerMask")]
    [SerializeField] private LayerMask ignoreLayerMask;

    [Header("List Of Enemies")]
    [SerializeField] private List<GameObject> detectedEnemies = new List<GameObject>();

    [Header("Array Of Tags To Compare")]
    [SerializeField] private string[] enemyTags;


    private void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        raycastDistance = 10.0f;

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
        return this.enemyWithinRange;
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
            if(IsTagInList(hitInfo.collider.tag))
            {
                enemyWithinRange = true;
            }
        }
    }

    private bool IsTagInList(string tag)
    {
        foreach (var enemyTag in enemyTags)
        {
            if (tag == enemyTag)
            {
                return true;
            }
        }
        return false;
    }

    public bool ReturnEnemyWithinRange()
    {
        return this.enemyWithinRange;
    }
}




        