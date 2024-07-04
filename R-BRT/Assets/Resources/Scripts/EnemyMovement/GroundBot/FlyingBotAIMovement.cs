using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class FlyingBotAIMovement : MonoBehaviour
{
    [Header("Nav Mesh")]
    [SerializeField] private NavMeshAgent groundBotAI;

    [Header("Transforms")]
    [SerializeField] private List<Transform> patrolPoints1 = new List<Transform>();
    [SerializeField] private List<Transform> patrolPoints2 = new List<Transform>();

    [Header("Int")]
    [SerializeField] private int currentWaypointIndex;

    private bool isWaiting;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("GroundBotAI popping");
        InitializePatrolPoints();

        groundBotAI = GetComponent<NavMeshAgent>();
        if (groundBotAI == null)
        {
            Debug.LogError("There is no AI attached to this GameObject");
            return;
        }

        isWaiting = false;
        currentWaypointIndex = 0;

        // Start the patrolling process
        Patrolling();
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

    public void Patrolling()
    {
        List<Transform> patrolPoints = null;

        if (gameObject.name == "GroundBotGroup2")
        {
            patrolPoints = patrolPoints1;
        }
        else if (gameObject.name == "GroundBotGroup4")
        {
            patrolPoints = patrolPoints2;
        }

        if (patrolPoints != null && patrolPoints.Count > 0 && !isWaiting)
        {
            StartCoroutine(MoveToNextWaypoint(patrolPoints));
        }
        else
        {
            Debug.LogError("Patrol points not properly set or empty.");
        }
    }

    private IEnumerator MoveToNextWaypoint(List<Transform> patrolPoints)
    {
        while (true)
        {
            isWaiting = true;

            if (currentWaypointIndex < patrolPoints.Count)
            {
                groundBotAI.SetDestination(patrolPoints[currentWaypointIndex].position);
            }
            else
            {
                Debug.LogError("currentWaypointIndex out of range.");
            }

            yield return new WaitForSeconds(3.5f);

            currentWaypointIndex = (currentWaypointIndex + 1) % patrolPoints.Count;

            isWaiting = false;
        }
    }
}
