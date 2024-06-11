using System.Collections;
using UnityEngine;
using UnityEngine.AI;


public class SpiderBotStateMachine : MonoBehaviour
{
    [Header("GameObjects")]
    [SerializeField] GameObject player;
    

    [Header("Vector3")]
    [SerializeField] Vector3 startingLocation;
    

    [Header("Rotations")]
    [SerializeField] private Quaternion startRotation;
    [SerializeField] private Quaternion targetRotation;

    [Header("Transform")]
    //[SerializeField] Transform endingLocationObject;

    [Header("Floats")]
    [SerializeField] private float robotHeight;
    [SerializeField] private float duration;
    [SerializeField] private float elapsedTime;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float chaseSpeed; 
    [SerializeField] private float stoppingDistance = 2f;

    [Header("Rigidbody")]
    private Rigidbody rb;

    [Header("Scripts")]
    [SerializeField] GameOverScreen gameOverScreen;


    public IdleState currentState;

    public enum IdleState
    {
        patrolling,
        scanning,
        playerCaught,
        reset

    }

    // Start is called before the first frame update
    void Start()
    {
        stoppingDistance = 1.0f;
        chaseSpeed = 2.5f;
        rotationSpeed = 2.0f;
        duration = 20.0f;
        robotHeight = 1.329f;
        startingLocation = transform.position;
        elapsedTime = 0f;
        startRotation = transform.rotation;
        currentState = IdleState.patrolling;
    }

    // Update is called once per frame
    void Update()
    {
        
        
    }

    private void FixedUpdate()
    {
        UpdateBehaviour();
    }

    public void ChangeBehaviour(IdleState newState)
    {
        currentState = newState;
    }

    void UpdateBehaviour()
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

            case IdleState.reset:

                float returnSpeed = 10.0f;

                transform.position = Vector3.Lerp(transform.position, startingLocation, returnSpeed);

                break;

            default:
             
                    currentState = IdleState.patrolling;
               
                break;
        }
    }

    IEnumerator RotateToFaceDirection()
    {
        float rotationTime = 1.0f; // Duration to complete the rotation
        float elapsedRotationTime = 0f;
        Quaternion initialRotation = transform.rotation;

        while (elapsedRotationTime < rotationTime)
        {
            elapsedRotationTime += Time.deltaTime;
            transform.rotation = Quaternion.Lerp(initialRotation, targetRotation, elapsedRotationTime / rotationTime);
            yield return null;
        }
    }
}
