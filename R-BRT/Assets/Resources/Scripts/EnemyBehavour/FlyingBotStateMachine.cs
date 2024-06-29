using System.Collections;
using UnityEngine;
using UnityEngine.AI;


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


    [Header("Scripts")]
    [SerializeField] private GameOverScreen gameOverScreen;
    [SerializeField] private EnemyFlyingBotFieldOfView enemyFlyingBotFieldOfView;
    [SerializeField] private GameManager gameManager;
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

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
        player = GameObject.FindWithTag("Player");
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        playerCamera = GameObject.FindGameObjectWithTag("MainCamera");
        playerCameraTransform = playerCamera.transform;
        gameOverScreen = GameObject.FindWithTag("Canvas").GetComponent<GameOverScreen>();   
        enemyFlyingBotFieldOfView = GetComponentInChildren<EnemyFlyingBotFieldOfView>();
        flyingBotHeadColor.material = red;
        fieldOfViewRenderer.material = fieldOfViewRed;
        currentState = FlyingState.patrolling;
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


    public void ChangeBehavior(FlyingState newState)
    {
        currentState = newState;
    }

    void UpdateBehavior()
    {
        switch (currentState)
        {
            case FlyingState.patrolling:



                break;

            case FlyingState.scanning:



                break;

            case FlyingState.playerCaught:

                Debug.Log("inside player Caught");
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


                break;

            default:
             
                    currentState = FlyingState.patrolling;
               
                break;
        }
    }
}
