using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class GroundBotStateMachine : MonoBehaviour
{
    [Header("GameObjects")]
    [SerializeField] GameObject playerCamera;

    [Header("Transform")]
    [SerializeField] private Transform playerCameraTransform;


    [Header("Scripts")]
    [SerializeField] GameOverScreen gameOverScreen;
    [SerializeField] PlayerDetectionState playerDetectionState;
    [SerializeField] PlayerController playerController;
    [SerializeField] EnemyGroundBotFieldOfView enemyGroundBotFieldOfView;
    [SerializeField] GroundBotAIMovement groundBotAIMovement;


    private Quaternion startRotation;
    private Quaternion endRotation;
    private float lerpTime = 0f;
    private float lerpDuration = .25f;

    private bool isLerping = false;

    public BehaviorState currentState;

    public enum BehaviorState
    {
        patrolling,
        scanning,
        playerCaught,
    }

    // Start is called before the first frame update
    void Start()
    {
        Setup();
    }

    void Setup()
    {
        Debug.Log("GroundBotStateMachine popping");
        groundBotAIMovement = GetComponent<GroundBotAIMovement>();
        playerCameraTransform = GameManager.instance.ReturnCameraTransform();
        gameOverScreen = GameManager.instance.ReturnGameOver();
        playerController = GameManager.instance.ReturnPlayerController();
        enemyGroundBotFieldOfView = GetComponentInChildren<EnemyGroundBotFieldOfView>();
        playerDetectionState = GameManager.instance.ReturnPlayerDetectionState();
        currentState = BehaviorState.patrolling;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateBehavior();

        if (isLerping)
        {
            lerpTime += Time.deltaTime;
            float lerpFactor = lerpTime / lerpDuration;
            playerCameraTransform.rotation = Quaternion.Slerp(startRotation, endRotation, lerpFactor);

            // Stop lerping after the duration is complete
            if (lerpTime >= lerpDuration)
            {
                isLerping = false;
            }
        }
    }

    private void FixedUpdate()
    {
        UpdateBehavior();
    }

    public void ChangeBehavior(BehaviorState newState)
    {
        currentState = newState;
    }

    void UpdateBehavior()
    {
        switch (currentState)
        {
            case BehaviorState.patrolling:

                if (gameObject.name == "GroundBotGroup2" || gameObject.name == "GroundBotGroup4")
                {
                    groundBotAIMovement.Patrolling();
                }
                break;

            case BehaviorState.scanning:

                playerDetectionState.ChangeDetectionState(PlayerDetectionState.DetectionState.beingDetected);

                break;

            case BehaviorState.playerCaught:

                Debug.Log("inside player Caught");
                gameOverScreen.ReturnGameOverPanel().SetActive(true);

                playerController.SetCameraLock(true);

                startRotation = playerCameraTransform.rotation;

                if (enemyGroundBotFieldOfView != null)
                {
                    Vector3 directionToEnemy = enemyGroundBotFieldOfView.ReturnThisEnemy().position - playerCameraTransform.position;
                    endRotation = Quaternion.LookRotation(directionToEnemy);
                }
                lerpTime = 0f;
                isLerping = true;

                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;

                break;

            default:

                currentState = BehaviorState.patrolling;

                break;
        }
    }
}
