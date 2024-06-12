using System.Collections;
using UnityEngine;
using UnityEngine.AI;


public class SpiderBotStateMachine : MonoBehaviour
{
    [Header("GameObjects")]
    [SerializeField] GameObject player;

    [Header("Scripts")]
    [SerializeField] GameOverScreen gameOverScreen;


    public IdleState currentState;

    public enum IdleState
    {
        patrolling,
        scanning,
        playerCaught,
    }

    // Start is called before the first frame update
    void Start()
    {
        currentState = IdleState.patrolling;
    }
    private void Update()
    {
        UpdateBehavior();
    }

    public void ChangeBehavior(IdleState newState)
    {
        currentState = newState;
    }

    void UpdateBehavior()
    {
        switch (currentState)
        {
            case IdleState.patrolling:

                break;

            case IdleState.scanning:
               
                break;

            case IdleState.playerCaught:

            gameOverScreen.ReturnGameOverPanel().SetActive(true);
            Time.timeScale = 0.0f;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;


                break;

            default:
             
                    currentState = IdleState.patrolling;
               
                break;
        }
    }
}
