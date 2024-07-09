using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class GroundBotStateMachine : MonoBehaviour
{
    [Header("Transform")]
    [SerializeField] private Transform playerCameraTransform;

    [Header("Scripts")]
    [SerializeField] GameOverScreen gameOverScreen;
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
        playerCameraTransform = GameManager.instance.ReturnCameraTransform();
        gameOverScreen = GameManager.instance.ReturnGameOver();
        playerController = GameManager.instance.ReturnPlayerController();
        enemyGroundBotFieldOfView = GetComponentInChildren<EnemyGroundBotFieldOfView>();
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

                if(gameObject.name == "GroundBotGroup3SecondFloor1" || gameObject.name == "GroundBotGroup3SecondFloor2" || gameObject.name == "GroundBotGroup3SecondFloor3" || gameObject.name == "GroundBotGroup3SecondFloor4")
                {
                    groundBotAIMovement.StartCoroutine(groundBotAIMovement.Patrolling());
                }
                

                break;

            case BehaviorState.scanning:

                groundBotAIMovement.StopAllCoroutines();

                break;

            case BehaviorState.playerCaught:

                
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

    public BehaviorState ReturnCurrentState()
    {
        return this.currentState;
    }

    public void SetGroundBotAIMovement(GroundBotAIMovement value)
    {
        this.groundBotAIMovement = value;
    }
}
