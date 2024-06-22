using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using static FlyingBotStateMachine;

public class SpiderBotStateMachine : MonoBehaviour
{
    [Header("Transform")]
    [SerializeField] private Transform playerCameraTransform;

    [Header("GameObjects")]
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject playerCamera;

    [Header("Scripts")]
    [SerializeField] private PlayerController playerController;
    [SerializeField] private PlayerDetectionState playerDetectionState;
    [SerializeField] private GameOverScreen gameOverScreen;
    [SerializeField] private EnemySpiderBotFieldOfView enemySpiderBotFieldOfView;
    [SerializeField] private GameManager gameManager;



    [Header("Renderer")]
    [SerializeField] private Renderer spiderBotEyeColor;
    [SerializeField] private Renderer fieldOfViewRenderer;

    [Header("Materials")]
    [SerializeField] private Material lightBlue;
    [SerializeField] private Material yellow;
    [SerializeField] private Material red;
    [SerializeField] private Material fieldOfViewLightBlue;
    [SerializeField] private Material fieldOfViewYellow;
    [SerializeField] private Material fieldOfViewRed;

    [Header("Quaternions")]
    [SerializeField] private Quaternion startRotation;
    [SerializeField] private Quaternion endRotation;

    [Header("Floats")]
    [SerializeField] private float lerpTime = 0f;
    [SerializeField] private float lerpDuration = .25f;

    [Header("Bools")]
    [SerializeField] private bool isLerping = false;


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
        playerController = player.GetComponent<PlayerController>();
        gameOverScreen = GameObject.FindWithTag("Canvas").GetComponent<GameOverScreen>();
        playerDetectionState = player.GetComponent<PlayerDetectionState>();
        enemySpiderBotFieldOfView = GetComponentInChildren<EnemySpiderBotFieldOfView>();
        gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
        playerCamera = GameObject.FindGameObjectWithTag("MainCamera");
        playerCameraTransform = playerCamera.transform;
        spiderBotEyeColor.material = lightBlue;
        fieldOfViewRenderer.material = fieldOfViewLightBlue;
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
