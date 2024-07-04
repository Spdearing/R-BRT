using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class GroundBotAIMovement : MonoBehaviour
{
    [Header("Nav Mesh")]
    [SerializeField] private NavMeshAgent groundBotAI;

    [Header("Scripts")]
    [SerializeField] GroundBotHeadMovement groundBotHeadMovement;

    [Header("Transforms")]
    [SerializeField] private List<Transform> patrolPoints1 = new List<Transform>();
    [SerializeField] private List<Transform> patrolPoints2 = new List<Transform>();

    [Header("Int")]
    [SerializeField] private int currentWaypointIndex;

    [SerializeField] private bool isWaiting;

    [Header("Detection")]
    [SerializeField] private float detectionSpeed = 1.0f;
    [SerializeField] private float crouchDetectionMultiplier = 0.5f; // Multiplier to reduce detection speed when crouching

    [Header("Bools")]
    [SerializeField] private bool isRoaming;

    // Start is called before the first frame update
    void Start()
    {
        Setup();
    }

    private void Update()
    {
        if (groundBotHeadMovement.ReturnPlayerIsSpotted())
        {
            isRoaming = false;
            StopBot();
        }
        else
        {
            isRoaming = true;
            ResumePatrolling();
        }
    }

    void Setup()
    {
        isRoaming = true;
        InitializePatrolPoints();
        groundBotAI = GetComponent<NavMeshAgent>();
        isWaiting = false;
        currentWaypointIndex = 0;
        StartCoroutine(Patrolling());
    }

    private void InitializePatrolPoints()
    {
        patrolPoints1.AddRange(GameObject.FindGameObjectsWithTag("PatrolPoint1").Select(g => g.transform));
        patrolPoints1.AddRange(GameObject.FindGameObjectsWithTag("PatrolPoint2").Select(g => g.transform));
        patrolPoints1.AddRange(GameObject.FindGameObjectsWithTag("PatrolPoint3").Select(g => g.transform));
        patrolPoints1.AddRange(GameObject.FindGameObjectsWithTag("PatrolPoint4").Select(g => g.transform));

        patrolPoints2.AddRange(GameObject.FindGameObjectsWithTag("PatrolPoint5").Select(g => g.transform));
        patrolPoints2.AddRange(GameObject.FindGameObjectsWithTag("PatrolPoint6").Select(g => g.transform));
        patrolPoints2.AddRange(GameObject.FindGameObjectsWithTag("PatrolPoint7").Select(g => g.transform));
        patrolPoints2.AddRange(GameObject.FindGameObjectsWithTag("PatrolPoint8").Select(g => g.transform));
        patrolPoints2.AddRange(GameObject.FindGameObjectsWithTag("PatrolPoint9").Select(g => g.transform));
        patrolPoints2.AddRange(GameObject.FindGameObjectsWithTag("PatrolPoint10").Select(g => g.transform));
        patrolPoints2.AddRange(GameObject.FindGameObjectsWithTag("PatrolPoint11").Select(g => g.transform));
        patrolPoints2.AddRange(GameObject.FindGameObjectsWithTag("PatrolPoint12").Select(g => g.transform));

        ShufflePatrolPoints(patrolPoints1);
        ShufflePatrolPoints(patrolPoints2);

        Debug.Log($"PatrolPoints1 count: {patrolPoints1.Count}");
        Debug.Log($"PatrolPoints2 count: {patrolPoints2.Count}");
    }

    private void ShufflePatrolPoints(List<Transform> patrolPoints)
    {
        for (int i = 0; i < patrolPoints.Count; i++)
        {
            Transform temp = patrolPoints[i];
            int randomIndex = Random.Range(i, patrolPoints.Count);
            patrolPoints[i] = patrolPoints[randomIndex];
            patrolPoints[randomIndex] = temp;
        }
    }

    private void StopBot()
    {
        isWaiting = true;
        groundBotAI.isStopped = true;
        groundBotAI.velocity = Vector3.zero;
    }

    public void ResumePatrolling()
    {
        if (!isWaiting) return;

        isWaiting = false;
        groundBotAI.isStopped = false;
        StartCoroutine(Patrolling());
    }

    private IEnumerator Patrolling()
    {
        if(isRoaming)
        {
            List<Transform> patrolPoints = gameObject.name == "GroundBotGroup2" ? patrolPoints1 : patrolPoints2;

            if (patrolPoints != null && patrolPoints.Count > 0)
            {
                while (!isWaiting)
                {
                    groundBotAI.SetDestination(patrolPoints[currentWaypointIndex].position);

                    // Wait until the bot reaches the waypoint
                    while (!isWaiting && groundBotAI.remainingDistance > groundBotAI.stoppingDistance)
                    {
                        yield return null;
                    }

                    yield return new WaitForSeconds(5.0f); // Wait at the waypoint

                    currentWaypointIndex = (currentWaypointIndex + 1) % patrolPoints.Count;
                }
            }
            else
            {
                Debug.LogError("Patrol points not properly set or empty.");
            }
        }

    }

    public void SetGroundBotHeadMovement(GroundBotHeadMovement value)
    {
        this.groundBotHeadMovement = value;
    }

    public void SetRoamingStatus(bool value)
    {
        isRoaming = value;
    }
}
