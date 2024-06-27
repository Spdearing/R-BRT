using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class GroundBotAIMovement : MonoBehaviour
{
    [Header("Nav Mesh")]
    [SerializeField] private NavMeshAgent groundBotAI;

    [Header("Transform")]
    [SerializeField] private Transform[] patrolPoints1;
    [SerializeField] private Transform[] patrolPoints2;

    [Header("Int")]
    [SerializeField] private int currentWaypointIndex;

    private bool isWaiting;

    // Start is called before the first frame update
    void Start()
    {
        groundBotAI = GetComponent<NavMeshAgent>();
        if (groundBotAI == null)
        {
            Debug.LogError("There is no AI attached to this GameObject");
            return;
        }

        // Initialize patrol points arrays (assuming they will be assigned in the Inspector)
        patrolPoints1 = new Transform[4];
        patrolPoints2 = new Transform[8];
        patrolPoints1[0] = GameObject.FindWithTag("PatrolPoint1").transform;
        patrolPoints1[1] = GameObject.FindWithTag("PatrolPoint2").transform;
        patrolPoints1[2] = GameObject.FindWithTag("PatrolPoint3").transform;
        patrolPoints1[3] = GameObject.FindWithTag("PatrolPoint4").transform;

        patrolPoints2[0] = GameObject.FindWithTag("PatrolPoint5").transform;
        patrolPoints2[1] = GameObject.FindWithTag("PatrolPoint6").transform;
        patrolPoints2[2] = GameObject.FindWithTag("PatrolPoint7").transform;
        patrolPoints2[3] = GameObject.FindWithTag("PatrolPoint8").transform;
        patrolPoints2[4] = GameObject.FindWithTag("PatrolPoint9").transform;
        patrolPoints2[5] = GameObject.FindWithTag("PatrolPoint10").transform;
        patrolPoints2[6] = GameObject.FindWithTag("PatrolPoint11").transform;
        patrolPoints2[7] = GameObject.FindWithTag("PatrolPoint12").transform;

        isWaiting = false;

        // Start the patrolling process
        Patrolling();
    }

    public void Patrolling()
    {
        Transform[] patrolPoints = null;

        if (gameObject.name == "GroundBotGroup2")
        {
            patrolPoints = patrolPoints1;
        }
        else if (gameObject.name == "GroundBotGroup4")
        {
            patrolPoints = patrolPoints2;
        }

        if (patrolPoints != null && !isWaiting)
        {
            StartCoroutine(MoveToNextWaypoint(patrolPoints));
        }
    }

    private IEnumerator MoveToNextWaypoint(Transform[] patrolPoints)
    {
        isWaiting = true;

        groundBotAI.SetDestination(patrolPoints[currentWaypointIndex].position);

        
        while (groundBotAI.remainingDistance > 0.1f)
        {
            yield return null;
        }

       
        yield return new WaitForSeconds(3f);

        
        currentWaypointIndex = (currentWaypointIndex + 1) % patrolPoints.Length;

        isWaiting = false;

        
        Patrolling();
    }
}
