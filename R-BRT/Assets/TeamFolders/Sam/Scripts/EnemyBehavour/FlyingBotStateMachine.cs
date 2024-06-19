using System.Collections;
using UnityEngine;
using UnityEngine.AI;


public class FlyingBotStateMachine : MonoBehaviour
{

    [Header("")]
    [SerializeField] private Transform playerCamera;

    [Header("GameObjects")]
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject flyingBotHead;
    [SerializeField] private GameObject fieldOfView;

    [Header("Renderer")]
    [SerializeField] private Renderer flyingBotHeadColor;
    [SerializeField] private Renderer fieldOfViewRenderer;


    [Header("Scripts")]
    [SerializeField] private GameOverScreen gameOverScreen;
    [SerializeField] private EnemyFieldOfView enemyFieldOfView;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private PlayerController playerController;

    [Header("Materials")]
    [SerializeField] private Material lightBlue;
    [SerializeField] private Material yellow;
    [SerializeField] private Material red;
    [SerializeField] private Material fieldOfViewLightBlue;
    [SerializeField] private Material fieldOfViewYellow;
    [SerializeField] private Material fieldOfViewRed;


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
        currentState = FlyingState.patrolling;
        flyingBotHeadColor.material = lightBlue;
        fieldOfViewRenderer.material = lightBlue;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateBehavior();
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

                flyingBotHeadColor.material = lightBlue;
                fieldOfViewRenderer.material = lightBlue;

                break;

            case FlyingState.scanning:

                flyingBotHeadColor.material = yellow;
                fieldOfViewRenderer.material = yellow;

                break;

            case FlyingState.playerCaught:

                flyingBotHeadColor.material = red;
                fieldOfViewRenderer.material = red;

                playerController.SetCameraLock(true);
                playerCamera.LookAt(transform.position);

                gameOverScreen.ReturnGameOverPanel().SetActive(true);
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;


                break;

            default:
             
                    currentState = FlyingState.patrolling;
               
                break;
        }
    }
}
