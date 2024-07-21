using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder.MeshOperations;

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
    [SerializeField] private PlayerAbilities playerAbilities;

    private void Start()
    {
        raycastDistance = 7.5f;
        playerController = GameManager.instance.ReturnPlayerController();
        playerAbilities = GameManager.instance.ReturnPlayerAbilities();
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
            
            if (IsTagInList(hitInfo.collider.tag))
            {
                enemyWithinRange = true;
                if(hitInfo.collider.tag == "GroundBot" && enemyWithinRange)
                {
                    groundBotStateMachine = hitInfo.collider.GetComponent<GroundBotStateMachine>();
                    groundBotStateMachine.SetDetectingPlayer(true);

                    if (Vector3.Distance(transform.position, hitInfo.collider.transform.position) < 1.5f && !playerAbilities.ReturnUsingInvisibility())
                    {
                        Debug.Log("Touched the bot");
                        groundBotStateMachine = hitInfo.collider.GetComponent<GroundBotStateMachine>();
                        groundBotStateMachine.ChangeBehavior(GroundBotStateMachine.BehaviorState.playerCaught);
                        playerController.SetPlayerActivity(false);
                    }
                }
                else if(hitInfo.collider.tag == "GroundBot" && !enemyWithinRange)
                {
                    groundBotStateMachine = hitInfo.collider.GetComponent<GroundBotStateMachine>();
                    groundBotStateMachine.SetDetectingPlayer(false);
                }
            }
            else
            {
                enemyWithinRange = false;
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
        return enemyWithinRange;
    }
}
