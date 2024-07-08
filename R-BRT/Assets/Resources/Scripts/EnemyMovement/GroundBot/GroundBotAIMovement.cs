using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class GroundBotAIMovement : MonoBehaviour
{
    [Header("Nav Mesh")]
    [SerializeField] private NavMeshAgent groundBotAI;

    [Header("Scripts")]
    [SerializeField] GroundBotHeadMovement groundBotHeadMovement;
    [SerializeField] GroundBotStateMachine groundBotStateMachine;

    [Header("Transforms")]
    [SerializeField] private Transform roamingPointA;
    [SerializeField] private Transform roamingPointB;

    [Header("Detection")]
    [SerializeField] private float detectionSpeed = 1.0f;
    [SerializeField] private float crouchDetectionMultiplier = 0.5f;

    [Header("Bools")]
    [SerializeField] private bool isRoaming;
    [SerializeField] private bool isWaiting;

    private void Start()
    {
        Setup();
        StartCoroutine(Patrolling());
    }

    private void Update()
    {
        if (groundBotHeadMovement.ReturnPlayerIsSpotted())
        {
            isRoaming = false;
            StopBot();
        }
    }

    private void Setup()
    {
        groundBotAI = GetComponent<NavMeshAgent>();

        string groundBotName = gameObject.name;

        switch (groundBotName)
        {
            case "GroundBotGroup3SecondFloor1":
                InitializePatrolPoints("RoamingPointA", "RoamingPointB");
                break;
            case "GroundBotGroup3SecondFloor2":
                InitializePatrolPoints("RoamingPointC", "RoamingPointD");
                break;
            case "GroundBotGroup3SecondFloor3":
                InitializePatrolPoints("RoamingPointE", "RoamingPointF");
                break;
            case "GroundBotGroup3SecondFloor4":
                InitializePatrolPoints("RoamingPointG", "RoamingPointH");
                break;
        }



        isRoaming = true;
        isWaiting = false;
        //StartCoroutine(Patrolling());
    }

    private void InitializePatrolPoints(string pointAName, string pointBName)
    {
        roamingPointA = GameObject.Find(pointAName).transform;
        roamingPointB = GameObject.Find(pointBName).transform;
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
        //StartCoroutine(Patrolling());
    }

    public IEnumerator Patrolling()
    {
        while (true)
        {
            if(isRoaming)
            {
                isRoaming = false;
                yield return MoveToPoint(roamingPointA.position);
                yield return new WaitForSeconds(4.0f);
                yield return RotateBotGlobal(180);
                yield return MoveToPoint(roamingPointB.position);
                yield return new WaitForSeconds(4.0f);
                yield return RotateBotGlobal(180);
                isRoaming = true;
            }
            else
            {
                yield return null;
            }
             
        }
    }

    private IEnumerator MoveToPoint(Vector3 target)
    {
        groundBotAI.SetDestination(target);

        while (groundBotAI.pathPending || groundBotAI.remainingDistance > groundBotAI.stoppingDistance)
        {
            yield return null;
        }
    }

    private IEnumerator RotateBotGlobal(float angle)
    {
        Quaternion initialRotation = transform.rotation;
        Quaternion targetRotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y + angle, transform.eulerAngles.z);
        float rotateTime = 0f;
        float rotateDuration = 0.5f;

        while (rotateTime < rotateDuration)
        {
            rotateTime += Time.deltaTime;
            transform.rotation = Quaternion.Slerp(initialRotation, targetRotation, rotateTime / rotateDuration);
            yield return null;
        }
        transform.rotation = targetRotation;
    }

    public void SetGroundBotHeadMovement(GroundBotHeadMovement value)
    {
        groundBotHeadMovement = value;
    }

    public void SetRoamingStatus(bool value)
    {
        isRoaming = value;
    }

    public void SetStateMachine(GroundBotStateMachine value)
    {
        this.groundBotStateMachine = value;
    }

    public bool ReturnIsRoaming()
    {
        return this.isRoaming;
    }
}
