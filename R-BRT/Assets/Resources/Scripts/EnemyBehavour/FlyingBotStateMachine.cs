using System.Collections;
using UnityEngine;

public class FlyingBotStateMachine : MonoBehaviour
{
    [Header("GameObjects")]
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject playerCamera;
    [SerializeField] private GameObject flyingBotHead;
    [SerializeField] private GameObject fieldOfView;

    [Header("Renderer")]
    [SerializeField] private Renderer flyingBotHeadColor;
    [SerializeField] private Renderer fieldOfViewRenderer;

    [Header("Transform")]
    [SerializeField] private Transform playerCameraTransform;
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

    [Header("Materials")]
    [SerializeField] private Material red;
    [SerializeField] private Material fieldOfViewRed;

    private Quaternion startRotation;
    private Quaternion endRotation;
    private float lerpTime = 0f;
    private float lerpDuration = .25f;
    private bool isLerping = false;

    public FlyingState currentState;

    public enum FlyingState
    {
        patrolling,
        scanning,
        playerCaught,
    }

    void Start()
    {
        Setup();
        
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
        
        patrolSpeed = 3.5f;
        patrolling = true;
        player = GameManager.instance.ReturnPlayer();
        playerController = GameManager.instance.ReturnPlayerController();
        playerCamera = GameManager.instance.ReturnPlayerCamera();
        playerCameraTransform = playerCamera.transform;
        gameOverScreen = GameManager.instance.ReturnGameOver();
        enemyFlyingBotFieldOfView = GetComponentInChildren<EnemyFlyingBotFieldOfView>();

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

        flyingBotHeadColor.material = red;
        fieldOfViewRenderer.material = fieldOfViewRed;
        currentState = FlyingState.patrolling;
    }

    private void AssignPatrolPoints(string pointAName, string pointBName)
    {
        flyingBotOnePatrolPointA = GameObject.Find(pointAName).transform;
        flyingBotOnePatrolPointB = GameObject.Find(pointBName).transform;
    }

    private IEnumerator PatrolRoutine()
    {
        while (true)
        {
            if (currentState == FlyingState.patrolling && patrolling == true)
            {
                patrolling = false;
                yield return MoveToPoint(flyingBotOnePatrolPointA.position);
                yield return new WaitForSeconds(2.5f);
                yield return RotateBotGlobal(180);
                yield return MoveToPoint(flyingBotOnePatrolPointB.position);
                yield return new WaitForSeconds(2.5f);
                yield return RotateBotGlobal(180);
                patrolling = true;
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

                if(gameObject.name == "FlyingBotGroup1Lobby1" || gameObject.name == "FlyingBotGroup1Lobby2" || gameObject.name == "FlyingBotGroup1Lobby3" || gameObject.name == "FlyingBotGroup1Lobby4")

                StartCoroutine(PatrolRoutine());


                break;

            case FlyingState.scanning:

                Debug.Log(gameObject.name.ToString() + "Changing to scanning state");
                StopAllCoroutines();

                break;
            case FlyingState.playerCaught:
                HandlePlayerCaught();
                break;
            default:
                currentState = FlyingState.patrolling;
                break;
        }
    }

    private void HandlePlayerCaught()
    {
        gameOverScreen.ReturnGameOverPanel().SetActive(true);
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
        if (this.patrolling == false)
        {
            patrolling = value;
        }
    }
}
