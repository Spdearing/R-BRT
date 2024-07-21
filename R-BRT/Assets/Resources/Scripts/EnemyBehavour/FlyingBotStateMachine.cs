using System.Collections;
using UnityEngine;

public class FlyingBotStateMachine : MonoBehaviour
{
    [Header("GameObjects")]
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject playerCamera;
    [SerializeField] private GameObject flyingBotHead;
    [SerializeField] private GameObject fieldOfView;

    [Header("Transform")]
    [SerializeField] private Transform playerCameraTransform;
    [SerializeField] private Transform playerOtherTransform;
    [SerializeField] private Transform flyingBotOnePatrolPointA;
    [SerializeField] private Transform flyingBotOnePatrolPointB;

    [Header("Patrol Speed")]
    [SerializeField] private float patrolSpeed;

    [Header("Bools")]
    [SerializeField] private bool patrolling;

    [Header("Scripts")]
    [SerializeField] private GameOverScreen gameOverScreen;
    [SerializeField] private EnemyFlyingBotFieldOfView enemyFlyingBotFieldOfView;
    [SerializeField] private PlayerController playerController;

    private Quaternion startRotation;
    private Quaternion endRotation;
    private float lerpTime = 0f;
    private float lerpDuration = 0.25f;
    private bool isLerping = false;
    private Coroutine patrolRoutine;

    public FlyingState currentState;

    public enum FlyingState
    {
        patrolling,
        moving,
        scanning,
        playerCaught,
        lookingAtPlayer,
    }

    void Start()
    {
        Setup();
        if (gameObject.name == "FlyingBotGroup1Lobby1" || gameObject.name == "FlyingBotGroup1Lobby2" || gameObject.name == "FlyingBotGroup1Lobby3" || gameObject.name == "FlyingBotGroup1Lobby4")
        {
            AssignPatrolPointsBasedOnName();
            StartPatrolRoutine();
        }
        
    }

    private void OnEnable()
    {
        Setup();
        if (gameObject.name == "FlyingBotGroup1Lobby1" || gameObject.name == "FlyingBotGroup1Lobby2" || gameObject.name == "FlyingBotGroup1Lobby3" || gameObject.name == "FlyingBotGroup1Lobby4")
        {
            AssignPatrolPointsBasedOnName();
            StartPatrolRoutine();
        }
    }

    void Update()
    {
        UpdateBehavior();

        if (isLerping)
        {
            lerpTime += Time.deltaTime;
            float lerpFactor = lerpTime / lerpDuration;
            playerCameraTransform.rotation = Quaternion.Slerp(startRotation, endRotation, lerpFactor);

            if (lerpTime >= lerpDuration)
            {
                isLerping = false;
            }
        }
    }

    void Setup()
    {
        player = GameManager.instance.ReturnPlayer();
        playerController = GameManager.instance.ReturnPlayerController();
        playerCamera = GameManager.instance.ReturnPlayerCamera();
        playerOtherTransform = GameManager.instance.ReturnPlayerOtherCamera();
        playerCameraTransform = playerCamera.transform;
        gameOverScreen = GameManager.instance.ReturnGameOver();
        enemyFlyingBotFieldOfView = GetComponentInChildren<EnemyFlyingBotFieldOfView>();
        patrolSpeed = 5.0f;
        patrolling = true;
        
        
            
        currentState = FlyingState.patrolling;
    }

    private void AssignPatrolPointsBasedOnName()
    {
        string botName = gameObject.name;

        switch (botName)
        {
            case "FlyingBotGroup1Lobby1":
                AssignPatrolPoints("FlyingBotOnePatrolPointA", "FlyingBotOnePatrolPointB");
                break;
            case "FlyingBotGroup1Lobby2":
                AssignPatrolPoints("FlyingBotOnePatrolPointC", "FlyingBotOnePatrolPointD");
                break;
            case "FlyingBotGroup1Lobby3":
                AssignPatrolPoints("FlyingBotOnePatrolPointE", "FlyingBotOnePatrolPointF");
                break;
            case "FlyingBotGroup1Lobby4":
                AssignPatrolPoints("FlyingBotOnePatrolPointH", "FlyingBotOnePatrolPointG");
                break;
        }
    }

    private void AssignPatrolPoints(string pointAName, string pointBName)
    {
        flyingBotOnePatrolPointA = GameObject.Find(pointAName).transform;
        flyingBotOnePatrolPointB = GameObject.Find(pointBName).transform;
    }

    private void StartPatrolRoutine()
    {
        if (patrolRoutine != null)
        {
            StopCoroutine(patrolRoutine);
        }
        patrolRoutine = StartCoroutine(PatrolRoutine());
    }

    private IEnumerator PatrolRoutine()
    {
        while (true)
        {
            if (currentState == FlyingState.patrolling && patrolling)
            {
                ChangeBehavior(FlyingState.moving);
                patrolling = false;
                yield return MoveToPoint(flyingBotOnePatrolPointA.position);
                yield return new WaitForSeconds(.1f);
                yield return RotateBotGlobal(180);
                yield return MoveToPoint(flyingBotOnePatrolPointB.position);
                yield return new WaitForSeconds(.1f);
                yield return RotateBotGlobal(180);
                patrolling = true;
                ChangeBehavior(FlyingState.patrolling);
            }
            else
            {
                yield return null;
            }
        }
    }

    private IEnumerator MoveToPoint(Vector3 target)
    {
        while (Vector3.Distance(transform.position, target) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, patrolSpeed * Time.deltaTime);
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

    public void ChangeBehavior(FlyingState newState)
    {
        currentState = newState;
    }

    void UpdateBehavior()
    {
        switch (currentState)
        {
            case FlyingState.patrolling:

                if (gameObject.name == "FlyingBotGroup1Lobby1" || gameObject.name == "FlyingBotGroup1Lobby2" || gameObject.name == "FlyingBotGroup1Lobby3" || gameObject.name == "FlyingBotGroup1Lobby4")
                {
                    if (patrolRoutine == null)
                    {
                        StartPatrolRoutine();
                    }
                }

                break;
            case FlyingState.moving:
                break;
            case FlyingState.scanning:
                StopPatrolRoutine();
                break;
            case FlyingState.playerCaught:
                HandlePlayerCaught();
                break;
            case FlyingState.lookingAtPlayer:
                StopPatrolRoutine();
                LookAtPlayer();
                break;
            default:
                currentState = FlyingState.patrolling;
                break;
        }
    }

    private void StopPatrolRoutine()
    {
        if (patrolRoutine != null)
        {
            StopCoroutine(patrolRoutine);
            patrolRoutine = null;
        }
    }

    private void LookAtPlayer()
    {
        // Make the bot look at the player
        Vector3 directionToPlayer = player.transform.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(directionToPlayer);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * patrolSpeed);
    }

    private void HandlePlayerCaught()
    {
        gameOverScreen.ReturnGameOverPanel().SetActive(true);

        if (GameManager.instance.ReturnNewGameStatus() == true)
        {
            GameManager.instance.SetNewGameStatus(false);
        }

        playerController.SetCameraLock(true);
        startRotation = playerCameraTransform.rotation;

        if (enemyFlyingBotFieldOfView != null)
        {
            Vector3 directionToEnemy = enemyFlyingBotFieldOfView.ReturnThisEnemy().position - playerCameraTransform.position;
            endRotation = Quaternion.LookRotation(directionToEnemy);
        }
        lerpTime = 0f;
        isLerping = true;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void SetPatrollingStatus(bool value)
    {
        patrolling = value;
    }
}
