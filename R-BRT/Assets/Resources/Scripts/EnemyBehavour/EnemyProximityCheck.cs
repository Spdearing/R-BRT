using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProximityCheck : MonoBehaviour
{
    [Header("Bools")]
    [SerializeField] private bool enemyWithinRange;

    [Header("Floats")]
    [SerializeField] private float raycastDistance;

    [Header("LayerMask")]
    [SerializeField] private LayerMask ignoreLayerMask;

    [Header("List Of Enemies")]
    [SerializeField] private List<GameObject> detectedEnemies = new List<GameObject>();

    [Header("Array Of Tags To Compare")]
    [SerializeField] private string[] enemyTags = new string[] { "GroundBot", "SpiderBot", "FlyingBot" };


    [Header("Scripts")]
    [SerializeField] private PlayerController playerController;
    [SerializeField] private GroundBotStateMachine groundBotStateMachine;

    private void Start()
    {
        raycastDistance = 10.0f;
        playerController = GameManager.instance.ReturnPlayerController();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (IsTagInList(other.tag))
        {
            detectedEnemies.Add(other.gameObject);
            DetectEnemy(other.gameObject);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (IsTagInList(other.tag))
        {
            DetectEnemy(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (IsTagInList(other.tag))
        {
            detectedEnemies.Remove(other.gameObject);
            if (detectedEnemies.Count == 0)
            {
                enemyWithinRange = false;
            }
        }
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
            Debug.Log(hitInfo.collider.tag);
            Debug.Log(raycastDistance);

            if (IsTagInList(hitInfo.collider.tag))
            {
                enemyWithinRange = true;

                if(hitInfo.collider.tag == "GroundBot" && Vector3.Distance(transform.position, hitInfo.collider.transform.position) < 1.0f)
                {
                    groundBotStateMachine = hitInfo.collider.GetComponent<GroundBotStateMachine>();
                    groundBotStateMachine.ChangeBehavior(GroundBotStateMachine.BehaviorState.playerCaught);
                    playerController.SetPlayerActivity(false);
                }
            }
            else
            {
                enemyWithinRange = false;
            }
        }
        else
        {
            enemyWithinRange = false;
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
        return enemyWithinRange;
    }
}
