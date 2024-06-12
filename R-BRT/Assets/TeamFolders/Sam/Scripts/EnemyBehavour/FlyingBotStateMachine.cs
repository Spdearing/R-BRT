using System.Collections;
using UnityEngine;
using UnityEngine.AI;


public class FlyingBotStateMachine : MonoBehaviour
{
    [Header("Nav Mesh")]
    private NavMeshAgent navGhost;

    [Header("GameObjects")]
    [SerializeField] GameObject player;
    
    [Header("Scripts")]
    [SerializeField] GameOverScreen gameOverScreen;


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

                break;

            case FlyingState.scanning:
               
                break;

            case FlyingState.playerCaught:

            gameOverScreen.ReturnGameOverPanel().SetActive(true);
            Time.timeScale = 0.0f;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;


                break;

            default:
             
                    currentState = FlyingState.patrolling;
               
                break;
        }
    }
}
