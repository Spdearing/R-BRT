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
    [SerializeField] private string[] enemyTags;

    [Header("Scripts")]
    [SerializeField] private PlayerController playerController;
    [SerializeField] private GroundBotStateMachine groundBotStateMachine;
    [SerializeField] private FlyingBotStateMachine flyingBotStateMachine;
    [SerializeField] private PlayerAbilities playerAbilities;

    private HashSet<string> enemyTagSet;

    private void Start()
    {
        enemyTags = new string[] { "GroundBot", "FlyingBot" };
        raycastDistance = 7.5f;
        playerController = GameManager.instance.ReturnPlayerController();
        playerAbilities = GameManager.instance.ReturnPlayerAbilities();

        enemyTagSet = new HashSet<string>(enemyTags);
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
                if (hitInfo.collider.tag == "GroundBot")
                {
                    groundBotStateMachine = hitInfo.collider.GetComponent<GroundBotStateMachine>();
                    if (groundBotStateMachine != null)
                    {
                        groundBotStateMachine.SetDetectingPlayer(true);
                        enemyWithinRange = true;

                        if (Vector3.Distance(transform.position, hitInfo.collider.transform.position) < 1.5f && !playerAbilities.ReturnUsingInvisibility())
                        {
                            Debug.Log("Touched the bot");
                            groundBotStateMachine.ChangeBehavior(GroundBotStateMachine.BehaviorState.playerCaught);
                            playerController.SetPlayerActivity(false);
                        }
                    }
                }
                else if (hitInfo.collider.tag == "FlyingBot")
                {
                    flyingBotStateMachine = hitInfo.collider.GetComponent<FlyingBotStateMachine>();
                    if (flyingBotStateMachine != null)
                    {
                        flyingBotStateMachine.SetDetectingPlayer(true);
                    }
                }
            }
            else
            {
                ResetDetectionStates();
            }
        }
        else
        {
            ResetDetectionStates();
        }
    }

    private void ResetDetectionStates()
    {
        if (flyingBotStateMachine != null)
        {
            flyingBotStateMachine.SetDetectingPlayer(false);
        }
        if (groundBotStateMachine != null)
        {
            groundBotStateMachine.SetDetectingPlayer(false);
        }
        enemyWithinRange = false;
    }

    private bool IsTagInList(string tag)
    {
        return enemyTagSet.Contains(tag);
    }

    public bool ReturnEnemyWithinRange()
    {
        return enemyWithinRange;
    }
}
