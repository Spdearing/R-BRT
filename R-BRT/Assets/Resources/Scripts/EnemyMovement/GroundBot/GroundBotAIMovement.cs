using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GroundBotAIMovement : MonoBehaviour
{

    [Header("Nav Mesh")]
    [SerializeField] private NavMeshAgent groundBotAI;

    [Header("Transform")]
    [SerializeField] static private Transform[] patrolPoints1;

    [Header("Int")]
    [SerializeField] private int currentWaypointIndex;



    // Start is called before the first frame update
    void Start()
    {
        //groundBotAI = gameObject.GetComponent<NavMeshAgent>();
        patrolPoints1 = new Transform[4];   
        patrolPoints1[0] = GameObject.FindWithTag("PatrolPoint1").transform;
        patrolPoints1[1] = GameObject.FindWithTag("PatrolPoint2").transform;
        patrolPoints1[2] = GameObject.FindWithTag("PatrolPoint3").transform;
        patrolPoints1[3] = GameObject.FindWithTag("PatrolPoint4").transform;
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Patrolling()
    {
        if(gameObject.name == "GroundBotGroup2")
        {
            if (groundBotAI.remainingDistance < 0.1f)
            {
                currentWaypointIndex = UnityEngine.Random.Range(0, patrolPoints1.Length);

                groundBotAI.SetDestination(patrolPoints1[currentWaypointIndex].position);
            }
        }
    }
}
