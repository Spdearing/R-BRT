using UnityEngine;

public class SpiderBotStateMachine : MonoBehaviour
{
    [Header("Transform")]
    [SerializeField] private Transform playerCameraTransform;

    [Header("GameObjects")]
    [SerializeField] private GameObject playerCamera;

    [Header("Scripts")]
    [SerializeField] private PlayerController playerController;
    [SerializeField] private PlayerDetectionState playerDetectionState;
    [SerializeField] private GameOverScreen gameOverScreen;
    [SerializeField] private EnemySpiderBotFieldOfView enemySpiderBotFieldOfView;



    [Header("Renderer")]
    [SerializeField] private Renderer spiderBotEyeColor;
    [SerializeField] private Renderer fieldOfViewRenderer;

    [Header("Materials")]
    [SerializeField] private Material red;
    [SerializeField] private Material fieldOfViewRed;

    [Header("Quaternions")]
    [SerializeField] private Quaternion startRotation;
    [SerializeField] private Quaternion endRotation;

    [Header("Floats")]
    [SerializeField] private float lerpTime = 0f;
    [SerializeField] private float lerpDuration = .25f;

    [Header("Bools")]
    [SerializeField] private bool isLerping = false;


    public SpiderState currentState;

    public enum SpiderState
    {
        patrolling,
        scanning,
        playerCaught,
    }

    // Start is called before the first frame update
    void Start()
    {
        playerController = GameManager.instance.ReturnPlayerController();
        playerDetectionState = GameManager.instance.ReturnPlayerDetectionState();
        gameOverScreen = GameManager.instance.ReturnGameOver();
        enemySpiderBotFieldOfView = GetComponentInChildren<EnemySpiderBotFieldOfView>();
        playerCamera = GameManager.instance.ReturnPlayerCamera();
        playerCameraTransform = GameManager.instance.ReturnCameraTransform();
        spiderBotEyeColor.material = red;
        fieldOfViewRenderer.material = fieldOfViewRed;
        currentState = SpiderState.patrolling;
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

    public void ChangeBehavior(SpiderState newState)
    {
        currentState = newState;
    }

    void UpdateBehavior()
    {
        switch (currentState)
        {
            case SpiderState.patrolling:

                break;

            case SpiderState.scanning:

                //playerDetectionState.ChangeDetectionState(PlayerDetectionState.DetectionState.beingDetected);

                break;

            case SpiderState.playerCaught:

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

                currentState = SpiderState.patrolling;

                break;
        }
    }
}
