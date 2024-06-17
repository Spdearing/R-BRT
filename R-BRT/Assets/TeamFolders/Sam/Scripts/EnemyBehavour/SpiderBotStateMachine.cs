using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class SpiderBotStateMachine : MonoBehaviour
{
    [Header("GameObjects")]
    [SerializeField] GameObject player;

    [Header("Scripts")]
    [SerializeField] GameOverScreen gameOverScreen;
    [SerializeField] PlayerDetectionState playerDetectionState;
    [SerializeField] DetectionMeter detectionMeter;

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
        currentState = BehaviorState.patrolling;
        playerDetectionState = GameObject.Find("Player").GetComponent<PlayerDetectionState>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateBehavior();
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

                gameOverScreen.ReturnGameOverPanel().SetActive(true);
                Time.timeScale = 0.0f;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;

                break;

            default:

                currentState = BehaviorState.patrolling;

                break;
        }
    }
}
