using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class SpiderBotStateMachine : MonoBehaviour
{
    [Header("GameObjects")]
    [SerializeField] GameObject player;
    [SerializeField] GameObject playerCamera;

    [Header("Scripts")]
    [SerializeField] GameOverScreen gameOverScreen;
    [SerializeField] PlayerDetectionState playerDetectionState;
    [SerializeField] DetectionMeter detectionMeter;
    [SerializeField] EnemySpiderBotFieldOfView enemySpiderBotFieldOfView;
    [SerializeField] PlayerController playerController;

    [Header("Transform")]
    [SerializeField] private Transform playerCameraTransform;


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
        player = GameObject.FindWithTag("Player");
        playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        playerDetectionState = GameObject.FindWithTag("Player").GetComponent<PlayerDetectionState>();
        playerCamera = GameObject.FindGameObjectWithTag("MainCamera");
        playerCameraTransform = playerCamera.transform;
        gameOverScreen = GameObject.FindWithTag("Canvas").GetComponent<GameOverScreen>();
        enemySpiderBotFieldOfView = GetComponentInChildren<EnemySpiderBotFieldOfView>();
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

                break;

            case BehaviorState.scanning:

                playerDetectionState.ChangeDetectionState(PlayerDetectionState.DetectionState.beingDetected);

                break;

            case BehaviorState.playerCaught:

                Debug.Log("inside player Caught");
                gameOverScreen.ReturnGameOverPanel().SetActive(true);

                playerController.SetCameraLock(true);

                startRotation = playerCameraTransform.rotation;

                if (enemySpiderBotFieldOfView != null)
                {
                    Vector3 directionToEnemy = enemySpiderBotFieldOfView.ReturnThisEnemy().position - playerCameraTransform.position;
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
